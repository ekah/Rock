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
    /// 
    /// </summary>
    [LinkedPage( "Resident Project Assignment Page" )]
    public partial class CompetencyPersonProjectAssignmentAssessmentDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "competencyPersonProjectAssignmentAssessmentId" );
                string competencyPersonProjectAssignmentId = PageParameter( "competencyPersonProjectAssignmentId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( competencyPersonProjectAssignmentId ) )
                    {
                        ShowDetail( "competencyPersonProjectAssignmentAssessmentId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "competencyPersonProjectAssignmentAssessmentId", int.Parse( itemId ), int.Parse( competencyPersonProjectAssignmentId ) );
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
            SetEditMode( false );

            if ( hfCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the Project Assignment Detail page, return to that
                string competencyPersonProjectAssignmentId = PageParameter( "competencyPersonProjectAssignmentId" );
                if ( !string.IsNullOrWhiteSpace( competencyPersonProjectAssignmentId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["competencyPersonProjectAssignmentId"] = competencyPersonProjectAssignmentId;
                    NavigateToParentPage( qryString );
                }
                else
                {
                    NavigateToParentPage();
                }
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<CompetencyPersonProjectAssignmentAssessment> service = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>();
                CompetencyPersonProjectAssignmentAssessment item = service.Get( hfCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt() );
                ShowReadonlyDetails( item );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            ResidencyService<CompetencyPersonProjectAssignmentAssessment> service = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>();
            CompetencyPersonProjectAssignmentAssessment item = service.Get( hfCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt() );
            ShowEditDetails( item );
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;

            DimOtherBlocks( editable );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            CompetencyPersonProjectAssignmentAssessment competencyPersonProjectAssignmentAssessment;
            ResidencyService<CompetencyPersonProjectAssignmentAssessment> competencyPersonProjectAssignmentAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>();

            int competencyPersonProjectAssignmentAssessmentId = int.Parse( hfCompetencyPersonProjectAssignmentAssessmentId.Value );

            if ( competencyPersonProjectAssignmentAssessmentId == 0 )
            {
                competencyPersonProjectAssignmentAssessment = new CompetencyPersonProjectAssignmentAssessment();
                competencyPersonProjectAssignmentAssessmentService.Add( competencyPersonProjectAssignmentAssessment, CurrentPersonId );
            }
            else
            {
                competencyPersonProjectAssignmentAssessment = competencyPersonProjectAssignmentAssessmentService.Get( competencyPersonProjectAssignmentAssessmentId );
            }

            competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId = hfCompetencyPersonProjectAssignmentId.ValueAsInt();
            competencyPersonProjectAssignmentAssessment.AssessmentDateTime = dtpAssessmentDateTime.SelectedDateTime;
            competencyPersonProjectAssignmentAssessment.Rating = tbRating.Text.AsInteger();
            competencyPersonProjectAssignmentAssessment.RatingNotes = tbRatingNotes.Text;
            
            
            
            
            // TODO competencyPersonProjectAssignmentAssessment.ResidentComments = tbResidentComments.Text;




            if ( !competencyPersonProjectAssignmentAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                competencyPersonProjectAssignmentAssessmentService.Save( competencyPersonProjectAssignmentAssessment, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["competencyPersonProjectAssignmentAssessmentId"] = competencyPersonProjectAssignmentAssessment.Id.ToString();
            NavigateToPage( this.CurrentPage.Guid, qryParams );
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
        /// <param name="competencyPersonProjectAssignmentId">The residency competency person project assignment id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? competencyPersonProjectAssignmentId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonProjectAssignmentAssessmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPersonProjectAssignmentAssessment competencyPersonProjectAssignmentAssessment = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                competencyPersonProjectAssignmentAssessment = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>().Get( itemKeyValue );
            }
            else
            {
                competencyPersonProjectAssignmentAssessment = new CompetencyPersonProjectAssignmentAssessment { Id = 0 };
                competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId = competencyPersonProjectAssignmentId ?? 0;
                competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment 
                    = new ResidencyService<CompetencyPersonProjectAssignment>().Get( competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId );
            }

            hfCompetencyPersonProjectAssignmentAssessmentId.Value = competencyPersonProjectAssignmentAssessment.Id.ToString();
            hfCompetencyPersonProjectAssignmentId.Value = competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPersonProjectAssignmentAssessment.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( competencyPersonProjectAssignmentAssessment );
            }
            else
            {
                btnEdit.Visible = true;
                if ( competencyPersonProjectAssignmentAssessment.Id > 0 )
                {
                    ShowReadonlyDetails( competencyPersonProjectAssignmentAssessment );
                }
                else
                {
                    ShowEditDetails( competencyPersonProjectAssignmentAssessment );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="competencyPersonProjectAssignmentAssessment">The residency competency person project assignment assessment.</param>
        private void ShowEditDetails( CompetencyPersonProjectAssignmentAssessment competencyPersonProjectAssignmentAssessment )
        {
            if ( competencyPersonProjectAssignmentAssessment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProjectAssignmentAssessment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProjectAssignmentAssessment.FriendlyTypeName );
            }

            SetEditMode( true );

            lblResident.Text = competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person.FullName;
            lblCompetency.Text = competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Competency.Name;
            lblProjectName.Text = competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name;
            
            if ( competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson != null )
            {
                lblAssessor.Text = competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson.FullName;
            }
            else
            {
                lblAssessor.Text = Rock.Constants.None.Text;
            }

            dtpAssessmentDateTime.SelectedDateTime = competencyPersonProjectAssignmentAssessment.AssessmentDateTime;
            tbRating.Text = competencyPersonProjectAssignmentAssessment.Rating.ToString();
            tbRatingNotes.Text = competencyPersonProjectAssignmentAssessment.RatingNotes;
            tbResidentComments.Text = competencyPersonProjectAssignmentAssessment.ResidentComments;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competencyPersonProjectAssignmentAssessment">The residency competency person project assignment assessment.</param>
        private void ShowReadonlyDetails( CompetencyPersonProjectAssignmentAssessment competencyPersonProjectAssignmentAssessment )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Resident", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person.FullName );
            lblMainDetails.Text += string.Format( descriptionFormat, "Competency", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Competency.Name );

            if ( competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson != null )
            {
                lblMainDetails.Text += string.Format( descriptionFormat, "Assessor", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson.FullName );
            }

            string residentProjectAssignmentPageGuid = this.GetAttributeValue( "ResidentProjectAssignmentPage" );
            string projectAssignmentHtml = competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name;
            if ( !string.IsNullOrWhiteSpace( residentProjectAssignmentPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentProjectAssignmentPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectAssignmentId", competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                projectAssignmentHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, competencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name);
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Project Assignment", projectAssignmentHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}