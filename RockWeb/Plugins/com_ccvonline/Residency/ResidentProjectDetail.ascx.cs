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
using Rock.Attribute;
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
    [LinkedPage( "Grade Request Page" )]
    public partial class ResidentProjectDetail : RockBlock, IDetailBlock
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
                string competencyPersonProjectId = PageParameter( "competencyPersonProjectId" );
                if ( !string.IsNullOrWhiteSpace( competencyPersonProjectId ) )
                {
                    ShowDetail( "competencyPersonProjectId", int.Parse( competencyPersonProjectId ) );
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

            int? competencyPersonProjectId = this.PageParameter( pageReference, "competencyPersonProjectId" ).AsInteger();
            if ( competencyPersonProjectId != null )
            {
                CompetencyPersonProject competencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( competencyPersonProjectId.Value );
                if ( competencyPersonProject != null )
                {
                    breadCrumbs.Add( new BreadCrumb( competencyPersonProject.Project.Name, pageReference ) );
                }
                else
                {
                    breadCrumbs.Add( new BreadCrumb( "Project", pageReference ) );
                }
            }
            else
            {
                // don't show a breadcrumb if we don't have a pageparam to work with
            }

            return breadCrumbs;
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonProjectId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            CompetencyPersonProject competencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( itemKeyValue );

            if ( competencyPersonProject.CompetencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonId", competencyPersonProject.CompetencyPersonId.ToString() );
                NavigateToParentPage( queryString );
                return;
            }

            hfCompetencyPersonProjectId.Value = competencyPersonProject.Id.ToString();

            ShowReadonlyDetails( competencyPersonProject );
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competencyPersonProject">The competency person project.</param>
        private void ShowReadonlyDetails( CompetencyPersonProject competencyPersonProject )
        {
            string residentCompetencyPageGuid = this.GetAttributeValue( "ResidentCompetencyPage" );

            lblMainDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProject.CompetencyPerson.Person )
                .Add( "Competency", competencyPersonProject.CompetencyPerson.Competency.Name )
                .Add( "Project", string.Format( "{0} - {1}", competencyPersonProject.Project.Name, competencyPersonProject.Project.Description ) )
                .Html;
        }

        /// <summary>
        /// Handles the Click event of the btnGrade control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnGrade_Click( object sender, EventArgs e )
        {
            string gradeRequestPageGuid = this.GetAttributeValue( "GradeRequestPage" );
            if ( !string.IsNullOrWhiteSpace( gradeRequestPageGuid ) )
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectId", hfCompetencyPersonProjectId.Value );
                var page = new PageService().Get( new Guid( gradeRequestPageGuid ) );
                NavigateToPage( page.Guid, queryString );
            }
        }

        #endregion
    }
}