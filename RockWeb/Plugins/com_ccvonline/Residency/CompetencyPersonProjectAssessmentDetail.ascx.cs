//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
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
    [LinkedPage( "Resident Project Page" )]
    public partial class CompetencyPersonProjectAssessmentDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "competencyPersonProjectAssessmentId" );
                string competencyPersonProjectId = PageParameter( "competencyPersonProjectId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( competencyPersonProjectId ) )
                    {
                        ShowDetail( "competencyPersonProjectAssessmentId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "competencyPersonProjectAssessmentId", int.Parse( itemId ), int.Parse( competencyPersonProjectId ) );
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

            if ( hfCompetencyPersonProjectAssessmentId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the Project Detail page, return to that
                string competencyPersonProjectId = PageParameter( "competencyPersonProjectId" );
                if ( !string.IsNullOrWhiteSpace( competencyPersonProjectId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["competencyPersonProjectId"] = competencyPersonProjectId;
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
                ResidencyService<CompetencyPersonProjectAssessment> service = new ResidencyService<CompetencyPersonProjectAssessment>();
                CompetencyPersonProjectAssessment item = service.Get( hfCompetencyPersonProjectAssessmentId.ValueAsInt() );
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
            ResidencyService<CompetencyPersonProjectAssessment> service = new ResidencyService<CompetencyPersonProjectAssessment>();
            CompetencyPersonProjectAssessment item = service.Get( hfCompetencyPersonProjectAssessmentId.ValueAsInt() );
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
            CompetencyPersonProjectAssessment competencyPersonProjectAssessment;
            ResidencyService<CompetencyPersonProjectAssessment> competencyPersonProjectAssessmentService = new ResidencyService<CompetencyPersonProjectAssessment>();

            int competencyPersonProjectAssessmentId = int.Parse( hfCompetencyPersonProjectAssessmentId.Value );

            if ( competencyPersonProjectAssessmentId == 0 )
            {
                competencyPersonProjectAssessment = new CompetencyPersonProjectAssessment();
                competencyPersonProjectAssessmentService.Add( competencyPersonProjectAssessment, CurrentPersonId );
            }
            else
            {
                competencyPersonProjectAssessment = competencyPersonProjectAssessmentService.Get( competencyPersonProjectAssessmentId );
            }

            competencyPersonProjectAssessment.CompetencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();
            competencyPersonProjectAssessment.AssessorPersonId = ppAssessor.PersonId;
            competencyPersonProjectAssessment.AssessmentDateTime = dtpAssessmentDateTime.SelectedDateTime;
            competencyPersonProjectAssessment.RatingNotes = tbRatingNotes.Text;
            competencyPersonProjectAssessment.ResidentComments = tbResidentComments.Text;

            if ( !competencyPersonProjectAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                competencyPersonProjectAssessmentService.Save( competencyPersonProjectAssessment, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["competencyPersonProjectAssessmentId"] = competencyPersonProjectAssessment.Id.ToString();
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
        /// <param name="competencyPersonProjectId">The competency person project id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? competencyPersonProjectId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonProjectAssessmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPersonProjectAssessment competencyPersonProjectAssessment = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                competencyPersonProjectAssessment = new ResidencyService<CompetencyPersonProjectAssessment>().Get( itemKeyValue );
            }
            else
            {
                competencyPersonProjectAssessment = new CompetencyPersonProjectAssessment { Id = 0 };
                competencyPersonProjectAssessment.CompetencyPersonProjectId = competencyPersonProjectId ?? 0;
                competencyPersonProjectAssessment.CompetencyPersonProject
                    = new ResidencyService<CompetencyPersonProject>().Get( competencyPersonProjectAssessment.CompetencyPersonProjectId );
            }

            hfCompetencyPersonProjectAssessmentId.Value = competencyPersonProjectAssessment.Id.ToString();
            hfCompetencyPersonProjectId.Value = competencyPersonProjectAssessment.CompetencyPersonProjectId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPersonProjectAssessment.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( competencyPersonProjectAssessment );
            }
            else
            {
                btnEdit.Visible = true;
                if ( competencyPersonProjectAssessment.Id > 0 )
                {
                    ShowReadonlyDetails( competencyPersonProjectAssessment );
                }
                else
                {
                    ShowEditDetails( competencyPersonProjectAssessment );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="competencyPersonProjectAssessment">The competency person project assessment.</param>
        private void ShowEditDetails( CompetencyPersonProjectAssessment competencyPersonProjectAssessment )
        {
            if ( competencyPersonProjectAssessment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( "Project Assessment" );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( "Project Assessment" );
            }

            SetEditMode( true );

            lblEditDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProjectAssessment.CompetencyPersonProject.CompetencyPerson.Person )
                .Add( "Project", string.Format( "{0} - {1}", competencyPersonProjectAssessment.CompetencyPersonProject.Project.Name, competencyPersonProjectAssessment.CompetencyPersonProject.Project.Description ) )
                .Add( "Competency", competencyPersonProjectAssessment.CompetencyPersonProject.CompetencyPerson.Competency.Name )
                .StartSecondColumn()
                .Html;

            ppAssessor.SetValue( competencyPersonProjectAssessment.AssessorPerson );
            dtpAssessmentDateTime.SelectedDateTime = competencyPersonProjectAssessment.AssessmentDateTime;
            lblOverallRating.Text = competencyPersonProjectAssessment.OverallRating.ToString();
            tbRatingNotes.Text = competencyPersonProjectAssessment.RatingNotes;
            tbResidentComments.Text = competencyPersonProjectAssessment.ResidentComments;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competencyPersonProjectAssessment">The competency person project assessment.</param>
        private void ShowReadonlyDetails( CompetencyPersonProjectAssessment competencyPersonProjectAssessment )
        {
            SetEditMode( false );

            string residentProjectPageGuid = this.GetAttributeValue( "ResidentProjectPage" );
            string projectHtml = string.Format( "{0} - {1}", competencyPersonProjectAssessment.CompetencyPersonProject.Project.Name, competencyPersonProjectAssessment.CompetencyPersonProject.Project.Description );
            if ( !string.IsNullOrWhiteSpace( residentProjectPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentProjectPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectId", competencyPersonProjectAssessment.CompetencyPersonProjectId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                projectHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, projectHtml );
            }

            lblMainDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProjectAssessment.CompetencyPersonProject.CompetencyPerson.Person )
                .Add( "Competency", competencyPersonProjectAssessment.CompetencyPersonProject.Project.Competency.Name )
                .Add( "Project", projectHtml )
                .StartSecondColumn()
                .Add( "Assessor", competencyPersonProjectAssessment.AssessorPerson )
                .Add( "Assessment Date/Time", competencyPersonProjectAssessment.AssessmentDateTime )
                .Add( "Rating", competencyPersonProjectAssessment.OverallRating.ToString() )
                .Html;
        }

        #endregion
    }
}