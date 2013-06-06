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

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    [LinkedPage( "Residency Period Page" )]
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
                string itemId = PageParameter( "residencyTrackId" );
                string residencyPeriodId = PageParameter( "residencyPeriodId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( residencyPeriodId ) )
                    {
                        ShowDetail( "residencyTrackId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyTrackId", int.Parse( itemId ), int.Parse( residencyPeriodId ) );
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

            if ( hfResidencyTrackId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the ResidencyPeriod Detail page, return to that
                string residencyPeriodId = PageParameter( "residencyPeriodId" );
                if ( !string.IsNullOrWhiteSpace( residencyPeriodId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["residencyPeriodId"] = residencyPeriodId;
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
                ResidencyService<ResidencyTrack> service = new ResidencyService<ResidencyTrack>();
                ResidencyTrack item = service.Get( hfResidencyTrackId.ValueAsInt() );
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
            ResidencyService<ResidencyTrack> service = new ResidencyService<ResidencyTrack>();
            ResidencyTrack item = service.Get( hfResidencyTrackId.ValueAsInt() );
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
            residencyTrack.ResidencyPeriodId = hfResidencyPeriodId.ValueAsInt();

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

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyTrackId"] = residencyTrack.Id.ToString();
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
        /// <param name="residencyPeriodId">The residency competency id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyPeriodId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyTrackId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            ResidencyTrack residencyTrack = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyTrack = new ResidencyService<ResidencyTrack>().Get( itemKeyValue );
            }
            else
            {
                residencyTrack = new ResidencyTrack { Id = 0 };
                residencyTrack.ResidencyPeriodId = residencyPeriodId ?? 0;
                residencyTrack.ResidencyPeriod = new ResidencyService<ResidencyPeriod>().Get( residencyTrack.ResidencyPeriodId );
            }

            hfResidencyTrackId.Value = residencyTrack.Id.ToString();
            hfResidencyPeriodId.Value = residencyTrack.ResidencyPeriodId.ToString();

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
                btnEdit.Visible = false;
                ShowReadonlyDetails( residencyTrack );
            }
            else
            {
                btnEdit.Visible = true;
                if ( residencyTrack.Id > 0 )
                {
                    ShowReadonlyDetails( residencyTrack );
                }
                else
                {
                    ShowEditDetails( residencyTrack );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyTrack">The residency project.</param>
        private void ShowEditDetails( ResidencyTrack residencyTrack )
        {
            if ( residencyTrack.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( ResidencyTrack.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( ResidencyTrack.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = residencyTrack.Name;
            tbDescription.Text = residencyTrack.Description;
            lblPeriod.Text = residencyTrack.ResidencyPeriod.Name;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyTrack">The residency project.</param>
        private void ShowReadonlyDetails( ResidencyTrack residencyTrack )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Name", residencyTrack.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Description", residencyTrack.Description );

            string residencyPeriodPageGuid = this.GetAttributeValue( "ResidencyPeriodPage" );
            string periodHtml = residencyTrack.ResidencyPeriod.Name;
            if ( !string.IsNullOrWhiteSpace( residencyPeriodPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residencyPeriodPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "residencyPeriodId", residencyTrack.ResidencyPeriodId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                periodHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyTrack.ResidencyPeriod.Name );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Period", periodHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}