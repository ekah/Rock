//
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
using Rock.Data;
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
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                BindGrid();
            }

            base.OnLoad( e );
        }

        #endregion

        #region Grid Events

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
                    lbStart.Visible = !recording.StartTime.HasValue && !recording.StopTime.HasValue;
                    lbStop.Visible = recording.StartTime.HasValue && !recording.StopTime.HasValue;
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

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var service = new RecordingService();
            var sortProperty = gRecordings.SortProperty;

            if ( sortProperty != null )
            {
                gRecordings.DataSource = service.Queryable().Sort( sortProperty ).ToList();
            }
            else
            {
                gRecordings.DataSource = service.Queryable().OrderByDescending( s => s.Date ).ToList();
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