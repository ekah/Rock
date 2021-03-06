//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
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
                string itemId = PageParameter( "trackId" );
                string periodId = PageParameter( "periodId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( periodId ) )
                    {
                        ShowDetail( "trackId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "trackId", int.Parse( itemId ), int.Parse( periodId ) );
                    }
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns></returns>
        public override List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();

            int? trackId = this.PageParameter( pageReference, "trackId" ).AsInteger();
            if ( trackId != null )
            {
                Track track = new ResidencyService<Track>().Get( trackId.Value );
                if ( track != null )
                {
                    breadCrumbs.Add( new BreadCrumb( track.Name, pageReference ) );
                }
                else
                {
                    breadCrumbs.Add( new BreadCrumb( "Track", pageReference ) );
                }
            }
            else
            {
                // don't show a breadcrumb if we don't have a pageparam to work with
            }

            return breadCrumbs;
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

            if ( hfTrackId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the Period Detail page, return to that
                string periodId = PageParameter( "periodId" );
                if ( !string.IsNullOrWhiteSpace( periodId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["periodId"] = periodId;
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
                Track item = service.Get( hfTrackId.ValueAsInt() );
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
            Track item = service.Get( hfTrackId.ValueAsInt() );
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
            Track track;
            ResidencyService<Track> trackService = new ResidencyService<Track>();

            int trackId = hfTrackId.ValueAsInt();
            int periodId = hfPeriodId.ValueAsInt();

            if ( trackId == 0 )
            {
                track = new Track();
                trackService.Add( track, CurrentPersonId );

                int maxDisplayOrder = trackService.Queryable()
                        .Where( a => a.PeriodId.Equals( periodId ) )
                        .Select( a => a.DisplayOrder ).DefaultIfEmpty( 0 ).Max();
                track.DisplayOrder = maxDisplayOrder + 1;
            }
            else
            {
                track = trackService.Get( trackId );
            }

            track.Name = tbName.Text;
            track.Description = tbDescription.Text;
            track.PeriodId = periodId;

            if ( !track.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                trackService.Save( track, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["trackId"] = track.Id.ToString();
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
        /// <param name="periodId">The period id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? periodId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "trackId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            Track track = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                track = new ResidencyService<Track>().Get( itemKeyValue );
            }
            else
            {
                track = new Track { Id = 0 };
                track.PeriodId = periodId ?? 0;
                track.Period = new ResidencyService<Period>().Get( track.PeriodId );
            }

            hfTrackId.Value = track.Id.ToString();
            hfPeriodId.Value = track.PeriodId.ToString();

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
                ShowReadonlyDetails( track );
            }
            else
            {
                btnEdit.Visible = true;
                if ( track.Id > 0 )
                {
                    ShowReadonlyDetails( track );
                }
                else
                {
                    ShowEditDetails( track );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="track">The track.</param>
        private void ShowEditDetails( Track track )
        {
            if ( track.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Track.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Track.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = track.Name;
            tbDescription.Text = track.Description;
            lblPeriod.Text = track.Period.Name;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="track">The track.</param>
        private void ShowReadonlyDetails( Track track )
        {
            SetEditMode( false );

            lblMainDetails.Text = new DescriptionList()
                .Add( "Name", track.Name )
                .Add( "Description", track.Description )
                .StartSecondColumn()
                .Add( "Period", track.Period.Name )
                .Html;
        }

        #endregion
    }
}