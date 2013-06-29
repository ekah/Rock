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
    [LinkedPage( "Person Project Detail Page" )]
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

            // minimize the chance of using the Browser Back button to accidently "re-grade" the project after the residentGraderSessionKey has expired
            Page.Response.Cache.SetCacheability( System.Web.HttpCacheability.NoCache );
            Page.Response.Cache.SetExpires( DateTime.UtcNow.AddHours( -1 ) );
            Page.Response.Cache.SetNoStore();
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
            CompetencyPersonProjectAssignment competencyPersonProjectAssignment;
            ResidencyService<CompetencyPersonProjectAssignment> competencyPersonProjectAssignmentService = new ResidencyService<CompetencyPersonProjectAssignment>();

            CompetencyPersonProjectAssignmentAssessment competencyPersonProjectAssignmentAssessment;
            ResidencyService<CompetencyPersonProjectAssignmentAssessment> competencyPersonProjectAssignmentAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>();

            ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment> competencyPersonProjectAssignmentAssessmentPointOfAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>();

            int competencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();
            int competencyPersonProjectAssignmentId = hfCompetencyPersonProjectAssignmentId.ValueAsInt();
            if ( competencyPersonProjectAssignmentId == 0 )
            {
                competencyPersonProjectAssignment = new CompetencyPersonProjectAssignment();
                competencyPersonProjectAssignmentService.Add( competencyPersonProjectAssignment, CurrentPersonId );
            }
            else
            {
                competencyPersonProjectAssignment = competencyPersonProjectAssignmentService.Get( competencyPersonProjectAssignmentId );
            }

            competencyPersonProjectAssignment.CompetencyPersonProjectId = competencyPersonProjectId;
            competencyPersonProjectAssignment.AssessorPersonId = hfAssessorPersonId.ValueAsInt();
            competencyPersonProjectAssignment.CompletedDateTime = competencyPersonProjectAssignment.CompletedDateTime ?? DateTime.Now;

            int competencyPersonProjectAssignmentAssessmentId = hfCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt();
            if ( competencyPersonProjectAssignmentAssessmentId == 0 )
            {
                competencyPersonProjectAssignmentAssessment = new CompetencyPersonProjectAssignmentAssessment();
                competencyPersonProjectAssignmentAssessmentService.Add( competencyPersonProjectAssignmentAssessment, CurrentPersonId );
            }
            else
            {
                competencyPersonProjectAssignmentAssessment = competencyPersonProjectAssignmentAssessmentService.Get( competencyPersonProjectAssignmentAssessmentId );
                competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentAssessmentPointOfAssessments = new List<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>();
            }

            // set competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId after saving competencyPersonProjectAssignment in case it is new
            competencyPersonProjectAssignmentAssessment.AssessmentDateTime = DateTime.Now;
            competencyPersonProjectAssignmentAssessment.RatingNotes = tbRatingNotesOverall.Text;
            //competencyPersonProjectAssignmentAssessment.ResidentComments = tbResidentComments.Text;

            if ( !competencyPersonProjectAssignmentAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            List<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment> competencyPersonProjectAssignmentAssessmentPointOfAssessmentList = new List<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>();

            foreach ( RepeaterItem item in rptPointOfAssessment.Items.OfType<RepeaterItem>() )
            {
                HiddenField hfProjectPointOfAssessmentId = item.FindControl( "hfProjectPointOfAssessmentId" ) as HiddenField;
                int projectPointOfAssessmentId = hfProjectPointOfAssessmentId.ValueAsInt();

                CompetencyPersonProjectAssignmentAssessmentPointOfAssessment competencyPersonProjectAssignmentAssessmentPointOfAssessment = competencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Queryable()
                    .Where( a => a.ProjectPointOfAssessmentId == projectPointOfAssessmentId )
                    .Where( a => a.CompetencyPersonProjectAssignmentAssessmentId == competencyPersonProjectAssignmentAssessmentId ).FirstOrDefault();

                if ( competencyPersonProjectAssignmentAssessmentPointOfAssessment == null )
                {
                    competencyPersonProjectAssignmentAssessmentPointOfAssessment = new CompetencyPersonProjectAssignmentAssessmentPointOfAssessment();
                    // set competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId = competencyPersonProjectAssignmentAssessment.Id in save in case it's new
                    competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId = projectPointOfAssessmentId;
                    
                }

                LabeledDropDownList ddlPointOfAssessmentRating = item.FindControl( "ddlPointOfAssessmentRating" ) as LabeledDropDownList;
                TextBox tbRatingNotesPOA = item.FindControl( "tbRatingNotesPOA" ) as TextBox;

                competencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating = ddlPointOfAssessmentRating.SelectedValueAsInt();
                competencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes = tbRatingNotesPOA.Text;

                competencyPersonProjectAssignmentAssessmentPointOfAssessmentList.Add( competencyPersonProjectAssignmentAssessmentPointOfAssessment );
            }


            RockTransactionScope.WrapTransaction( () =>
            {
                competencyPersonProjectAssignmentService.Save( competencyPersonProjectAssignment, CurrentPersonId );
                competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId = competencyPersonProjectAssignment.Id;

                // set Overall Rating based on average of POA ratings
                competencyPersonProjectAssignmentAssessment.OverallRating = (decimal?)competencyPersonProjectAssignmentAssessmentPointOfAssessmentList.Average( a => a.Rating );
                competencyPersonProjectAssignmentAssessmentService.Save( competencyPersonProjectAssignmentAssessment, CurrentPersonId );

                foreach ( var competencyPersonProjectAssignmentAssessmentPointOfAssessment in competencyPersonProjectAssignmentAssessmentPointOfAssessmentList )
                {
                    competencyPersonProjectAssignmentAssessmentPointOfAssessment.CompetencyPersonProjectAssignmentAssessmentId = competencyPersonProjectAssignmentAssessment.Id;

                    if ( competencyPersonProjectAssignmentAssessmentPointOfAssessment.Id == 0 )
                    {
                        competencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Add( competencyPersonProjectAssignmentAssessmentPointOfAssessment, CurrentPersonId );
                    }

                    competencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Save( competencyPersonProjectAssignmentAssessmentPointOfAssessment, CurrentPersonId );
                }

            } );

            string personProjectDetailPageGuid = this.GetAttributeValue( "PersonProjectDetailPage" );
            var page = new PageService().Get( new Guid( personProjectDetailPageGuid ) );
            if ( page != null )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["competencyPersonProjectId"] = hfCompetencyPersonProjectId.Value;
                NavigateToPage( page.Guid, qryString );
            }
            else
            {
                throw new Exception( "PersonProjectDetailPage not configured correctly" );
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
            if ( itemKey != "competencyPersonProjectId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            hfCompetencyPersonProjectId.Value = this.PageParameter( "competencyPersonProjectId" );
            int competencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();

            string encryptedKey = Session["residentGraderSessionKey"] as string;

            // clear the residentGraderSessionKey so they don't accidently grade this again with a stale grader login
            Session["residentGraderSessionKey"] = null;

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
            hfAssessorPersonId.Value = assessorPerson.Id.ToString();

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

            hfCompetencyPersonProjectAssignmentId.Value = competencyPersonProjectAssignment.Id.ToString();

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
                                                                                               ProjectPointOfAssessmentId = projectPointOfAssessment.Id,
                                                                                               ProjectPointOfAssessment = projectPointOfAssessment,
                                                                                               CompetencyPersonProjectAssignmentAssessmentId = competencyPersonProjectAssignmentAssessment.Id,
                                                                                               CompetencyPersonProjectAssignmentAssessment = competencyPersonProjectAssignmentAssessment
                                                                                           } );

            rptPointOfAssessment.DataSource = competencyPersonProjectAssignmentAssessmentPointOfAssessmentListJoined.OrderBy( a => a.ProjectPointOfAssessment.AssessmentOrder ).ToList();
            rptPointOfAssessment.DataBind();

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
                ddlPointOfAssessmentRating.Items.Add( new ListItem( "-", Rock.Constants.None.IdValue ) );
                for ( int ratingOption = 1; ratingOption <= 5; ratingOption++ )
                {
                    ddlPointOfAssessmentRating.Items.Add( new ListItem( ratingOption.ToString(), ratingOption.ToString() ) );
                }

                ddlPointOfAssessmentRating.SetValue( competencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating.ToString() );
                HiddenField hfProjectPointOfAssessmentId = e.Item.FindControl( "hfProjectPointOfAssessmentId" ) as HiddenField;

                hfProjectPointOfAssessmentId.Value = competencyPersonProjectAssignmentAssessmentPointOfAssessment.ProjectPointOfAssessmentId.ToString();

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