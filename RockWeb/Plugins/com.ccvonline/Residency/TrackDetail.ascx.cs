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
    [LinkedPage( "Residency Period Page" )]
    public partial class TrackDetail : RockBlock, IDetailBlock
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
                ResidencyService<Track> service = new ResidencyService<Track>();
                Track item = service.Get( hfResidencyTrackId.ValueAsInt() );
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
            ResidencyService<Track> service = new ResidencyService<Track>();
            Track item = service.Get( hfResidencyTrackId.ValueAsInt() );
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
            Track residencyTrack;
            ResidencyService<Track> residencyTrackService = new ResidencyService<Track>();

            int residencyTrackId = int.Parse( hfResidencyTrackId.Value );

            if ( residencyTrackId == 0 )
            {
                residencyTrack = new Track();
                residencyTrackService.Add( residencyTrack, CurrentPersonId );
            }
            else
            {
                residencyTrack = residencyTrackService.Get( residencyTrackId );
            }

            residencyTrack.Name = tbName.Text;
            residencyTrack.Description = tbDescription.Text;
            residencyTrack.PeriodId = hfResidencyPeriodId.ValueAsInt();
             
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
            Track residencyTrack = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyTrack = new ResidencyService<Track>().Get( itemKeyValue );
            }
            else
            {
                residencyTrack = new Track { Id = 0 };
                residencyTrack.PeriodId = residencyPeriodId ?? 0;
                residencyTrack.Period = new ResidencyService<Period>().Get( residencyTrack.PeriodId );
            }

            hfResidencyTrackId.Value = residencyTrack.Id.ToString();
            hfResidencyPeriodId.Value = residencyTrack.PeriodId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Track.FriendlyTypeName );
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
        private void ShowEditDetails( Track residencyTrack )
        {
            if ( residencyTrack.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Track.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Track.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = residencyTrack.Name;
            tbDescription.Text = residencyTrack.Description;
            lblPeriod.Text = residencyTrack.Period.Name;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyTrack">The residency project.</param>
        private void ShowReadonlyDetails( Track residencyTrack )
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
            string periodHtml = residencyTrack.Period.Name;
            if ( !string.IsNullOrWhiteSpace( residencyPeriodPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residencyPeriodPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "residencyPeriodId", residencyTrack.PeriodId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                periodHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyTrack.Period.Name );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Period", periodHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}