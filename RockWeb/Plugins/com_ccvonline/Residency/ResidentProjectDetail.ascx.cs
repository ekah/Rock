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
    [LinkedPage( "Resident Competency Page" )]
    [LinkedPage( "Grade Page" )]
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
            string competencyPersonHtml = competencyPersonProject.CompetencyPerson.Competency.Name;
            if ( !string.IsNullOrWhiteSpace( residentCompetencyPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentCompetencyPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonId", competencyPersonProject.CompetencyPersonId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                competencyPersonHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, competencyPersonProject.CompetencyPerson.Competency.Name );
            }

            lblMainDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProject.CompetencyPerson.Person )
                .Add( "Project", string.Format( "{0} - {1}", competencyPersonProject.Project.Name, competencyPersonProject.Project.Description ) )
                .Add( "Competency", competencyPersonHtml )
                .StartSecondColumn()
                .Add( "Period", competencyPersonProject.Project.Competency.Track.Period.Name )
                .Add( "Track", competencyPersonProject.Project.Competency.Track.Name )
                .Html;
        }

        /// <summary>
        /// Handles the Click event of the btnGrade control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnGrade_Click( object sender, EventArgs e )
        {
            string gradePageGuid = this.GetAttributeValue( "GradePage" );
            if ( !string.IsNullOrWhiteSpace( gradePageGuid ) )
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add("competencyPersonProjectId", hfCompetencyPersonProjectId.Value);
                var page = new PageService().Get( new Guid( gradePageGuid ) );
                NavigateToPage( page.Guid, queryString );
            }
        }

        #endregion
    }
}