//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;

namespace RockWeb.Plugins.com.ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [LinkedPage("Residency Competency Page")]
    public partial class ProjectDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string itemId = PageParameter( "residencyProjectId" );
                string residencyCompetencyId = PageParameter( "residencyCompetencyId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace(residencyCompetencyId) )
                    {
                        ShowDetail( "residencyProjectId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyProjectId", int.Parse( itemId ), int.Parse(residencyCompetencyId) );
                    }
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            SetEditMode( false );

            if ( hfResidencyProjectId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the ResidencyCompetency Detail page, return to that
                string residencyCompetencyId = PageParameter( "residencyCompetencyId" );
                if ( !string.IsNullOrWhiteSpace( residencyCompetencyId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["residencyCompetencyId"] = residencyCompetencyId;
                    NavigateToParentPage( qryString );
                }
                else
                {
                    NavigateToParentPage();
                }
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<Project> service = new ResidencyService<Project>();
                Project item = service.Get( hfResidencyProjectId.ValueAsInt() );
                ShowReadonlyDetails( item );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            ResidencyService<Project> service = new ResidencyService<Project>();
            Project item = service.Get( hfResidencyProjectId.ValueAsInt() );
            ShowEditDetails( item );
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;

            DimOtherBlocks( editable );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            Project residencyProject;
            ResidencyService<Project> residencyProjectService = new ResidencyService<Project>();

            int residencyProjectId = int.Parse( hfResidencyProjectId.Value );

            if ( residencyProjectId == 0 )
            {
                residencyProject = new Project();
                residencyProjectService.Add( residencyProject, CurrentPersonId );
            }
            else
            {
                residencyProject = residencyProjectService.Get( residencyProjectId );
            }

            residencyProject.Name = tbName.Text;
            residencyProject.Description = tbDescription.Text;
            residencyProject.CompetencyId = hfResidencyCompetencyId.ValueAsInt();
            residencyProject.MinAssignmentCountDefault = tbMinAssignmentCountDefault.Text.AsInteger( false );

            if ( !residencyProject.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyProjectService.Save( residencyProject, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyProjectId"] = residencyProject.Id.ToString();
            NavigateToPage( this.CurrentPage.Guid, qryParams );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            ShowDetail( itemKey, itemKeyValue, null );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        /// <param name="residencyCompetencyId">The residency competency id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyCompetencyId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyProjectId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            Project residencyProject = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyProject = new ResidencyService<Project>().Get( itemKeyValue );
            }
            else
            {
                residencyProject = new Project { Id = 0 };
                residencyProject.CompetencyId = residencyCompetencyId ?? 0;
                residencyProject.Competency = new ResidencyService<Competency>().Get( residencyProject.CompetencyId );
            }

            hfResidencyProjectId.Value = residencyProject.Id.ToString();
            hfResidencyCompetencyId.Value = residencyProject.CompetencyId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Project.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( residencyProject );
            }
            else
            {
                btnEdit.Visible = true;
                if ( residencyProject.Id > 0 )
                {
                    ShowReadonlyDetails( residencyProject );
                }
                else
                {
                    ShowEditDetails( residencyProject );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyProject">The residency project.</param>
        private void ShowEditDetails( Project residencyProject )
        {
            if ( residencyProject.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Project.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Project.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = residencyProject.Name;
            tbDescription.Text = residencyProject.Description;
            lblPeriod.Text = residencyProject.Competency.Track.Period.Name;
            lblTrack.Text = residencyProject.Competency.Track.Name;
            lblCompetency.Text = residencyProject.Competency.Name;
            tbMinAssignmentCountDefault.Text = residencyProject.MinAssignmentCountDefault.ToString();
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyProject">The residency project.</param>
        private void ShowReadonlyDetails( Project residencyProject )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Name", residencyProject.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Description", residencyProject.Description );
            lblMainDetails.Text += string.Format( descriptionFormat, "Period", residencyProject.Competency.Track.Period.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Track", residencyProject.Competency.Track.Name );

            string residencyCompetencyPageGuid = this.GetAttributeValue( "ResidencyCompetencyPage" );
            string competencyHtml = residencyProject.Competency.Name;
            if ( !string.IsNullOrWhiteSpace( residencyCompetencyPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residencyCompetencyPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "residencyCompetencyId", residencyProject.CompetencyId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                competencyHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyProject.Competency.Name );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Competency", competencyHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}