﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Xml.Linq;
using System.Xml.Xsl;
using Rock.Attribute;

namespace RockWeb.Blocks.Cms
{
    [TextField( "XSLT File", "The path to the XSLT File ", true, "~/Assets/XSLT/PageList.xslt" )]
    [TextField( "Root Page", "The root page to use for the page collection. Defaults to the current page instance if not set.", false, "" )]
    [TextField( "Number of Levels", "Number of parent-child page levels to display. Default 3.", false, "3" )]
    [BooleanField( "Include Current Parameters", "Flag indicating if current page's parameters should be used when building url for child pages", false)]
    public partial class PageXslt : Rock.Web.UI.RockBlock
    {
        private static readonly string ROOT_PAGE = "RootPage";
        private static readonly string NUM_LEVELS = "NumberofLevels";

        protected override void OnInit( EventArgs e )
        {
            this.EnableViewState = false;

            base.OnInit( e );

            this.AttributesUpdated += PageXslt_AttributesUpdated;
            //this.AddAttributeUpdateTrigger( upContent );
            //upContent.ContentTemplateContainer.Controls.Add( )

            TransformXml();
        }

        void PageXslt_AttributesUpdated( object sender, EventArgs e )
        {
            TransformXml();
        }

        private void TransformXml()
        {
            XslCompiledTransform xslTransformer = new XslCompiledTransform();
            xslTransformer.Load( Server.MapPath( GetAttributeValue("XSLTFile") ) );

            Rock.Web.Cache.PageCache rootPage;
            if ( GetAttributeValue( ROOT_PAGE ) != string.Empty )
            {
                int pageId = Convert.ToInt32( GetAttributeValue( ROOT_PAGE ) );
                if ( pageId == -1 )
                    rootPage = CurrentPage;
                else
                    rootPage = Rock.Web.Cache.PageCache.Read( pageId );
            }
            else
                rootPage = CurrentPage;

            int levelsDeep = Convert.ToInt32( GetAttributeValue( NUM_LEVELS ) );

            Dictionary<string, string> pageParameters = null;
            bool passParams = false;
            if ( bool.TryParse( GetAttributeValue( "IncludeCurrentParameters" ), out passParams ) && passParams )
            {
                pageParameters = CurrentPageReference.Parameters;
            }
            XDocument pageXml = rootPage.MenuXml( levelsDeep, CurrentPerson, CurrentPage, pageParameters );

            StringBuilder sb = new StringBuilder();
            TextWriter tw = new StringWriter( sb );
            xslTransformer.Transform( pageXml.CreateReader(), null, tw );

            phContent.Controls.Clear();
            phContent.Controls.Add( new LiteralControl( sb.ToString() ) );
        }
    }
}