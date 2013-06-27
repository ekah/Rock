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
    public partial class ResidentGradeDetail : RockBlock, IDetailBlock
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
                ShowDetail( "competencyPersonProjectId", hfCompetencyPersonProjectId.ValueAsInt() );
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
            NavigateToGraderLogin();
        }

        /// <summary>
        /// Navigates to grader login.
        /// </summary>
        private void NavigateToGraderLogin()
        {
            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["competencyPersonProjectId"] = hfCompetencyPersonProjectId.Value;
            NavigateToParentPage( qryString );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            //todo navigate to Parent.Parent???

            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["competencyPersonProjectId"] = hfCompetencyPersonProjectId.Value;
            //NavigateToParentPage( qryString );
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ddlRatingOverall.Items.Clear();
            ddlRatingOverall.Items.Add( new ListItem( "-", "0" ) );
            for ( int ratingOption = 1; ratingOption <= 5; ratingOption++ )
            {
                ddlRatingOverall.Items.Add( ratingOption.ToString() );
            }

            ddlRatingOverall.SelectedIndex = 0;
        }

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

            LoadDropDowns();

            hfCompetencyPersonProjectId.Value = this.PageParameter( "competencyPersonProjectId" );
            int competencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();

            string encryptedKey = Session["residentGraderSessionKey"] as string;
            string residentGraderSessionKey = string.Empty;

            if ( !string.IsNullOrWhiteSpace( encryptedKey ) )
            {
                residentGraderSessionKey = Rock.Security.Encryption.DecryptString( encryptedKey );
            }

            string[] residentGraderSessionKeyParts = residentGraderSessionKey.Split( new char[] { '|' } );

            Person assessorPerson = null;

            // verify that the residentGraderSessionKey is for this Project, has a valid Person, and isn't stale (helps minimize the chance of incorrect teacher from a previous teacher login)
            if ( residentGraderSessionKeyParts.Length == 3 )
            {
                string userLoginGuid = residentGraderSessionKeyParts[1];
                if ( residentGraderSessionKeyParts[0].Equals( competencyPersonProjectId.ToString() ) )
                {
                    string ticks = residentGraderSessionKeyParts[2];
                    TimeSpan elapsed = DateTime.Now - new DateTime( long.Parse( ticks ) );
                    if ( elapsed.Duration().Minutes <= 10 )
                    {
                        assessorPerson = new UserLoginService().Get( new Guid( userLoginGuid ) ).Person;
                    }
                }
            }

            if ( assessorPerson == null )
            {
                NavigateToGraderLogin();
                return;
            }

            int assessorPersonId = assessorPerson.Id;

            CompetencyPersonProject competencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( competencyPersonProjectId );

            if ( competencyPersonProject.CompetencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                NavigateToParentPage();
                return;
            }

            // first, look for an incomplete assignment that needs to be completed.  Otherwise, just start a new one
            CompetencyPersonProjectAssignment competencyPersonProjectAssignment = new ResidencyService<CompetencyPersonProjectAssignment>().Queryable()
                .Where( a => a.CompetencyPersonProjectId == competencyPersonProjectId )
                .Where( a => a.AssessorPersonId == assessorPersonId )
                .Where( a => a.CompletedDateTime == null )
                .FirstOrDefault();

            if ( competencyPersonProjectAssignment == null )
            {
                competencyPersonProjectAssignment = new CompetencyPersonProjectAssignment
                {
                    AssessorPersonId = assessorPersonId,
                    CompetencyPersonProjectId = competencyPersonProjectId,
                    CompletedDateTime = null
                };
            }

            competencyPersonProjectAssignment.CompetencyPersonProjectAssignmentAssessments = competencyPersonProjectAssignment.CompetencyPersonProjectAssignmentAssessments ?? new List<CompetencyPersonProjectAssignmentAssessment>();

            // look for an incomplete assignment assessment, or start a new one
            CompetencyPersonProjectAssignmentAssessment competencyPersonProjectAssignmentAssessment = competencyPersonProjectAssignment.CompetencyPersonProjectAssignmentAssessments
                .Where( a => a.AssessmentDateTime == null ).FirstOrDefault();

            if ( competencyPersonProjectAssignmentAssessment == null )
            {
                competencyPersonProjectAssignmentAssessment = new CompetencyPersonProjectAssignmentAssessment
                {
                    CompetencyPersonProjectAssignment = competencyPersonProjectAssignment
                };
            }

            hfCompetencyPersonProjectAssignmentAssessmentId.Value = competencyPersonProjectAssignmentAssessment.Id.ToString();

            // populate page
            lblMainDetails.Text = new DescriptionList()
                .Add( "Student", competencyPersonProject.CompetencyPerson.Person )
                .Add( "Project", string.Format( "{0} - {1}", competencyPersonProject.Project.Name, competencyPersonProject.Project.Description ) )
                .Html;

            List<ProjectPointOfAssessment> projectPointOfAssessmentList = new ResidencyService<ProjectPointOfAssessment>().Queryable()
                .Where( a => a.ProjectId == competencyPersonProject.ProjectId ).ToList();

            // get any POA Ratings that might exist
            List<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment> competencyPersonProjectAssignmentAssessmentPointOfAssessmentList = new ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Queryable()
                .Where( a => a.CompetencyPersonProjectAssignmentAssessmentId == competencyPersonProjectAssignmentAssessment.Id ).ToList();

            var competencyPersonProjectAssignmentAssessmentPointOfAssessmentListJoined = from projectPointOfAssessment in projectPointOfAssessmentList
                                                                                         join poa in competencyPersonProjectAssignmentAssessmentPointOfAssessmentList
                                                                                         on projectPointOfAssessment.Id equals poa.ProjectPointOfAssessmentId into groupJoin
                                                                                         from qryResult in groupJoin.DefaultIfEmpty()
                                                                                         select ( qryResult ?? new CompetencyPersonProjectAssignmentAssessmentPointOfAssessment
                                                                                           {
                                                                                               ProjectPointOfAssessment = projectPointOfAssessment,
                                                                                               CompetencyPersonProjectAssignmentAssessmentId = competencyPersonProjectAssignmentAssessment.Id,
                                                                                               CompetencyPersonProjectAssignmentAssessment = competencyPersonProjectAssignmentAssessment
                                                                                           } );

            rptPointOfAssessment.DataSource = competencyPersonProjectAssignmentAssessmentPointOfAssessmentListJoined.OrderBy( a => a.ProjectPointOfAssessment.AssessmentOrder ).ToList();
            rptPointOfAssessment.DataBind();

            ddlRatingOverall.SetValue(competencyPersonProjectAssignmentAssessment.Rating.ToString());
            tbRatingNotesOverall.Text = competencyPersonProjectAssignmentAssessment.RatingNotes;
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rptPointOfAssessment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rptPointOfAssessment_ItemDataBound( object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e )
        {
            CompetencyPersonProjectAssignmentAssessmentPointOfAssessment competencyPersonProjectAssignmentAssessmentPointOfAssessment = e.Item.DataItem as CompetencyPersonProjectAssignmentAssessmentPointOfAssessment;
            if ( competencyPersonProjectAssignmentAssessmentPointOfAssessment != null )
            {
                LabeledDropDownList ddlPointOfAssessmentRating = e.Item.FindControl( "ddlPointOfAssessmentRating" ) as LabeledDropDownList;
                ddlPointOfAssessmentRating.Items.Clear();
                ddlPointOfAssessmentRating.Items.Add( new ListItem( "-", "0" ) );
                for ( int ratingOption = 1; ratingOption <= 5; ratingOption++ )
                {
                    ddlPointOfAssessmentRating.Items.Add( new ListItem( ratingOption.ToString(), ratingOption.ToString() ) );
                }

                ddlPointOfAssessmentRating.SetValue(competencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating.ToString());

                Literal lblAssessmentText = e.Item.FindControl( "lblAssessmentText" ) as Literal;
                lblAssessmentText.Text = string.Format( 
                    "{0}. {1}",
                    competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentOrder,
                    competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessment.AssessmentText );

                TextBox tbRatingNotesPOA = e.Item.FindControl( "tbRatingNotesPOA" ) as TextBox;
                tbRatingNotesPOA.Text = competencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes;
            }
        }

        #endregion
    }
}