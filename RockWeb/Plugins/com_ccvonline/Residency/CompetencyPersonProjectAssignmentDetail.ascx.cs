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
    public partial class CompetencyPersonProjectAssignmentDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "competencyPersonProjectAssignmentId" );
                string competencyPersonProjectId = PageParameter( "competencyPersonProjectId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( competencyPersonProjectId ) )
                    {
                        ShowDetail( "competencyPersonProjectAssignmentId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "competencyPersonProjectAssignmentId", int.Parse( itemId ), int.Parse( competencyPersonProjectId ) );
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

            if ( hfCompetencyPersonProjectAssignmentId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the CompetencyPersonProject Detail page, return to that
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
                ResidencyService<CompetencyPersonProjectAssignment> service = new ResidencyService<CompetencyPersonProjectAssignment>();
                CompetencyPersonProjectAssignment item = service.Get( hfCompetencyPersonProjectAssignmentId.ValueAsInt() );
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
            ResidencyService<CompetencyPersonProjectAssignment> service = new ResidencyService<CompetencyPersonProjectAssignment>();
            CompetencyPersonProjectAssignment item = service.Get( hfCompetencyPersonProjectAssignmentId.ValueAsInt() );
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
            CompetencyPersonProjectAssignment competencyPersonProjectAssignment;
            ResidencyService<CompetencyPersonProjectAssignment> competencyPersonProjectAssignmentService = new ResidencyService<CompetencyPersonProjectAssignment>();

            int competencyPersonProjectAssignmentId = int.Parse( hfCompetencyPersonProjectAssignmentId.Value );

            if ( competencyPersonProjectAssignmentId == 0 )
            {
                competencyPersonProjectAssignment = new CompetencyPersonProjectAssignment();
                competencyPersonProjectAssignmentService.Add( competencyPersonProjectAssignment, CurrentPersonId );
            }
            else
            {
                competencyPersonProjectAssignment = competencyPersonProjectAssignmentService.Get( competencyPersonProjectAssignmentId );
            }

            competencyPersonProjectAssignment.AssessorPersonId = ppAssessor.SelectedValue;
            competencyPersonProjectAssignment.CompletedDateTime = dtpCompletedDateTime.SelectedDateTime;
            competencyPersonProjectAssignment.CompetencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();

            if ( !competencyPersonProjectAssignment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                competencyPersonProjectAssignmentService.Save( competencyPersonProjectAssignment, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["competencyPersonProjectAssignmentId"] = competencyPersonProjectAssignment.Id.ToString();
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
            if ( itemKey != "competencyPersonProjectAssignmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPersonProjectAssignment competencyPersonProjectAssignment = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                competencyPersonProjectAssignment = new ResidencyService<CompetencyPersonProjectAssignment>().Get( itemKeyValue );
            }
            else
            {
                competencyPersonProjectAssignment = new CompetencyPersonProjectAssignment { Id = 0 };
                competencyPersonProjectAssignment.CompetencyPersonProjectId = competencyPersonProjectId ?? 0;
                competencyPersonProjectAssignment.CompetencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( competencyPersonProjectAssignment.CompetencyPersonProjectId );
            }

            hfCompetencyPersonProjectAssignmentId.Value = competencyPersonProjectAssignment.Id.ToString();
            hfCompetencyPersonProjectId.Value = competencyPersonProjectAssignment.CompetencyPersonProjectId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPersonProjectAssignment.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( competencyPersonProjectAssignment );
            }
            else
            {
                btnEdit.Visible = true;
                if ( competencyPersonProjectAssignment.Id > 0 )
                {
                    ShowReadonlyDetails( competencyPersonProjectAssignment );
                }
                else
                {
                    ShowEditDetails( competencyPersonProjectAssignment );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="competencyPersonProjectAssignment">The competency person project assignment.</param>
        private void ShowEditDetails( CompetencyPersonProjectAssignment competencyPersonProjectAssignment )
        {
            if ( competencyPersonProjectAssignment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProjectAssignment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProjectAssignment.FriendlyTypeName );
            }

            SetEditMode( true );

            lblResidentPersonName.Text = competencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person.FullName;
            lblProject.Text = string.Format( "{0} - {1}", competencyPersonProjectAssignment.CompetencyPersonProject.Project.Name, competencyPersonProjectAssignment.CompetencyPersonProject.Project.Description );
            ppAssessor.SetValue( competencyPersonProjectAssignment.AssessorPerson );
            dtpCompletedDateTime.SelectedDateTime = competencyPersonProjectAssignment.CompletedDateTime;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competencyPersonProjectAssignment">The competency person project assignment.</param>
        private void ShowReadonlyDetails( CompetencyPersonProjectAssignment competencyPersonProjectAssignment )
        {
            SetEditMode( false );

            string residentProjectAssignmentPageGuid = this.GetAttributeValue( "ResidentProjectPage" );
            string projectHtml = string.Format("{0} - {1}", competencyPersonProjectAssignment.CompetencyPersonProject.Project.Name, competencyPersonProjectAssignment.CompetencyPersonProject.Project.Description);
            if ( !string.IsNullOrWhiteSpace( residentProjectAssignmentPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentProjectAssignmentPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectId", competencyPersonProjectAssignment.CompetencyPersonProjectId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                projectHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, projectHtml );
            }

            lblMainDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person )
                .Add( "Project", projectHtml )
                .StartSecondColumn()
                .Add( "Assessor", competencyPersonProjectAssignment.AssessorPerson )
                .Add( "Completed", competencyPersonProjectAssignment.CompletedDateTime )
                .Html;
        }

        #endregion
    }
}