//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using com.ccvonline.CommandCenter.Model;
using Rock.Rest;
using Rock.Rest.Filters;

namespace Rock.Com.CCVOnline.Rest.Service
{
    /// <summary>
    /// Recordings REST API
    /// </summary>
    /// 
    public partial class RecordingsController : Rock.Rest.ApiController<Recording>, IHasCustomRoutes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingsController" /> class.
        /// </summary>
        public RecordingsController() : base( new RecordingService() ) { }

        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "com.ccvonline.CommandCenter.Recording",
                routeTemplate: "api/Recordings/{action}/{campusId}/{label}/{app}/{stream}/{recording}",
                defaults: new
                {
                    controller = "recordings"
                } );

            routes.MapHttpRoute(
                name: "com.ccvonline.CommandCenter.RecordingDate",
                routeTemplate: "api/Recordings/dates/{qualifier}",
                defaults: new
                {
                    controller = "recordings",
                    action = "dates"
                } );
        }

        /// <summary>
        /// Starts the specified campus id.
        /// </summary>
        /// <param name="campusId">The campus id.</param>
        /// <param name="label">The label.</param>
        /// <param name="app">The app.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="recording">The recording.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [HttpGet]
        [Authenticate]
        public Recording Start( int campusId, string label, string app, string stream, string recording )
        {
            var user = this.CurrentUser();
            if ( user != null )
            {
                var RecordingService = new RecordingService();
                var Recording = RecordingService.StartRecording( campusId, label, app, stream, recording, user.PersonId );

                if ( Recording != null )
                    return Recording;
                else
                    throw new HttpResponseException( HttpStatusCode.BadRequest );
            }
            throw new HttpResponseException( HttpStatusCode.Unauthorized );
        }

        /// <summary>
        /// Stops the specified campus id.
        /// </summary>
        /// <param name="campusId">The campus id.</param>
        /// <param name="label">The label.</param>
        /// <param name="app">The app.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="recording">The recording.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [HttpGet]
        [Authenticate]
        public Recording Stop( int campusId, string label, string app, string stream, string recording )
        {
            var user = this.CurrentUser();
            if ( user != null )
            {
                var RecordingService = new RecordingService();
                var Recording = RecordingService.StopRecording( campusId, label, app, stream, recording, user.PersonId );

                if ( Recording != null )
                    return Recording;
                else
                    throw new HttpResponseException( HttpStatusCode.BadRequest );
            }
            throw new HttpResponseException( HttpStatusCode.Unauthorized );
        }

        /// <summary>
        /// Dateses the specified qualifier.
        /// </summary>
        /// <param name="qualifier">The qualifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [HttpGet]
        [Authenticate]
        public IEnumerable<DateTime> Dates( string qualifier )
        {
            var user = this.CurrentUser();
            if ( user != null )
            {
                var RecordingService = new RecordingService();
                var dates = RecordingService.Queryable()
                    .Where( r => r.StartTime.HasValue )
                    .OrderByDescending( r => r.StartTime.Value )
                    .Select( r => r.StartTime.Value )
                    .ToList();

                if ( string.Equals( qualifier, "distinct", StringComparison.CurrentCultureIgnoreCase ) )
                    return dates.Select( d => d.Date ).Distinct();
                else
                    return dates;
            }

            throw new HttpResponseException( HttpStatusCode.Unauthorized );
        }

    }
}
