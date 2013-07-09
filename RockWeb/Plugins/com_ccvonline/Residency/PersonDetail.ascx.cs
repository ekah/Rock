//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using Rock;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PersonDetail : RockBlock, IDetailBlock
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
                int personId = this.PageParameter( "personId" ).AsInteger() ?? 0;

                if ( personId != 0 )
                {
                    ShowDetail( "personId", personId );
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

            int? personId = this.PageParameter( pageReference, "personId" ).AsInteger();
            if ( personId != null )
            {
                Person person = new PersonService().Get( personId.Value );
                if ( person != null )
                {
                    breadCrumbs.Add( new BreadCrumb( person.FullName, pageReference ) );
                }
                else
                {
                    breadCrumbs.Add( new BreadCrumb( "Resident", pageReference ) );
                }
            }
            else
            {
                // don't show a breadcrumb if we don't have a pageparam to work with
            }

            return breadCrumbs;
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "personId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // this is always just a View block (they are added/edited on the GroupMember block)
            Person person = new ResidencyService<Person>().Get( itemKeyValue );

            ShowReadonlyDetails( person );
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="person">The person.</param>
        private void ShowReadonlyDetails( Person person )
        {
            fieldsetViewDetails.Visible = true;

            lblMainDetails.Text = new DescriptionList()
                .Add( "Name", person )
                .Html;
        }

        #endregion
    }
}