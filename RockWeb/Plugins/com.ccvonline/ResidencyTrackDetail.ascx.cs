//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Linq;
using System.Web.UI;
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
    public partial class ResidencyTrackDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "ResidencyTrackId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "ResidencyTrackId", int.Parse( itemId ) );
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
            ResidencyService<ResidencyPeriod> service = new ResidencyService<ResidencyPeriod>();
            var list = service.Queryable().OrderBy( a => a.Name ).ToList();
            ddlPeriod.DataSource = list;
            ddlPeriod.DataBind();
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
            ResidencyTrack residencyTrack;
            ResidencyService<ResidencyTrack> residencyTrackService = new ResidencyService<ResidencyTrack>();

            int residencyTrackId = int.Parse( hfResidencyTrackId.Value );

            if ( residencyTrackId == 0 )
            {
                residencyTrack = new ResidencyTrack();
                residencyTrackService.Add( residencyTrack, CurrentPersonId );
            }
            else
            {
                residencyTrack = residencyTrackService.Get( residencyTrackId );
            }

            residencyTrack.Name = tbName.Text;
            residencyTrack.Description = tbDescription.Text;
            residencyTrack.ResidencyPeriodId = ddlPeriod.SelectedValueAsInt().Value;

            // check for duplicates within Period
            if ( residencyTrackService.Queryable().Count( a => a.Name.Equals( residencyTrack.Name, StringComparison.OrdinalIgnoreCase ) && a.ResidencyPeriodId.Equals( residencyTrack.ResidencyPeriodId ) && !a.Id.Equals( residencyTrack.Id ) ) > 0 )
            {
                nbWarningMessage.Text = WarningMessage.DuplicateFoundMessage( "name", ResidencyTrack.FriendlyTypeName );
                return;
            }

            if ( !residencyTrack.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyTrackService.Save( residencyTrack, CurrentPersonId );
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
            if ( itemKey != "ResidencyTrackId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            ResidencyTrack residencyTrack = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyTrack = new ResidencyService<ResidencyTrack>().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( ResidencyTrack.FriendlyTypeName );
            }
            else
            {
                residencyTrack = new ResidencyTrack { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( ResidencyTrack.FriendlyTypeName );
            }

            hfResidencyTrackId.Value = residencyTrack.Id.ToString();

            LoadDropDowns();

            tbName.Text = residencyTrack.Name;
            tbDescription.Text = residencyTrack.Description;
            ddlPeriod.SetValue( residencyTrack.ResidencyPeriodId );

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ResidencyTrack.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( ResidencyTrack.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            tbName.ReadOnly = readOnly;
            tbDescription.ReadOnly = readOnly;
            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}