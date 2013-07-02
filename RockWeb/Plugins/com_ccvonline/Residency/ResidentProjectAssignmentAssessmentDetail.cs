using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    [LinkedPage( "Resident Project Assignment Page" )]
    public partial class ResidentProjectAssignmentAssessmentDetail : RockBlock, IDimmableBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // NOTE:  this is special case of where we need two key fields, and no add or delete
            gList.DataKeyNames = new string[] { "ProjectPointOfAssessmentId", "CompetencyPersonProjectAssignmentAssessmentId" };
            gList.Actions.ShowAdd = false;
            gList.IsDeleteEnabled = false;
            gList.GridRebind += gList_GridRebind;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                int? competencyPersonProjectAssignmentAssessmentId = this.PageParameter( "competencyPersonProjectAssignmentAssessmentId" ).AsInteger();
                hfCompetencyPersonProjectAssignmentAssessmentId.Value = competencyPersonProjectAssignmentAssessmentId.ToString();
                BindGrid();
            }
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Handles the Edit event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( "projectPointOfAssessmentId", (int)e.RowKeyValues["ProjectPointOfAssessmentId"], "competencyPersonProjectAssignmentAssessmentId", (int)e.RowKeyValues["CompetencyPersonProjectAssignmentAssessmentId"] );
        }

        /// <summary>
        /// Handles the GridRebind event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gList_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            int competencyPersonProjectAssignmentAssessmentId = hfCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt();

            CompetencyPersonProjectAssignmentAssessment competencyPersonProjectAssignmentAssessment
                = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>().Get( competencyPersonProjectAssignmentAssessmentId );

            if ( competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add("competencyPersonProjectAssignmentId", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId.ToString());
                NavigateToParentPage( queryString );
                return;
            }

            List<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment> personPointOfAssessmentList = new ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Queryable()
                .Where( a => a.CompetencyPersonProjectAssignmentAssessmentId.Equals( competencyPersonProjectAssignmentAssessmentId ) ).ToList();

            

            List<ProjectPointOfAssessment> projectPointOfAssessmentList;
            if ( competencyPersonProjectAssignmentAssessment != null )
            {
                projectPointOfAssessmentList = new ResidencyService<ProjectPointOfAssessment>().Queryable()
                    .Where( a => a.ProjectId.Equals( competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.ProjectId ) ).ToList();
            }
            else
            {
                projectPointOfAssessmentList = new List<ProjectPointOfAssessment>();
            }

            var joinedItems = from projectPointOfAssessment in projectPointOfAssessmentList
                              join personPointOfAssessment in personPointOfAssessmentList
                              on projectPointOfAssessment.Id equals personPointOfAssessment.ProjectPointOfAssessmentId into groupJoin
                              from qryResult in groupJoin.DefaultIfEmpty()
                              select new
                              {
                                  // note: two key fields, since we want to show all the Points of Assessment for this Project, if the person hasn't had a rating on it yet
                                  ProjectPointOfAssessmentId = projectPointOfAssessment.Id,
                                  CompetencyPersonProjectAssignmentAssessmentId = competencyPersonProjectAssignmentAssessmentId,
                                  ProjectPointOfAssessment = projectPointOfAssessment,
                                  CompetencyPersonProjectAssignmentAssessmentPointOfAssessment = personPointOfAssessmentList.FirstOrDefault( a => a.ProjectPointOfAssessmentId.Equals( projectPointOfAssessment.Id ) )
                              };


            string residentProjectAssignmentPageGuid = this.GetAttributeValue( "ResidentProjectAssignmentPage" );
            string projectAssignmentHtml = string.Format( "{0} - {1}", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name
                , competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Description );
            if ( !string.IsNullOrWhiteSpace( residentProjectAssignmentPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentProjectAssignmentPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectAssignmentId", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                projectAssignmentHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, projectAssignmentHtml );
            }

            lblProjectAssignmentDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person )
                .Add( "Project Assignment", projectAssignmentHtml )
                .Add( "Assessor", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson )
                .Add( "Assessment Date/Time", competencyPersonProjectAssignmentAssessment.AssessmentDateTime )
                .StartSecondColumn()
                .Add( "Rating", competencyPersonProjectAssignmentAssessment.OverallRating.ToString() )
                .Add( "Competency", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Competency.Name )
                .Add( "Track", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Competency.Track.Name )
                .Add( "Period", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Competency.Track.Period.Name )
                .Html;


            gList.DataSource = joinedItems.OrderBy( s => s.ProjectPointOfAssessment.AssessmentOrder ).ToList();

            gList.DataBind();
        }

        #endregion

        #region IDimmableBlock

        /// <summary>
        /// Sets the dimmed.
        /// </summary>
        /// <param name="dimmed">if set to <c>true</c> [dimmed].</param>
        public void SetDimmed( bool dimmed )
        {
            gList.Enabled = !dimmed;
        }

        #endregion
    }
}