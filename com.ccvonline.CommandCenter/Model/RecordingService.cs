﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using com.ccvonline.CommandCenter.Data;

namespace com.ccvonline.CommandCenter.Model
{
    /// <summary>
    /// 
    /// </summary>
    class RecordingService : CommandCenterService<Recording>
    {
        /// <summary>
        /// Sends the recording request.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="streamName">Name of the stream.</param>
        /// <param name="recordingName">Name of the recording.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">missing 'ccvonlineWowzaServer' Global Attribute value</exception>
        static public Rock.Net.RockWebResponse SendRecordingRequest( string app, string streamName, string recordingName, string action )
        {
            var globalAttributes = Rock.Web.Cache.GlobalAttributesCache.Read();

            if ( globalAttributes.AttributeValues.ContainsKey( "ccvonlineWowzaServer" ) )
            {
                string wowzaServerUrl = globalAttributes.AttributeValues["ccvonlineWowzaServer"].Value;
                if ( !string.IsNullOrWhiteSpace( wowzaServerUrl ) )
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add( "app", HttpUtility.UrlEncode( app ) );
                    parms.Add( "streamname", HttpUtility.UrlEncode( streamName ) );
                    parms.Add( "recordingname", HttpUtility.UrlEncode( recordingName ) );
                    parms.Add( "action", HttpUtility.UrlEncode( action ) );

                    return Rock.Net.RockWebRequest.Send( wowzaServerUrl, "GET", parms, null );
                }
            }

            throw new ApplicationException( "missing 'ccvonlineWowzaServer' Global Attribute value" );
        }

        /// <summary>
        /// Parses the response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        static public string ParseResponse( string message )
        {
            Match match = Regex.Match( message, @"((?<=(\<h1\>)).*(?=(\<\/h1\>)))", RegexOptions.IgnoreCase );
            if ( match.Success )
                return match.Value;
            return string.Empty;
        }

        /// <summary>
        /// Starts the recording.
        /// </summary>
        /// <param name="campusId">The campus id.</param>
        /// <param name="label">The label.</param>
        /// <param name="app">The app.</param>
        /// <param name="streamName">Name of the stream.</param>
        /// <param name="recordingName">Name of the recording.</param>
        /// <param name="personId">The person id.</param>
        /// <returns></returns>
        public Recording StartRecording( int? campusId, string label, string app, string streamName, string recordingName, int? personId )
        {
            Rock.Net.RockWebResponse response = SendRecordingRequest( app, streamName, recordingName, "start" );

            if ( response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK )
            {
                Recording recording = new Recording();
                this.Add( recording, personId );

                recording.CampusId = campusId;
                recording.Date = DateTime.Today;
                recording.Label = label;
                recording.App = app;
                recording.StreamName = streamName;
                recording.RecordingName = recordingName;
                recording.StartTime = DateTime.Now;
                recording.StartResponse = ParseResponse( response.Message );
                this.Save( recording, personId );

                return recording;
            }

            return null;
        }

        /// <summary>
        /// Stops the recording.
        /// </summary>
        /// <param name="campusId">The campus id.</param>
        /// <param name="label">The label.</param>
        /// <param name="app">The app.</param>
        /// <param name="streamName">Name of the stream.</param>
        /// <param name="recordingName">Name of the recording.</param>
        /// <param name="personId">The person id.</param>
        /// <returns></returns>
        public Recording StopRecording( int? campusId, string label, string app, string streamName, string recordingName, int? personId )
        {
            Rock.Net.RockWebResponse response = SendRecordingRequest( app, streamName, recordingName, "stop" );

            if ( response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK )
            {
                IQueryable<Recording> recordings = Queryable().
                    Where( r =>
                        r.CampusId == campusId &&
                        r.Label == label &&
                        r.App == app &&
                        r.StreamName == streamName &&
                        r.RecordingName == recordingName &&
                        r.StartTime != null &&
                        r.StopTime == null );

                Recording stoppedRecording = new Recording();
                DateTime stopTime = DateTime.Now;
                string responseMessage = ParseResponse( response.Message );

                foreach ( var recording in recordings.OrderBy( r => r.StartTime ).ToList() )
                {
                    recording.StopTime = stopTime;
                    recording.StopResponse = responseMessage;
                    this.Save( recording, personId );

                    stoppedRecording = recording;
                }

                return stoppedRecording;
            }

            return null;
        }

    }
}
