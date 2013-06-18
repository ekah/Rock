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

namespace RockWeb.Plugins.com.ccvonline.Residency
{
    /// <summary>
    /// Note: This isn't a standard DetailPage.  It takes a two parameters instead of just one
    /// </summary>
    [LinkedPage( "Resident Project Assignment Assessment Page" )]
    public partial class CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentDetail : RockBlock
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
                int projectPointOfAssessmentId = PageParameter( "projectPointOfAssessmentId" ).AsInteger() ?? 0;
                int competencyPersonProjectAssignmentAssessmentId = PageParameter( "competencyPersonProjectAssignmentAssessmentId" ).AsInteger() ?? 0;
                ShowDetail( projectPointOfAssessmentId, competencyPersonProjectAssignmentAssessmentId );
            }
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            // Cancelling on Add/Edit.  Return to Grid
            // if this page was called from the CompetencyPersonProjectAssignmentAssessment Detail page, return to that
            string competencyPersonProjectAssignmentAssessmentId = PageParameter( "competencyPersonProjectAssignmentAssessmentId" );
            if ( !string.IsNullOrWhiteSpace( competencyPersonProjectAssignmentAssessmentId ) )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["competencyPersonProjectAssignmentAssessmentId"] = competencyPersonProjectAssignmentAssessmentId;
                NavigateToParentPage( qryString );
            }
            else
            {
                NavigateToParentPage();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            int projectPointOfAssessmentId = hfProjectPointOfAssessmentId.ValueAsInt();
            int competencyPersonProjectAssignmentAssessmentId = hfCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt();

            var competencyPersonProjectAssignmentAssessmentPointOfAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>();
            CompetencyPersonProjectAssignmentAssessmentPointOfAssessment competencyPersonProjectAssignmentAssessmentPointOfAssessment = null;
            competencyPersonProjectAssignmentAssessmentPointOfAssessment = competencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Queryable()
                .Where( a => a.ProjectPointOfAssessmentId.Equals( projectPointOfAssessmentId ) && a.CompetencyPersonProjectAssignmentAssessmentId.Equals( competencyPersonProjectAssignmentAssessmentId ) )
                .FirstOrDefault();

            if ( competencyPersonProjectAssignmentAssessmentPointOfAssessment == null )
            {
                // Create a record to store the rating for this PointOfAssessment if one doesn't already exist
                competencyPersonProjectAssignmentAssessmentPointOfAssessment = new CompetencyPersonProjectAssignmentAssessmentPointOfAssessment { Id = 0 };
                competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId = projectPointOfAssessmentId;
                competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId = competencyPersonProjectAssignmentAssessmentId;
                competencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Add( competencyPersonProjectAssignmentAssessmentPointOfAssessment, CurrentPersonId );
            }
            
            competencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating = tbRating.Text.AsInteger();
            competencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes = tbRatingNotes.Text;

            if ( !competencyPersonProjectAssignmentAssessmentPointOfAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                competencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Save( competencyPersonProjectAssignmentAssessmentPointOfAssessment, CurrentPersonId );
            } );

            if ( competencyPersonProjectAssignmentAssessmentId != 0 )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["competencyPersonProjectAssignmentAssessmentId"] = competencyPersonProjectAssignmentAssessmentId.ToString();
                NavigateToParentPage( qryString );
            }
            else
            {
                NavigateToParentPage();
            }
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="projectPointOfAssessmentId">The residency project point of assessment id.</param>
        /// <param name="competencyPersonProjectAssignmentAssessmentId">The residency competency person project assignment assessment id.</param>
        public void ShowDetail( int projectPointOfAssessmentId, int competencyPersonProjectAssignmentAssessmentId )
        {
            pnlDetails.Visible = true;

            var qry = new ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Queryable();
            CompetencyPersonProjectAssignmentAssessmentPointOfAssessment competencyPersonProjectAssignmentAssessmentPointOfAssessment = null;
            competencyPersonProjectAssignmentAssessmentPointOfAssessment = qry
                .Where( a => a.ProjectPointOfAssessmentId.Equals( projectPointOfAssessmentId ) && a.CompetencyPersonProjectAssignmentAssessmentId.Equals( competencyPersonProjectAssignmentAssessmentId ) ).FirstOrDefault();

            if ( competencyPersonProjectAssignmentAssessmentPointOfAssessment == null )
            {
                competencyPersonProjectAssignmentAssessmentPointOfAssessment = new CompetencyPersonProjectAssignmentAssessmentPointOfAssessment { Id = 0 };
                competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId = projectPointOfAssessmentId;
                competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment 
                    = new ResidencyService<ProjectPointOfAssessment>().Get( projectPointOfAssessmentId );
                competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId = competencyPersonProjectAssignmentAssessmentId;
                competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessment
                    = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>().Get(competencyPersonProjectAssignmentAssessmentId);
            }

            hfProjectPointOfAssessmentId.Value = competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId.ToString();
            hfCompetencyPersonProjectAssignmentAssessmentId.Value = competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }

            if ( competencyPersonProjectAssignmentAssessmentPointOfAssessment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }

            var personProject = competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject;

            lblResident.Text = personProject.CompetencyPerson.Person.FullName;
            lblCompetency.Text = personProject.CompetencyPerson.Competency.Name;
            lblProjectName.Text = personProject.Project.Name;

            var projectAssignment = competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment;
            if ( projectAssignment.AssessorPerson != null )
            {
                lblAssessor.Text = projectAssignment.AssessorPerson.FullName;
            }
            else
            {
                lblAssessor.Text = Rock.Constants.None.Text;
            }

            lblAssessmentOrder.Text = competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentOrder.ToString();
            lblAssessmentText.Text = competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentText.ToString();
            tbRating.Text = competencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating.ToString();
            tbRatingNotes.Text = competencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes;

            tbRating.ReadOnly = readOnly;
            tbRatingNotes.ReadOnly = readOnly;

            btnCancel.Visible = !readOnly;
            btnSave.Text = readOnly ? "Close" : "Save";
        }

        #endregion
    }
}