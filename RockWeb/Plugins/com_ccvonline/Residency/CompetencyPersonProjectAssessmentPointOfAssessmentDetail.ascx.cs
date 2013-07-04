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
    /// Note: This isn't a standard DetailPage.  It takes a two parameters instead of just one
    /// </summary>
    [LinkedPage( "Resident Project Assessment Page" )]
    public partial class CompetencyPersonProjectAssessmentPointOfAssessmentDetail : RockBlock
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
                int competencyPersonProjectAssessmentId = PageParameter( "competencyPersonProjectAssessmentId" ).AsInteger() ?? 0;
                ShowDetail( projectPointOfAssessmentId, competencyPersonProjectAssessmentId );
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
            // if this page was called from the CompetencyPersonProjectAssessment Detail page, return to that
            string competencyPersonProjectAssessmentId = PageParameter( "competencyPersonProjectAssessmentId" );
            if ( !string.IsNullOrWhiteSpace( competencyPersonProjectAssessmentId ) )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["competencyPersonProjectAssessmentId"] = competencyPersonProjectAssessmentId;
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
            int competencyPersonProjectAssessmentId = hfCompetencyPersonProjectAssessmentId.ValueAsInt();

            var competencyPersonProjectAssessmentPointOfAssessmentService = new ResidencyService<CompetencyPersonProjectAssessmentPointOfAssessment>();
            CompetencyPersonProjectAssessmentPointOfAssessment competencyPersonProjectAssessmentPointOfAssessment = null;
            competencyPersonProjectAssessmentPointOfAssessment = competencyPersonProjectAssessmentPointOfAssessmentService.Queryable()
                .Where( a => a.ProjectPointOfAssessmentId.Equals( projectPointOfAssessmentId ) && a.CompetencyPersonProjectAssessmentId.Equals( competencyPersonProjectAssessmentId ) )
                .FirstOrDefault();

            if ( competencyPersonProjectAssessmentPointOfAssessment == null )
            {
                // Create a record to store the rating for this PointOfAssessment if one doesn't already exist
                competencyPersonProjectAssessmentPointOfAssessment = new CompetencyPersonProjectAssessmentPointOfAssessment { Id = 0 };
                competencyPersonProjectAssessmentPointOfAssessment.ProjectPointOfAssessmentId = projectPointOfAssessmentId;
                competencyPersonProjectAssessmentPointOfAssessment.CompetencyPersonProjectAssessmentId = competencyPersonProjectAssessmentId;
                competencyPersonProjectAssessmentPointOfAssessmentService.Add( competencyPersonProjectAssessmentPointOfAssessment, CurrentPersonId );
            }

            competencyPersonProjectAssessmentPointOfAssessment.Rating = tbRating.Text.AsInteger();
            competencyPersonProjectAssessmentPointOfAssessment.RatingNotes = tbRatingNotes.Text;

            if ( !competencyPersonProjectAssessmentPointOfAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                competencyPersonProjectAssessmentPointOfAssessmentService.Save( competencyPersonProjectAssessmentPointOfAssessment, CurrentPersonId );

                // get the CompetencyPersonProjectAssessment using the same dbContext 
                var competencyPersonProjectAssessmentService = new ResidencyService<CompetencyPersonProjectAssessment>( competencyPersonProjectAssessmentPointOfAssessmentService.ResidencyContext );
                CompetencyPersonProjectAssessment competencyPersonProjectAssessment = competencyPersonProjectAssessmentService.Get( competencyPersonProjectAssessmentId );

                // set Overall Rating based on average of POA ratings
                competencyPersonProjectAssessment.OverallRating = (decimal?)competencyPersonProjectAssessment.CompetencyPersonProjectAssessmentPointOfAssessments.Average( a => a.Rating );
                competencyPersonProjectAssessmentService.Save( competencyPersonProjectAssessment, CurrentPersonId );
            } );

            if ( competencyPersonProjectAssessmentId != 0 )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["competencyPersonProjectAssessmentId"] = competencyPersonProjectAssessmentId.ToString();
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
        /// <param name="projectPointOfAssessmentId">The project point of assessment id.</param>
        /// <param name="competencyPersonProjectAssessmentId">The competency person project assessment id.</param>
        public void ShowDetail( int projectPointOfAssessmentId, int competencyPersonProjectAssessmentId )
        {
            pnlDetails.Visible = true;

            var qry = new ResidencyService<CompetencyPersonProjectAssessmentPointOfAssessment>().Queryable();
            CompetencyPersonProjectAssessmentPointOfAssessment competencyPersonProjectAssessmentPointOfAssessment = null;
            competencyPersonProjectAssessmentPointOfAssessment = qry
                .Where( a => a.ProjectPointOfAssessmentId.Equals( projectPointOfAssessmentId ) && a.CompetencyPersonProjectAssessmentId.Equals( competencyPersonProjectAssessmentId ) ).FirstOrDefault();

            if ( competencyPersonProjectAssessmentPointOfAssessment == null )
            {
                competencyPersonProjectAssessmentPointOfAssessment = new CompetencyPersonProjectAssessmentPointOfAssessment { Id = 0 };
                competencyPersonProjectAssessmentPointOfAssessment.ProjectPointOfAssessmentId = projectPointOfAssessmentId;
                competencyPersonProjectAssessmentPointOfAssessment.ProjectPointOfAssessment
                    = new ResidencyService<ProjectPointOfAssessment>().Get( projectPointOfAssessmentId );
                competencyPersonProjectAssessmentPointOfAssessment.CompetencyPersonProjectAssessmentId = competencyPersonProjectAssessmentId;
                competencyPersonProjectAssessmentPointOfAssessment.CompetencyPersonProjectAssessment
                    = new ResidencyService<CompetencyPersonProjectAssessment>().Get( competencyPersonProjectAssessmentId );
            }

            hfProjectPointOfAssessmentId.Value = competencyPersonProjectAssessmentPointOfAssessment.ProjectPointOfAssessmentId.ToString();
            hfCompetencyPersonProjectAssessmentId.Value = competencyPersonProjectAssessmentPointOfAssessment.CompetencyPersonProjectAssessmentId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( "Project Assessment- Point Of Assessment" );
            }

            if ( competencyPersonProjectAssessmentPointOfAssessment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( "Project Assessment- Point Of Assessment" );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( "Project Assessment- Point Of Assessment" );
            }

            var personProject = competencyPersonProjectAssessmentPointOfAssessment.CompetencyPersonProjectAssessment.CompetencyPersonProject;
            var projectAssessment = competencyPersonProjectAssessmentPointOfAssessment.CompetencyPersonProjectAssessment;

            lblMainDetails.Text = new DescriptionList()
                .Add( "Resident", personProject.CompetencyPerson.Person )
                .Add( "Project", string.Format( "{0} - {1}", personProject.Project.Name, personProject.Project.Description ) )
                .Add( "Assessment #", competencyPersonProjectAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentOrder )
                .Add( "Assessment Text", competencyPersonProjectAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentText )
                .StartSecondColumn()
                .Add( "Competency", personProject.CompetencyPerson.Competency.Name )
                .Add( "Track", personProject.CompetencyPerson.Competency.Track.Name )
                .Add( "Assessor", projectAssessment.AssessorPerson )
                .Html;

            tbRating.Text = competencyPersonProjectAssessmentPointOfAssessment.Rating.ToString();
            tbRatingNotes.Text = competencyPersonProjectAssessmentPointOfAssessment.RatingNotes;

            tbRating.ReadOnly = readOnly;
            tbRatingNotes.ReadOnly = readOnly;

            btnCancel.Visible = !readOnly;
            btnSave.Text = readOnly ? "Close" : "Save";
        }

        #endregion
    }
}