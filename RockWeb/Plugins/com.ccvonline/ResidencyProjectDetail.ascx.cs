//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
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
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "residencyProjectId", int.Parse( itemId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        #region Edit Events

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
            NavigateToParentPage();
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
            residencyProject.MinAssignmentCountDefault = tbMinAssignmentCountDefault.Text.AsInteger(false);

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

            NavigateToParentPage();
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyProjectId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            ResidencyProject residencyProject = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyProject = new ResidencyService<ResidencyProject>().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( ResidencyProject.FriendlyTypeName );
            }
            else
            {
                residencyProject = new ResidencyProject { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( ResidencyProject.FriendlyTypeName );
            }

            hfResidencyProjectId.Value = residencyProject.Id.ToString();

            LoadDropDowns();

            tbName.Text = residencyProject.Name;
            tbDescription.Text = residencyProject.Description;
            ddlCompetency.SetValue( residencyProject.ResidencyCompetencyId );
            tbMinAssignmentCountDefault.Text = residencyProject.MinAssignmentCountDefault.ToString();

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
                lActionTitle.Text = ActionTitle.View( ResidencyProject.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            tbName.ReadOnly = readOnly;
            tbDescription.ReadOnly = readOnly;
            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}