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
                string itemId = PageParameter( "residencyCompetencyPersonProjectAssignmentAssessmentId" );
                string residencyCompetencyPersonProjectAssignmentId = PageParameter( "residencyCompetencyPersonProjectAssignmentId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( residencyCompetencyPersonProjectAssignmentId ) )
                    {
                        ShowDetail( "residencyCompetencyPersonProjectAssignmentAssessmentId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyCompetencyPersonProjectAssignmentAssessmentId", int.Parse( itemId ), int.Parse( residencyCompetencyPersonProjectAssignmentId ) );
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

            if ( hfResidencyCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the Project Assignment Detail page, return to that
                string residencyCompetencyPersonProjectAssignmentId = PageParameter( "residencyCompetencyPersonProjectAssignmentId" );
                if ( !string.IsNullOrWhiteSpace( residencyCompetencyPersonProjectAssignmentId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["residencyCompetencyPersonProjectAssignmentId"] = residencyCompetencyPersonProjectAssignmentId;
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
                CompetencyPersonProjectAssignmentAssessment item = service.Get( hfResidencyCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt() );
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
            CompetencyPersonProjectAssignmentAssessment item = service.Get( hfResidencyCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt() );
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
            CompetencyPersonProjectAssignmentAssessment residencyCompetencyPersonProjectAssignmentAssessment;
            ResidencyService<CompetencyPersonProjectAssignmentAssessment> residencyCompetencyPersonProjectAssignmentAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>();

            int residencyCompetencyPersonProjectAssignmentAssessmentId = int.Parse( hfResidencyCompetencyPersonProjectAssignmentAssessmentId.Value );

            if ( residencyCompetencyPersonProjectAssignmentAssessmentId == 0 )
            {
                residencyCompetencyPersonProjectAssignmentAssessment = new CompetencyPersonProjectAssignmentAssessment();
                residencyCompetencyPersonProjectAssignmentAssessmentService.Add( residencyCompetencyPersonProjectAssignmentAssessment, CurrentPersonId );
            }
            else
            {
                residencyCompetencyPersonProjectAssignmentAssessment = residencyCompetencyPersonProjectAssignmentAssessmentService.Get( residencyCompetencyPersonProjectAssignmentAssessmentId );
            }

            residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId = hfResidencyCompetencyPersonProjectAssignmentId.ValueAsInt();
            residencyCompetencyPersonProjectAssignmentAssessment.AssessmentDateTime = dtpAssessmentDateTime.SelectedDateTime;
            residencyCompetencyPersonProjectAssignmentAssessment.Rating = tbRating.Text.AsInteger();
            residencyCompetencyPersonProjectAssignmentAssessment.RatingNotes = tbRatingNotes.Text;
            
            
            
            
            // TODO residencyCompetencyPersonProjectAssignmentAssessment.ResidentComments = tbResidentComments.Text;




            if ( !residencyCompetencyPersonProjectAssignmentAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyCompetencyPersonProjectAssignmentAssessmentService.Save( residencyCompetencyPersonProjectAssignmentAssessment, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyCompetencyPersonProjectAssignmentAssessmentId"] = residencyCompetencyPersonProjectAssignmentAssessment.Id.ToString();
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
        /// <param name="residencyCompetencyPersonProjectAssignmentId">The residency competency person project assignment id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyCompetencyPersonProjectAssignmentId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyCompetencyPersonProjectAssignmentAssessmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPersonProjectAssignmentAssessment residencyCompetencyPersonProjectAssignmentAssessment = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetencyPersonProjectAssignmentAssessment = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetencyPersonProjectAssignmentAssessment = new CompetencyPersonProjectAssignmentAssessment { Id = 0 };
                residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId = residencyCompetencyPersonProjectAssignmentId ?? 0;
                residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment 
                    = new ResidencyService<CompetencyPersonProjectAssignment>().Get( residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId );
            }

            hfResidencyCompetencyPersonProjectAssignmentAssessmentId.Value = residencyCompetencyPersonProjectAssignmentAssessment.Id.ToString();
            hfResidencyCompetencyPersonProjectAssignmentId.Value = residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId.ToString();

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
                ShowReadonlyDetails( residencyCompetencyPersonProjectAssignmentAssessment );
            }
            else
            {
                btnEdit.Visible = true;
                if ( residencyCompetencyPersonProjectAssignmentAssessment.Id > 0 )
                {
                    ShowReadonlyDetails( residencyCompetencyPersonProjectAssignmentAssessment );
                }
                else
                {
                    ShowEditDetails( residencyCompetencyPersonProjectAssignmentAssessment );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyCompetencyPersonProjectAssignmentAssessment">The residency competency person project assignment assessment.</param>
        private void ShowEditDetails( CompetencyPersonProjectAssignmentAssessment residencyCompetencyPersonProjectAssignmentAssessment )
        {
            if ( residencyCompetencyPersonProjectAssignmentAssessment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProjectAssignmentAssessment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProjectAssignmentAssessment.FriendlyTypeName );
            }

            SetEditMode( true );

            lblResident.Text = residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person.FullName;
            lblCompetency.Text = residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Competency.Name;
            lblProjectName.Text = residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name;
            
            if ( residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson != null )
            {
                lblAssessor.Text = residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson.FullName;
            }
            else
            {
                lblAssessor.Text = Rock.Constants.None.Text;
            }

            dtpAssessmentDateTime.SelectedDateTime = residencyCompetencyPersonProjectAssignmentAssessment.AssessmentDateTime;
            tbRating.Text = residencyCompetencyPersonProjectAssignmentAssessment.Rating.ToString();
            tbRatingNotes.Text = residencyCompetencyPersonProjectAssignmentAssessment.RatingNotes;
            tbResidentComments.Text = residencyCompetencyPersonProjectAssignmentAssessment.ResidentComments;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyCompetencyPersonProjectAssignmentAssessment">The residency competency person project assignment assessment.</param>
        private void ShowReadonlyDetails( CompetencyPersonProjectAssignmentAssessment residencyCompetencyPersonProjectAssignmentAssessment )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Resident", residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person.FullName );
            lblMainDetails.Text += string.Format( descriptionFormat, "Competency", residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Competency.Name );

            if ( residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson != null )
            {
                lblMainDetails.Text += string.Format( descriptionFormat, "Assessor", residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.AssessorPerson.FullName );
            }

            string residentProjectAssignmentPageGuid = this.GetAttributeValue( "ResidentProjectAssignmentPage" );
            string projectAssignmentHtml = residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name;
            if ( !string.IsNullOrWhiteSpace( residentProjectAssignmentPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentProjectAssignmentPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "residencyCompetencyPersonProjectAssignmentId", residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignmentId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                projectAssignmentHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyCompetencyPersonProjectAssignmentAssessment.CompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name);
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Project Assignment", projectAssignmentHtml );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}