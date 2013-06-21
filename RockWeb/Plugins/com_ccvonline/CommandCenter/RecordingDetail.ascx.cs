﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using com.ccvonline.CommandCenter.Model;

namespace RockWeb.Plugins.com_ccvonline.CommandCenter
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RecordingDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            cpCampus.Campuses = new CampusService().Queryable().OrderBy( c => c.Name ).ToList();
            cpCampus.Items.Insert( 0, new ListItem( None.Text, None.IdValue ) );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string itemId = PageParameter( "recordingId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "recordingId", int.Parse( itemId ) );
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
            Recording recording;
            var service = new RecordingService();

            int recordingId = 0;
            if ( !Int32.TryParse( hfRecordingId.Value, out recordingId ) )
                recordingId = 0;

            if ( recordingId == 0 )
            {
                recording = new Recording();
                service.Add( recording, CurrentPersonId );
            }
            else
            {
                recording = service.Get( recordingId );
            }

            recording.CampusId = cpCampus.SelectedCampusId;
            recording.App = tbApp.Text;
            recording.Date = dpDate.SelectedDate;
            recording.StreamName = tbStream.Text;
            recording.Label = tbLabel.Text;
            recording.RecordingName = tbRecording.Text;

            if ( recordingId == 0 && cbStartRecording.Visible && cbStartRecording.Checked )
            {
                SendRequest( "start", recording );
            }

            RockTransactionScope.WrapTransaction( () =>
                {
                    service.Save( recording, CurrentPersonId );
                } );

            NavigateToParentPage();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "recordingId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            Recording recording = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                recording = new RecordingService().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( Recording.FriendlyTypeName );
            }
            else
            {
                recording = new Recording { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( Recording.FriendlyTypeName );
            }

            hfRecordingId.Value = recording.Id.ToString();

            cpCampus.SelectedCampusId = recording.CampusId;
            tbApp.Text = recording.App ?? string.Empty;
            dpDate.SelectedDate = recording.Date;
            tbStream.Text = recording.StreamName ?? string.Empty;
            tbLabel.Text = recording.Label ?? string.Empty;
            tbRecording.Text = recording.RecordingName ?? string.Empty;
            lStarted.Text = recording.StartTime.HasValue ? recording.StartTime.Value.ToString() : string.Empty;
            lStartResponse.Text = recording.StartResponse ?? string.Empty;
            lStopped.Text = recording.StopTime.HasValue ? recording.StopTime.Value.ToString() : string.Empty;
            lStopResponse.Text = recording.StopResponse ?? string.Empty;

            lStarted.Visible = recording.StartTime.HasValue;
            lStartResponse.Visible = !string.IsNullOrEmpty( recording.StartResponse );
            lStopped.Visible = recording.StopTime.HasValue;
            lStopResponse.Visible = !string.IsNullOrEmpty( recording.StopResponse );

            cbStartRecording.Visible = false;

            bool readOnly = !IsUserAuthorized( "Edit" );
            nbEditModeMessage.Text = string.Empty;

            if ( readOnly )
            {
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Recording.FriendlyTypeName );
                lActionTitle.Text = ActionTitle.View( Recording.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            cpCampus.Enabled = !readOnly;
            tbApp.ReadOnly = readOnly;
            dpDate.ReadOnly = readOnly;
            tbStream.ReadOnly = readOnly;
            tbLabel.ReadOnly = readOnly;
            tbRecording.ReadOnly = readOnly;

            btnSave.Visible = !readOnly;
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="recording">The recording.</param>
        /// <returns></returns>
        public bool SendRequest( string action, Recording recording )
        {
            Rock.Net.RockWebResponse response = RecordingService.SendRecordingRequest( recording.App, recording.StreamName, recording.RecordingName, action.ToLower() );

            if ( response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK )
            {

                if ( action.ToLower() == "start" )
                {
                    recording.StartTime = DateTime.Now;
                    recording.StartResponse = RecordingService.ParseResponse( response.Message );
                }
                else
                {
                    recording.StopTime = DateTime.Now;
                    recording.StopResponse = RecordingService.ParseResponse( response.Message );
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}