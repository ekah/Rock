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

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyPeriodDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "residencyPeriodId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "residencyPeriodId", int.Parse( itemId ) );
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
            NavigateToParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            if ( dpStartDate.SelectedDate > dpEndDate.SelectedDate )
            {
                dpStartDate.ShowErrorMessage( WarningMessage.DateRangeEndDateBeforeStartDate() );
                return;
            }

            ResidencyPeriod residencyPeriod;
            ResidencyService<ResidencyPeriod> residencyPeriodService = new ResidencyService<ResidencyPeriod>();

            int residencyPeriodId = int.Parse( hfResidencyPeriodId.Value );

            if ( residencyPeriodId == 0 )
            {
                residencyPeriod = new ResidencyPeriod();
                residencyPeriodService.Add( residencyPeriod, CurrentPersonId );
            }
            else
            {
                residencyPeriod = residencyPeriodService.Get( residencyPeriodId );
            }

            residencyPeriod.Name = tbName.Text;
            residencyPeriod.Description = tbDescription.Text;
            residencyPeriod.StartDate = dpStartDate.SelectedDate;
            residencyPeriod.EndDate = dpEndDate.SelectedDate;

            // check for duplicates
            if ( residencyPeriodService.Queryable().Count( a => a.Name.Equals( residencyPeriod.Name, StringComparison.OrdinalIgnoreCase ) && !a.Id.Equals( residencyPeriod.Id ) ) > 0 )
            {
                nbWarningMessage.Text = WarningMessage.DuplicateFoundMessage( "name", ResidencyPeriod.FriendlyTypeName );
                return;
            }

            if ( !residencyPeriod.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyPeriodService.Save( residencyPeriod, CurrentPersonId );
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
            if ( itemKey != "residencyPeriodId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            ResidencyPeriod residencyPeriod = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyPeriod = new ResidencyService<ResidencyPeriod>().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( ResidencyPeriod.FriendlyTypeName );
            }
            else
            {
                residencyPeriod = new ResidencyPeriod { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( ResidencyPeriod.FriendlyTypeName );
            }

            hfResidencyPeriodId.Value = residencyPeriod.Id.ToString();
            tbName.Text = residencyPeriod.Name;
            tbDescription.Text = residencyPeriod.Description;
            dpStartDate.SelectedDate = residencyPeriod.StartDate;
            dpEndDate.SelectedDate = residencyPeriod.EndDate;

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ResidencyPeriod.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( ResidencyPeriod.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            tbName.ReadOnly = readOnly;
            tbDescription.ReadOnly = readOnly;
            dpStartDate.ReadOnly = readOnly;
            dpEndDate.ReadOnly = readOnly;
            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}