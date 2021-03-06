﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
// Uses the ImageResizer image resizing library found here:
// http://imageresizing.net/docs/reference

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Web;

using ImageResizer;

using Rock.Model;

namespace RockWeb
{
    /// <summary>
    /// Handles retrieving file (image) data from storage, with all the bells and whistles.
    /// </summary>
    public class GetImage : IHttpHandler
    {
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest( HttpContext context )
        {
            context.Response.Clear();

            if ( context.Request.QueryString == null || context.Request.QueryString.Count == 0)
            {
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }

            string anID = context.Request.QueryString[0];
            int id;

            if (!int.TryParse( anID, out id))
            {
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }

            try
            {
                BinaryFileService fileService = new BinaryFileService();
                Rock.Model.BinaryFile file = null;

                string cacheName = Uri.EscapeDataString( context.Request.Url.Query );
                string physFilePath = context.Request.MapPath( string.Format( "~/Cache/{0}", cacheName ) );

                // Is it cached
                if ( File.Exists( physFilePath ) )
                {
                    // When was file last modified
                    dynamic fileInfo = fileService
                        .Queryable()
                        .Where( f => f.Id == id )
                        .Select( f => new
                        {
                            MimeType = f.MimeType,
                            LastModifiedDateTime = f.LastModifiedDateTime
                        } )
                        .FirstOrDefault();

                    file = new Rock.Model.BinaryFile();
                    file.MimeType = fileInfo.MimeType;
                    file.LastModifiedDateTime = fileInfo.LastModifiedDateTime;

                    // Is cached version newer?
                    if ( !file.LastModifiedDateTime.HasValue || file.LastModifiedDateTime.Value.CompareTo( File.GetCreationTime( physFilePath ) ) <= 0 )
                    {
                        if ( file.Data == null )
                        {
                            file.Data = new BinaryFileData();
                        }
                        file.Data.Content = FetchFromCache( physFilePath );
                    }
                }

                if ( file == null || file.Data == null )
                {
                    file = fileService.Get( id );

                    if ( file != null )
                    {
                        if ( WantsImageResizing( context ) )
                            Resize( context, file );

                        Cache( file, physFilePath );
                    }
                }

                if ( file == null || file.Data == null )
                {
                    context.Response.StatusCode = 404;
                    context.Response.End();
                    return;
                }

                // Post process
                SendFile( context, file );
            }
            catch
            {
            }
        }

        /// <summary>
        /// Resizes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        private static void Resize( HttpContext context, Rock.Model.BinaryFile file )
        {
            ResizeSettings settings = new ResizeSettings( context.Request.QueryString );
            MemoryStream resizedStream = new MemoryStream();
            ImageBuilder.Current.Build( new MemoryStream( file.Data.Content ), resizedStream, settings );
            file.Data.Content = resizedStream.GetBuffer();
        }

        /// <summary>
        /// Caches the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="physFilePath">The phys file path.</param>
        private static void Cache( Rock.Model.BinaryFile file, string physFilePath )
        {
            try
            {
                using ( BinaryWriter binWriter = new BinaryWriter( File.Open( physFilePath, FileMode.Create ) ) )
                {
                    binWriter.Write( file.Data.Content );
                }
            }
            catch { /* do nothing, not critical if this fails, although TODO: log */ }
        }

        /// <summary>
        /// Fetches from cache.
        /// </summary>
        /// <param name="physFilePath">The phys file path.</param>
        /// <returns></returns>
        private static byte[] FetchFromCache( string physFilePath )
        {
            try
            {
                byte[] data;
                using ( BinaryReader binReader = new BinaryReader( File.Open( physFilePath, FileMode.Open, FileAccess.Read, FileShare.Read ) ) )
                {
                    data = File.ReadAllBytes( physFilePath );
                }
                return data;
            }
            catch { /* ok, so we'll just skip using the cache, but TODO: log this */}

            return null;
        }

        /// <summary>
        /// A small utility method to determine if we need to use the ImageResizer.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>True if the request desires image resizing/manipulation; false otherwise.</returns>
        private static bool WantsImageResizing( HttpContext context )
        {
            return context.Request.QueryString.Count > 1;
        }

        /// <summary>
        /// Sends the file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        private static void SendFile( HttpContext context, Rock.Model.BinaryFile file )
        {
            context.Response.ContentType = file.MimeType;
            context.Response.AddHeader( "content-disposition", "inline;filename=" + file.FileName );
            context.Response.BinaryWrite( file.Data.Content );
            context.Response.Flush();
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Not Currently Used.
        /// 
        /// Utility method to renders an error image to the output stream.
        /// Not sure if I like this idea, but it would generate the errorMessages
        /// as an image in red text.
        /// </summary>
        /// <param name="context">HttpContext of current request</param>
        /// <param name="errorMessage">error message text to render</param>
        private void renderErrorImage( HttpContext context, string errorMessage )
        {
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            Bitmap bitmap = new Bitmap( 7 * errorMessage.Length, 30 );    // width based on error message
            Graphics g = Graphics.FromImage( bitmap );
            g.FillRectangle( new SolidBrush( Color.LightSalmon ), 0, 0, bitmap.Width, bitmap.Height ); // background
            g.DrawString( errorMessage, new Font( "Tahoma", 10, FontStyle.Bold ), new SolidBrush( Color.DarkRed ), new PointF( 5, 5 ) );
            bitmap.Save( context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg );
        }
    }
}