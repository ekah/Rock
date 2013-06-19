//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Constants;
using Rock.Data;
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
            Person person = new ResidencyService<Person>().Get(itemKeyValue);

            ShowReadonlyDetails( person );
            
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="person">The group member.</param>
        private void ShowReadonlyDetails( Person person )
        {
            fieldsetViewDetails.Visible = true;

            lblMainDetails.Text = new DescriptionList()
                .Add("Name", person)
                .Html;
            
        }

        #endregion
    }
}