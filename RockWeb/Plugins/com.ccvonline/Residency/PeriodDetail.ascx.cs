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
    public partial class PeriodDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "periodId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "periodId", int.Parse( itemId ) );
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

            if ( hfPeriodId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                NavigateToParentPage();

            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<Period> service = new ResidencyService<Period>();
                Period item = service.Get( hfPeriodId.ValueAsInt() );
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
            ResidencyService<Period> service = new ResidencyService<Period>();
            Period item = service.Get( hfPeriodId.ValueAsInt() );
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
            if ( dpStartDate.SelectedDate > dpEndDate.SelectedDate )
            {
                dpStartDate.ShowErrorMessage( WarningMessage.DateRangeEndDateBeforeStartDate() );
                return;
            }

            Period period;
            ResidencyService<Period> periodService = new ResidencyService<Period>();

            int periodId = int.Parse( hfPeriodId.Value );

            if ( periodId == 0 )
            {
                period = new Period();
                periodService.Add( period, CurrentPersonId );
            }
            else
            {
                period = periodService.Get( periodId );
            }

            period.Name = tbName.Text;
            period.Description = tbDescription.Text;
            period.StartDate = dpStartDate.SelectedDate;
            period.EndDate = dpEndDate.SelectedDate;

            if ( !period.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                periodService.Save( period, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["periodId"] = period.Id.ToString();
            NavigateToPage( this.CurrentPage.Guid, qryParams );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "periodId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            Period period = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                period = new ResidencyService<Period>().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( Period.FriendlyTypeName );
            }
            else
            {
                period = new Period { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( Period.FriendlyTypeName );
            }

            hfPeriodId.Value = period.Id.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Period.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( period );
            }
            else
            {
                btnEdit.Visible = true;
                if ( period.Id > 0 )
                {
                    ShowReadonlyDetails( period );
                }
                else
                {
                    ShowEditDetails( period );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="period">The residency period.</param>
        private void ShowEditDetails( Period period )
        {
            if ( period.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Period.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Period.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = period.Name;
            tbDescription.Text = period.Description;
            dpStartDate.SelectedDate = period.StartDate;
            dpEndDate.SelectedDate = period.EndDate;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="period">The residency project.</param>
        private void ShowReadonlyDetails( Period period )
        {
            SetEditMode( false );

            lblMainDetails.Text = new DescriptionList()
                .Add("Name", period.Name)
                .Add("Description", period.Description)
                .StartSecondColumn()
                .Add("Start Date", period.StartDate)
                .Add("End Date", period.EndDate)
                .Html;
        }

        #endregion
    }
}