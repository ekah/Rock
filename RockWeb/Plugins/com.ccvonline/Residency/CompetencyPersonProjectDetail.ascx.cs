//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
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
    [LinkedPage( "Residency Competency Person Page" )]
    public partial class CompetencyPersonProjectDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "residencyCompetencyPersonProjectId" );
                string residencyCompetencyPersonId = PageParameter( "residencyCompetencyPersonId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( residencyCompetencyPersonId ) )
                    {
                        ShowDetail( "residencyCompetencyPersonProjectId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyCompetencyPersonProjectId", int.Parse( itemId ), int.Parse( residencyCompetencyPersonId ) );
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

            if ( hfResidencyCompetencyPersonProjectId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the ResidencyCompetencyPerson Detail page, return to that
                string residencyCompetencyPersonId = PageParameter( "residencyCompetencyPersonId" );
                if ( !string.IsNullOrWhiteSpace( residencyCompetencyPersonId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["residencyCompetencyPersonId"] = residencyCompetencyPersonId;
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
                ResidencyService<CompetencyPersonProject> service = new ResidencyService<CompetencyPersonProject>();
                CompetencyPersonProject item = service.Get( hfResidencyCompetencyPersonProjectId.ValueAsInt() );
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
            ResidencyService<CompetencyPersonProject> service = new ResidencyService<CompetencyPersonProject>();
            CompetencyPersonProject item = service.Get( hfResidencyCompetencyPersonProjectId.ValueAsInt() );
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
            CompetencyPersonProject residencyCompetencyPersonProject;
            ResidencyService<CompetencyPersonProject> residencyCompetencyPersonProjectService = new ResidencyService<CompetencyPersonProject>();

            int ResidencyCompetencyPersonProjectId = int.Parse( hfResidencyCompetencyPersonProjectId.Value );

            if ( ResidencyCompetencyPersonProjectId == 0 )
            {
                residencyCompetencyPersonProject = new CompetencyPersonProject();
                residencyCompetencyPersonProjectService.Add( residencyCompetencyPersonProject, CurrentPersonId );
                
                // these inputs are only editable on Add
                residencyCompetencyPersonProject.ProjectId = ddlResidencyProject.SelectedValueAsInt() ?? 0;
                residencyCompetencyPersonProject.CompetencyPersonId = hfResidencyCompetencyPersonId.ValueAsInt();
            }
            else
            {
                residencyCompetencyPersonProject = residencyCompetencyPersonProjectService.Get( ResidencyCompetencyPersonProjectId );
            }

            if ( !residencyCompetencyPersonProject.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyCompetencyPersonProjectService.Save( residencyCompetencyPersonProject, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyCompetencyPersonProjectId"] = residencyCompetencyPersonProject.Id.ToString();
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
        /// <param name="residencyCompetencyPersonId">The residency competency person id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyCompetencyPersonId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyCompetencyPersonProjectId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPersonProject residencyCompetencyPersonProject = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetencyPersonProject = new CompetencyPersonProject { Id = 0 };
                residencyCompetencyPersonProject.CompetencyPersonId = residencyCompetencyPersonId ?? 0;
                residencyCompetencyPersonProject.CompetencyPerson = new ResidencyService<CompetencyPerson>().Get( residencyCompetencyPersonProject.CompetencyPersonId );
            }

            hfResidencyCompetencyPersonProjectId.Value = residencyCompetencyPersonProject.Id.ToString();
            hfResidencyCompetencyPersonId.Value = residencyCompetencyPersonProject.CompetencyPersonId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPersonProject.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( residencyCompetencyPersonProject );
            }
            else
            {
                btnEdit.Visible = true;
                if ( residencyCompetencyPersonProject.Id > 0 )
                {
                    ShowReadonlyDetails( residencyCompetencyPersonProject );
                }
                else
                {
                    ShowEditDetails( residencyCompetencyPersonProject );
                }
            }
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            var residencyProjectQry = new ResidencyService<Project>().Queryable();

            int residencyCompetencyPersonId = hfResidencyCompetencyPersonId.ValueAsInt();
            CompetencyPerson residencyCompetencyPerson = new ResidencyService<CompetencyPerson>().Get( residencyCompetencyPersonId );
            residencyProjectQry = residencyProjectQry.Where( a => a.CompetencyId == residencyCompetencyPerson.CompetencyId );

            // list 
            List<int> assignedProjectIds = new ResidencyService<CompetencyPersonProject>().Queryable()
                .Where( a => a.CompetencyPersonId.Equals( residencyCompetencyPersonId ) )
                .Select( a => a.ProjectId ).ToList();

            var list = residencyProjectQry.Where( a => !assignedProjectIds.Contains( a.Id ) ).OrderBy( a => a.Name ).ToList();

            ddlResidencyProject.DataSource = list;
            ddlResidencyProject.DataBind();
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyCompetencyPersonProject">The residency project.</param>
        private void ShowEditDetails( CompetencyPersonProject residencyCompetencyPersonProject )
        {
            if ( residencyCompetencyPersonProject.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProject.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProject.FriendlyTypeName );
            }

            LoadDropDowns();

            SetEditMode( true );

            lblPersonName.Text = residencyCompetencyPersonProject.CompetencyPerson.Person.FullName;
            lblResidencyCompetency.Text = residencyCompetencyPersonProject.CompetencyPerson.Competency.Name;
            ddlResidencyProject.SetValue( residencyCompetencyPersonProject.ProjectId );

            if ( residencyCompetencyPersonProject.Project != null )
            {
                lblResidencyProject.Text = residencyCompetencyPersonProject.Project.Name;
            }
            else
            {
                // shouldn't happen, but just in case
                lblResidencyProject.Text = Rock.Constants.None.Text;
            }

            
            bool addMode =residencyCompetencyPersonProject.Id == 0;

            ddlResidencyProject.Visible = addMode;
            lblResidencyProject.Visible = !addMode;
            lblPersonName.Visible = true;
            lblResidencyCompetency.Visible = true;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyCompetencyPersonProject">The residency competency person project.</param>
        private void ShowReadonlyDetails( CompetencyPersonProject residencyCompetencyPersonProject )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Resident", residencyCompetencyPersonProject.CompetencyPerson.Person.FullName );
            lblMainDetails.Text += string.Format( descriptionFormat, "Period", residencyCompetencyPersonProject.Project.Competency.Track.Period.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Track", residencyCompetencyPersonProject.Project.Competency.Track.Name);
            lblMainDetails.Text += string.Format( descriptionFormat, "Project", residencyCompetencyPersonProject.Project.Name );

            string residencyCompetencyPersonPageGuid = this.GetAttributeValue( "ResidencyCompetencyPersonPage" );
            string residencyCompetencyPersonHtml = residencyCompetencyPersonProject.CompetencyPerson.Competency.Name;
            if ( !string.IsNullOrWhiteSpace( residencyCompetencyPersonPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residencyCompetencyPersonPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "residencyCompetencyPersonId", residencyCompetencyPersonProject.CompetencyPersonId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                residencyCompetencyPersonHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyCompetencyPersonProject.CompetencyPerson.Competency.Name );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Competency", residencyCompetencyPersonHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}