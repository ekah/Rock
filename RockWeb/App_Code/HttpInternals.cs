﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

/// <summary>
/// Summary description for HttpInternals
/// derived from http://stackoverflow.com/questions/613824/how-to-prevent-an-asp-net-application-restarting-when-the-web-config-is-modified/629876#629876
/// </summary>
internal static class HttpInternals
{
    private static readonly FieldInfo s_TheRuntime = typeof( HttpRuntime ).GetField( "_theRuntime", BindingFlags.NonPublic | BindingFlags.Static );
    private static readonly FieldInfo s_FileChangesMonitor = typeof( HttpRuntime ).GetField( "_fcm", BindingFlags.NonPublic | BindingFlags.Instance );
    private static readonly MethodInfo s_FileChangesMonitorStop = s_FileChangesMonitor.FieldType.GetMethod( "Stop", BindingFlags.NonPublic | BindingFlags.Instance );

    /// <summary>
    /// Gets the HTTP runtime.
    /// </summary>
    /// <value>
    /// The HTTP runtime.
    /// </value>
    private static object TheHttpRuntime
    {
        get
        {
            return s_TheRuntime.GetValue( null );
        }
    }

    /// <summary>
    /// Gets the file changes monitor.
    /// </summary>
    /// <value>
    /// The file changes monitor.
    /// </value>
    private static object FileChangesMonitor
    {
        get
        {
            return s_FileChangesMonitor.GetValue( TheHttpRuntime );
        }
    }

    /// <summary>
    /// Stops the file monitoring.
    /// </summary>
    public static void StopFileMonitoring()
    {
        s_FileChangesMonitorStop.Invoke( FileChangesMonitor, null );
    }

    /// <summary>
    /// Replaces internal FCN with a custom file change monitor for RockWeb
    /// </summary>
    public static void RockWebFileChangeMonitor()
    {
        StopFileMonitoring();
        HttpInternals.StopFileMonitoring();
        System.IO.FileSystemWatcher fsw = new FileSystemWatcher( HttpRuntime.AppDomainAppPath );
        fsw.NotifyFilter = NotifyFilters.LastWrite;
        fsw.IncludeSubdirectories = true;
        fsw.Changed += fsw_Changed;
        fsw.EnableRaisingEvents = true;
    }

    /// <summary>
    /// Handles the Changed event of the fsw control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
    public static void fsw_Changed( object sender, FileSystemEventArgs e )
    {
        FileInfo fileInfo = new FileInfo( e.FullPath );

        string[] extensionIgnoreFilter = new string[] { ".csv", ".ignore2" };
        string[] dirIgnoreFilter = new string[] { "Cache", "Logs" };

        if ( fileInfo.Attributes.HasFlag( FileAttributes.Directory ) )
        {
            if ( dirIgnoreFilter.Contains( fileInfo.Name ) )
            {
                // directory content change and this is a directory to ignore
                return;
            }
        }

        if ( dirIgnoreFilter.Contains( fileInfo.Directory.Name ) )
        {
            // a file within an ignored folder changed
            return;
        }

        if ( !extensionIgnoreFilter.Contains( fileInfo.Extension ) )
        {
            if ( !dirIgnoreFilter.Contains( fileInfo.Name ) )
            {
                HostingEnvironment.InitiateShutdown();
            }
        }
    }
}