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
using Rock.Attribute;
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
    [DetailPage]
    public partial class RecordingList : RockBlock
    {
        #region Private Fields

        /// <summary>
        /// Can user Start/Stop or edit a recording
        /// </summary>
        private bool _canEdit = false;

        #endregion

        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            _canEdit = IsUserAuthorized( "Edit" );
            gRecordings.Actions.ShowAdd = _canEdit;
            gRecordings.IsDeleteEnabled = _canEdit;
            gRecordings.Columns[8].Visible = _canEdit;   // Start
            gRecordings.Columns[9].Visible = _canEdit;   // Stop

            gRecordings.DataKeyNames = new string[] { "id" };
            gRecordings.Actions.AddClick += gRecordings_Add;
            gRecordings.RowDataBound += gRecordings_RowDataBound;
            gRecordings.RowCommand += gRecordings_RowCommand;
            gRecordings.GridRebind += gRecordings_GridRebind;

            rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
            rFilter.DisplayFilterValue += rFilter_DisplayFilterValue;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                BindFilter();
                BindGrid();
            }

            base.OnLoad( e );
        }

        #endregion

        #region Grid Events

        void rFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Campus":

                    int campusId = 0;
                    if ( int.TryParse( e.Value, out campusId ) )
                    {
                        var service = new CampusService();
                        var campus = service.Get( campusId );
                        if ( campus != null )
                        {
                            e.Value = campus.Name;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the ApplyFilterClick event of the rFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rFilter.SaveUserPreference( "Campus", cpCampus.SelectedValue != All.Id.ToString() ? cpCampus.SelectedValue : string.Empty );
            rFilter.SaveUserPreference( "From Date", dtStartDate.Text );
            rFilter.SaveUserPreference( "To Date", dtEndDate.Text );
            rFilter.SaveUserPreference( "Stream", tbStream.Text );
            rFilter.SaveUserPreference( "Label", tbLabel.Text );
            rFilter.SaveUserPreference( "Recording", tbRecording.Text );

            BindGrid();
        }

        /// <summary>
        /// Handles the RowDataBound event of the gRecordings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gRecordings_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            if ( _canEdit )
            {
                Recording recording = e.Row.DataItem as Recording;
                LinkButton lbStart = (LinkButton)e.Row.FindControl( "lbStart" );
                LinkButton lbStop = (LinkButton)e.Row.FindControl( "lbStop" );

                if ( recording != null && lbStart != null && lbStop != null )
                {
                    lbStart.Enabled = !recording.StartTime.HasValue && !recording.StopTime.HasValue;
                    lbStop.Enabled = recording.StartTime.HasValue && !recording.StopTime.HasValue;

                    if ( lbStart.Enabled )
                    {
                        lbStart.RemoveCssClass( "disabled" );
                    }
                    else
                    {
                        lbStart.AddCssClass( "disabled" );
                    }

                    if ( lbStop.Enabled )
                    {
                        lbStop.RemoveCssClass( "disabled" );
                    }
                    else
                    {
                        lbStop.AddCssClass( "disabled" );
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Add event of the gRecordings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gRecordings_Add( object sender, EventArgs e )
        {
            NavigateToDetailPage( "recordingId", 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gRecordings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gRecordings_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( "recordingId", (int)e.RowKeyValue );
        }

        /// <summary>
        /// Handles the Delete event of the gRecordings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gRecordings_Delete( object sender, RowEventArgs e )
        {
            RockTransactionScope.WrapTransaction( () =>
            {
                var service = new RecordingService();
                var recording = service.Get( (int)gRecordings.DataKeys[e.RowIndex]["id"] );
                if ( recording != null )
                {
                    string errorMessage;
                    if ( !service.CanDelete( recording, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    service.Delete( recording, CurrentPersonId );
                    service.Save( recording, CurrentPersonId );
                }
            } );

            BindGrid();
        }

        /// <summary>
        /// Handles the RowCommand event of the gRecordings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs" /> instance containing the event data.</param>
        protected void gRecordings_RowCommand( object sender, GridViewCommandEventArgs e )
        {
            if ( e.CommandName == "START" || e.CommandName == "STOP" )
            {
                var service = new RecordingService();
                var recording = service.Get( Int32.Parse( e.CommandArgument.ToString() ) );
                if ( recording != null && SendRequest( e.CommandName.ToString().ToLower(), recording ) )
                {
                    service.Save( recording, CurrentPersonId );
                }
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gRecordings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gRecordings_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        #endregion

        #region Internal Methods

        private void BindFilter()
        {
            cpCampus.Campuses = new CampusService().Queryable().OrderBy( c => c.Name ).ToList();
            cpCampus.Items.Insert( 0, new ListItem( All.Text, All.IdValue ) );
            cpCampus.SelectedValue = rFilter.GetUserPreference( "Campus" );
            dtStartDate.Text = rFilter.GetUserPreference( "From Date" );
            dtEndDate.Text = rFilter.GetUserPreference( "To Date" );
            tbStream.Text = rFilter.GetUserPreference( "Stream" );
            tbLabel.Text = rFilter.GetUserPreference( "Label" );
            tbRecording.Text = rFilter.GetUserPreference( "Recording" );
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var service = new RecordingService();
            var sortProperty = gRecordings.SortProperty;

            var queryable = new RecordingService().Queryable();

            int campusId = int.MinValue;
            if ( int.TryParse( rFilter.GetUserPreference( "Campus" ), out campusId ) && campusId > 0 )
            {
                queryable = queryable.Where( r => r.CampusId == campusId );
            }

            DateTime fromDate = DateTime.MinValue;
            if ( DateTime.TryParse( rFilter.GetUserPreference( "From Date" ), out fromDate ) )
            {
                queryable = queryable.Where( r => r.Date >= fromDate );
            }

            DateTime toDate = DateTime.MinValue;
            if ( DateTime.TryParse( rFilter.GetUserPreference( "To Date" ), out toDate ) )
            {
                queryable = queryable.Where( r => r.Date <= toDate );
            }

            string stream = rFilter.GetUserPreference("Stream");
            if ( !string.IsNullOrWhiteSpace( stream ) )
            {
                queryable = queryable.Where( r => r.StreamName.StartsWith( stream ) );
            }

            string label = rFilter.GetUserPreference( "Label" );
            if ( !string.IsNullOrWhiteSpace( label ) )
            {
                queryable = queryable.Where( r => r.Label.StartsWith( label ) );
            }

            string recording = rFilter.GetUserPreference( "Recording" );
            if ( !string.IsNullOrWhiteSpace( recording ) )
            {
                queryable = queryable.Where( r => r.RecordingName.StartsWith( recording ) );
            }

            if ( sortProperty != null )
            {
                gRecordings.DataSource = queryable.Sort( sortProperty ).ToList();
            }
            else
            {
                gRecordings.DataSource = queryable.OrderByDescending( s => s.Date ).ToList();
            }

            gRecordings.DataBind();
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="recording">The recording.</param>
        /// <returns></returns>
        public bool SendRequest( string action, Recording recording )
        {
            try
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
            }
            catch( System.Exception ex )
            {
                mdGridWarning.Show( ex.Message, ModalAlertType.Alert );
            }

            return false;
        }

        #endregion
    }
}