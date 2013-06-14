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

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    [LinkedPage( "Resident Project Assignment Assessment Page" )]
    public partial class ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId" );
                string residencyCompetencyPersonProjectAssignmentAssessmentId = PageParameter( "residencyCompetencyPersonProjectAssignmentAssessmentId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( residencyCompetencyPersonProjectAssignmentAssessmentId ) )
                    {
                        ShowDetail( "residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId", int.Parse( itemId ), int.Parse( residencyCompetencyPersonProjectAssignmentAssessmentId ) );
                    }
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
            ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment;
            ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment> residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentService = new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment>();

            int residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId = int.Parse( hfResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId.Value );

            if ( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId == 0 )
            {
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = new ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment();
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Add( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment, CurrentPersonId );
            }
            else
            {
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentService.Get( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId );
            }

            /// TODO  not sure ..... residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyCompetencyPersonProjectAssignmentAssessmentId = hfResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId.ValueAsInt();
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
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            ShowDetail( itemKey, itemKeyValue, null );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        /// <param name="residencyCompetencyPersonProjectAssignmentAssessmentId">The residency competency id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyCompetencyPersonProjectAssignmentAssessmentId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = new ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment { Id = 0 };
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyCompetencyPersonProjectAssignmentAssessmentId = residencyCompetencyPersonProjectAssignmentAssessmentId ?? 0;
                residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyCompetencyPersonProjectAssignmentAssessment = new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessment>().Get( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyCompetencyPersonProjectAssignmentAssessmentId );
            }

            hfResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId.Value = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Id.ToString();
            hfResidencyCompetencyPersonProjectAssignmentAssessmentId.Value = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyCompetencyPersonProjectAssignmentAssessmentId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }

            if ( residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
            }

            var personProject = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyCompetencyPersonProjectAssignmentAssessment.ResidencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProject;

            lblResident.Text = personProject.ResidencyCompetencyPerson.Person.FullName;
            lblCompetency.Text = personProject.ResidencyCompetencyPerson.ResidencyCompetency.Name;
            lblProjectName.Text = personProject.ResidencyProject.Name;

            var projectAssignment = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyCompetencyPersonProjectAssignmentAssessment.ResidencyCompetencyPersonProjectAssignment;
            if ( projectAssignment.AssessorPerson != null )
            {
                lblAssessor.Text = projectAssignment.AssessorPerson.FullName;
            }
            else
            {
                lblAssessor.Text = Rock.Constants.None.Text;
            }

            lblAssessmentOrder.Text = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyProjectPointOfAssessment.AssessmentOrder.ToString();
            lblAssessmentText.Text = residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.ResidencyProjectPointOfAssessment.AssessmentText.ToString();
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