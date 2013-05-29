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

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    [LinkedPage("Residency Competency Page")]
    public partial class ResidencyProjectDetail : RockBlock, IDetailBlock
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
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ResidencyService<ResidencyCompetency> service = new ResidencyService<ResidencyCompetency>();
            var list = service.Queryable().OrderBy( a => a.ResidencyTrack.ResidencyPeriod.Name ).ThenBy( a => a.ResidencyTrack.Name ).ThenBy( a => a.Name ).ToList();
            ddlCompetency.Items.Clear();
            foreach ( var item in list )
            {
                ddlCompetency.Items.Add( new ListItem( string.Format( "{0} {1} - {2}", item.ResidencyTrack.ResidencyPeriod.Name, item.ResidencyTrack.Name, item.Name ), item.Id.ToString() ) );
            }
        }

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
                NavigateToParentPage();
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<ResidencyProject> service = new ResidencyService<ResidencyProject>();
                ResidencyProject item = service.Get( hfResidencyProjectId.ValueAsInt() );
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
            ResidencyService<ResidencyProject> service = new ResidencyService<ResidencyProject>();
            ResidencyProject item = service.Get( hfResidencyProjectId.ValueAsInt() );
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
            ResidencyProject residencyProject;
            ResidencyService<ResidencyProject> residencyProjectService = new ResidencyService<ResidencyProject>();

            int ResidencyProjectId = int.Parse( hfResidencyProjectId.Value ); ;

            if ( ResidencyProjectId == 0 )
            {
                residencyProject = new ResidencyProject();
                residencyProjectService.Add( residencyProject, CurrentPersonId );
            }
            else
            {
                residencyProject = residencyProjectService.Get( ResidencyProjectId );
            }

            residencyProject.Name = tbName.Text;
            residencyProject.Description = tbDescription.Text;
            residencyProject.ResidencyCompetencyId = ddlCompetency.SelectedValueAsInt().Value;
            residencyProject.MinAssignmentCountDefault = tbMinAssignmentCountDefault.Text.AsInteger( false );

            // check for duplicates
            if ( residencyProjectService.Queryable().Count( a => a.Name.Equals( residencyProject.Name, StringComparison.OrdinalIgnoreCase ) && !a.Id.Equals( residencyProject.Id ) ) > 0 )
            {
                nbWarningMessage.Text = WarningMessage.DuplicateFoundMessage( "name", ResidencyProject.FriendlyTypeName );
                return;
            }

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

            ResidencyProject residencyProject = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyProject = new ResidencyService<ResidencyProject>().Get( itemKeyValue );
            }
            else
            {
                residencyProject = new ResidencyProject { Id = 0 };
                residencyProject.ResidencyCompetencyId = residencyCompetencyId ?? 0;
            }

            hfResidencyProjectId.Value = residencyProject.Id.ToString();

            // only enable the CompetencyPicker if no Competecy parameter was specified
            ddlCompetency.Enabled = !residencyCompetencyId.HasValue;

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ResidencyProject.FriendlyTypeName );
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
        private void ShowEditDetails( ResidencyProject residencyProject )
        {
            if ( residencyProject.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( ResidencyProject.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( ResidencyProject.FriendlyTypeName );
            }

            SetEditMode( true );

            LoadDropDowns();

            tbName.Text = residencyProject.Name;
            tbDescription.Text = residencyProject.Description;
            ddlCompetency.SetValue( residencyProject.ResidencyCompetencyId );
            tbMinAssignmentCountDefault.Text = residencyProject.MinAssignmentCountDefault.ToString();
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyProject">The residency project.</param>
        private void ShowReadonlyDetails( ResidencyProject residencyProject )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Name", residencyProject.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Description", residencyProject.Description );

            string residencyCompetencyPageGuid = this.GetAttributeValue( "ResidencyCompetencyPage" );
            string competencyHtml = residencyProject.ResidencyCompetency.Name;
            if ( !string.IsNullOrWhiteSpace( residencyCompetencyPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residencyCompetencyPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "residencyCompetencyId", residencyProject.ResidencyCompetencyId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                competencyHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyProject.ResidencyCompetency.Name );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Competency", competencyHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";

        }

        #endregion
    }
}