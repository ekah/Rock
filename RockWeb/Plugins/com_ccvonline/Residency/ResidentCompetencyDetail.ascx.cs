//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;
using System.Web.UI.WebControls;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    public partial class ResidentCompetencyDetail : RockBlock, IDetailBlock
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
                string competencyPersonId = PageParameter( "competencyPersonId" );
                if ( !string.IsNullOrWhiteSpace( competencyPersonId ) )
                {
                    ShowDetail( "competencyPersonId", int.Parse( competencyPersonId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            hfCompetencyPersonId.Value = itemKeyValue.ToString();

            CompetencyPerson competencyPerson = new ResidencyService<CompetencyPerson>().Get(hfCompetencyPersonId.ValueAsInt());

            if ( competencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                NavigateToParentPage();
                return;
            }

            lblCompetencyName.Text = competencyPerson.Competency.Name;
            lblFacilitator.Text = competencyPerson.Competency.FacilitatorPerson != null ? competencyPerson.Competency.FacilitatorPerson.FullName : Rock.Constants.None.TextHtml;
            lblDescription.Text = !string.IsNullOrWhiteSpace( competencyPerson.Competency.Description ) ? competencyPerson.Competency.Description : Rock.Constants.None.TextHtml;
            
        }
    }
}