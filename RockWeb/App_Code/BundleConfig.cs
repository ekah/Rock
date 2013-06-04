﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Optimization;

/// <summary>
/// Loads and concats JS bundles, fired during App_Start
/// </summary>
public class BundleConfig
{
    /// <summary>
    /// Registers the bundles.
    /// </summary>
    /// <param name="bundles">The bundles.</param>
	public static void RegisterBundles( BundleCollection bundles )
    {
        // TODO: Add bundles for CSS files

        bundles.Add( new ScriptBundle( "~/bundles/WebFormsJs" ).Include(
            "~/Scripts/WebForms/WebForms.js",
            "~/Scripts/WebForms/WebUIValidation.js",
            "~/Scripts/WebForms/MenuStandards.js",
            "~/Scripts/WebForms/Focus.js",
            "~/Scripts/WebForms/GridView.js",
            "~/Scripts/WebForms/DetailsView.js",
            "~/Scripts/WebForms/TreeView.js",
            "~/Scripts/WebForms/WebParts.js" ) );

        // Omitting AjaxToolkit bundle for now. The toolkit and scriptmanager facade seem to
        // conflict with eachother, despite one being a "fix" for the other.
        //bundles.Add( new ScriptBundle( "~/bundles/MsAjaxJs" ).Include(
        //    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
        //    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
        //    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
        //    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js" ) );

        bundles.Add( new ScriptBundle( "~/bundles/RockLibs" ).Include(
            "~/Scripts/jquery.js",
            "~/Scripts/jquery-ui-1.10.0.custom.min.js",
            "~/Scripts/Kendo/kendo.web.min.js",
            "~/Scripts/bootstrap.min.js",
            "~/Scripts/bootbox.min.js",
            "~/Scripts/jquery.tinyscrollbar.js" ) );

        bundles.Add( new ScriptBundle( "~/bundles/RockUi" ).Include( 
            "~/Scripts/Rock/controls/*.js" ) );

        bundles.Add( new ScriptBundle( "~/bundles/RockValidation" ).Include(
            "~/Scripts/Rock/validate/*.js" ) );

        // Creating a separate "Admin" bundle specifically for JS functionality that needs
        // to be included for administrative users
        bundles.Add( new ScriptBundle( "~/bundles/RockAdmin" ).Include( 
            "~/Scripts/Rock/admin/*.js" ) );

        // TODO: Consider adding a MEF component to dynamically load external bundle configurations
	}
}