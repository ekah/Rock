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
                int residencyProjectPointOfAssessmentId = PageParameter( "residencyProjectPointOfAssessmentId" ).AsInteger() ?? 0;
                int residencyCompetencyPersonProjectAssignmentAssessmentId = PageParameter( "residencyCompetencyPersonProjectAssignmentAssessmentId" ).AsInteger() ?? 0;
                ShowDetail( residencyProjectPointOfAssessmentId, residencyCompetencyPersonProjectAssignmentAssessmentId );
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
            // if this page was called from the ResidencyCompetencyPersonProjectAssignmentAssessment Detail page, return to that
            string residencyCompetencyPersonProjectAssignmentAssessmentId = PageParameter( "residencyCompetencyPersonProjectAssignmentAssessmentId" );
            if ( !string.IsNullOrWhiteSpace( residencyCompetencyPersonProjectAssignmentAssessmentId ) )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["residencyCompetencyPersonProjectAssignmentAssessmentId"] = residencyCompetencyPersonProjectAssignmentAssessmentId;
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
            int residencyProjectPointOfAssessmentId = hfResidencyProjectPointOfAssessmentId.ValueAsInt();
            int residencyCompetencyPersonProjectAssignmentAssessmentId = hfResidencyCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt();

            var residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>();
            CompetencyPersonProjectAssignmentAssessmentPointOfAssessment residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = null;
            residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Queryable()
                .Where( a => a.ProjectPointOfAssessmentId.Equals( residencyProjectPointOfAssessmentId ) && a.CompetencyPersonProjectAssignmentAssessmentId.Equals( residencyCompetencyPersonProjectAssignmentAssessmentId ) )
                .FirstOrDefault();

            if ( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment == null )
            {
                // Create a record to store the rating for this PointOfAssessment if one doesn't already exist
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = new CompetencyPersonProjectAssignmentAssessmentPointOfAssessment { Id = 0 };
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId = residencyProjectPointOfAssessmentId;
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId = residencyCompetencyPersonProjectAssignmentAssessmentId;
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Add( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment, CurrentPersonId );
            }
            
            residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating = tbRating.Text.AsInteger();
            residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes = tbRatingNotes.Text;

            if ( !residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Save( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment, CurrentPersonId );
            } );

            if ( residencyCompetencyPersonProjectAssignmentAssessmentId != 0 )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["residencyCompetencyPersonProjectAssignmentAssessmentId"] = residencyCompetencyPersonProjectAssignmentAssessmentId.ToString();
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
        /// <param name="residencyProjectPointOfAssessmentId">The residency project point of assessment id.</param>
        /// <param name="residencyCompetencyPersonProjectAssignmentAssessmentId">The residency competency person project assignment assessment id.</param>
        public void ShowDetail( int residencyProjectPointOfAssessmentId, int residencyCompetencyPersonProjectAssignmentAssessmentId )
        {
            pnlDetails.Visible = true;

            var qry = new ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Queryable();
            CompetencyPersonProjectAssignmentAssessmentPointOfAssessment residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = null;
            residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = qry
                .Where( a => a.ProjectPointOfAssessmentId.Equals( residencyProjectPointOfAssessmentId ) && a.CompetencyPersonProjectAssignmentAssessmentId.Equals( residencyCompetencyPersonProjectAssignmentAssessmentId ) ).FirstOrDefault();

            if ( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment == null )
            {
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = new CompetencyPersonProjectAssignmentAssessmentPointOfAssessment { Id = 0 };
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId = residencyProjectPointOfAssessmentId;
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment 
                    = new ResidencyService<ProjectPointOfAssessment>().Get( residencyProjectPointOfAssessmentId );
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId = residencyCompetencyPersonProjectAssignmentAssessmentId;
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessment
                    = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>().Get(residencyCompetencyPersonProjectAssignmentAssessmentId);
            }

            hfResidencyProjectPointOfAssessmentId.Value = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId.ToString();
            hfResidencyCompetencyPersonProjectAssignmentAssessmentId.Value = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }

            if ( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }

            var personProject = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject;

            lblResident.Text = personProject.CompetencyPerson.Person.FullName;
            lblCompetency.Text = personProject.CompetencyPerson.Competency.Name;
            lblProjectName.Text = personProject.Project.Name;

            var projectAssignment = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment;
            if ( projectAssignment.AssessorPerson != null )
            {
                lblAssessor.Text = projectAssignment.AssessorPerson.FullName;
            }
            else
            {
                lblAssessor.Text = Rock.Constants.None.Text;
            }

            lblAssessmentOrder.Text = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentOrder.ToString();
            lblAssessmentText.Text = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentText.ToString();
            tbRating.Text = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating.ToString();
            tbRatingNotes.Text = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes;

            tbRating.ReadOnly = readOnly;
            tbRatingNotes.ReadOnly = readOnly;

            btnCancel.Visible = !readOnly;
            btnSave.Text = readOnly ? "Close" : "Save";
        }

        #endregion
    }
}