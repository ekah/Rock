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
            SetEditMode( false );

            if ( hfResidencyPeriodId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                NavigateToParentPage();

            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<ResidencyPeriod> service = new ResidencyService<ResidencyPeriod>();
                ResidencyPeriod item = service.Get( hfResidencyPeriodId.ValueAsInt() );
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
            ResidencyService<ResidencyPeriod> service = new ResidencyService<ResidencyPeriod>();
            ResidencyPeriod item = service.Get( hfResidencyPeriodId.ValueAsInt() );
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

            if ( !residencyPeriod.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyPeriodService.Save( residencyPeriod, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyPeriodId"] = residencyPeriod.Id.ToString();
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
                btnEdit.Visible = false;
                ShowReadonlyDetails( residencyPeriod );
            }
            else
            {
                btnEdit.Visible = true;
                if ( residencyPeriod.Id > 0 )
                {
                    ShowReadonlyDetails( residencyPeriod );
                }
                else
                {
                    ShowEditDetails( residencyPeriod );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyPeriod">The residency period.</param>
        private void ShowEditDetails( ResidencyPeriod residencyPeriod )
        {
            if ( residencyPeriod.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( ResidencyPeriod.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( ResidencyPeriod.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = residencyPeriod.Name;
            tbDescription.Text = residencyPeriod.Description;
            dpStartDate.SelectedDate = residencyPeriod.StartDate;
            dpEndDate.SelectedDate = residencyPeriod.EndDate;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyPeriod">The residency project.</param>
        private void ShowReadonlyDetails( ResidencyPeriod residencyPeriod )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Name", residencyPeriod.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Description", residencyPeriod.Description );
            lblMainDetails.Text += string.Format( descriptionFormat, "Start Date", residencyPeriod.StartDate.HasValue ? residencyPeriod.StartDate.Value.ToShortDateString() : Rock.Constants.None.TextHtml);
            lblMainDetails.Text += string.Format( descriptionFormat, "End Date", residencyPeriod.EndDate.HasValue ? residencyPeriod.EndDate.Value.ToShortDateString() : Rock.Constants.None.TextHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}