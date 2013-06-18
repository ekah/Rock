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
                string itemId = PageParameter( "residencyCompetencyPersonProjectAssignmentId" );
                string residencyCompetencyPersonProjectId = PageParameter( "residencyCompetencyPersonProjectId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( residencyCompetencyPersonProjectId ) )
                    {
                        ShowDetail( "residencyCompetencyPersonProjectAssignmentId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyCompetencyPersonProjectAssignmentId", int.Parse( itemId ), int.Parse( residencyCompetencyPersonProjectId ) );
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

            if ( hfResidencyCompetencyPersonProjectAssignmentId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the ResidencyCompetencyPersonProject Detail page, return to that
                string residencyCompetencyPersonProjectId = PageParameter( "residencyCompetencyPersonProjectId" );
                if ( !string.IsNullOrWhiteSpace( residencyCompetencyPersonProjectId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["residencyCompetencyPersonProjectId"] = residencyCompetencyPersonProjectId;
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
                CompetencyPersonProjectAssignment item = service.Get( hfResidencyCompetencyPersonProjectAssignmentId.ValueAsInt() );
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
            CompetencyPersonProjectAssignment item = service.Get( hfResidencyCompetencyPersonProjectAssignmentId.ValueAsInt() );
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
            CompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment;
            ResidencyService<CompetencyPersonProjectAssignment> residencyCompetencyPersonProjectAssignmentService = new ResidencyService<CompetencyPersonProjectAssignment>();

            int ResidencyCompetencyPersonProjectAssignmentId = int.Parse( hfResidencyCompetencyPersonProjectAssignmentId.Value );

            if ( ResidencyCompetencyPersonProjectAssignmentId == 0 )
            {
                residencyCompetencyPersonProjectAssignment = new CompetencyPersonProjectAssignment();
                residencyCompetencyPersonProjectAssignmentService.Add( residencyCompetencyPersonProjectAssignment, CurrentPersonId );
            }
            else
            {
                residencyCompetencyPersonProjectAssignment = residencyCompetencyPersonProjectAssignmentService.Get( ResidencyCompetencyPersonProjectAssignmentId );
            }

            residencyCompetencyPersonProjectAssignment.AssessorPersonId = ppAssessor.SelectedValue;
            residencyCompetencyPersonProjectAssignment.CompletedDateTime = dtpCompletedDateTime.SelectedDateTime;
            residencyCompetencyPersonProjectAssignment.CompetencyPersonProjectId = hfResidencyCompetencyPersonProjectId.ValueAsInt();


            if ( !residencyCompetencyPersonProjectAssignment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyCompetencyPersonProjectAssignmentService.Save( residencyCompetencyPersonProjectAssignment, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyCompetencyPersonProjectAssignmentId"] = residencyCompetencyPersonProjectAssignment.Id.ToString();
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
        /// <param name="residencyCompetencyPersonProjectId">The residency competency person project id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyCompetencyPersonProjectId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyCompetencyPersonProjectAssignmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetencyPersonProjectAssignment = new ResidencyService<CompetencyPersonProjectAssignment>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetencyPersonProjectAssignment = new CompetencyPersonProjectAssignment { Id = 0 };
                residencyCompetencyPersonProjectAssignment.CompetencyPersonProjectId = residencyCompetencyPersonProjectId ?? 0;
                residencyCompetencyPersonProjectAssignment.CompetencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( residencyCompetencyPersonProjectAssignment.CompetencyPersonProjectId );
            }

            hfResidencyCompetencyPersonProjectAssignmentId.Value = residencyCompetencyPersonProjectAssignment.Id.ToString();
            hfResidencyCompetencyPersonProjectId.Value = residencyCompetencyPersonProjectAssignment.CompetencyPersonProjectId.ToString();

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
                ShowReadonlyDetails( residencyCompetencyPersonProjectAssignment );
            }
            else
            {
                btnEdit.Visible = true;
                if ( residencyCompetencyPersonProjectAssignment.Id > 0 )
                {
                    ShowReadonlyDetails( residencyCompetencyPersonProjectAssignment );
                }
                else
                {
                    ShowEditDetails( residencyCompetencyPersonProjectAssignment );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyCompetencyPersonProjectAssignment">The residency project.</param>
        private void ShowEditDetails( CompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment )
        {
            if ( residencyCompetencyPersonProjectAssignment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProjectAssignment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProjectAssignment.FriendlyTypeName );
            }

            SetEditMode( true );

            lblResidentPersonName.Text = residencyCompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person.FullName;
            lblProject.Text = residencyCompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name;
            ppAssessor.SetValue( residencyCompetencyPersonProjectAssignment.AssessorPerson );
            dtpCompletedDateTime.SelectedDateTime = residencyCompetencyPersonProjectAssignment.CompletedDateTime;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyCompetencyPersonProjectAssignment">The residency project.</param>
        private void ShowReadonlyDetails( CompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Resident", residencyCompetencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person.FullName );
            lblMainDetails.Text += string.Format( descriptionFormat, "Project", residencyCompetencyPersonProjectAssignment.CompetencyPersonProject.Project.Name );

            if ( residencyCompetencyPersonProjectAssignment.AssessorPerson != null )
            {
                lblMainDetails.Text += string.Format( descriptionFormat, "Assessor", residencyCompetencyPersonProjectAssignment.AssessorPerson );
            }

            if ( residencyCompetencyPersonProjectAssignment.CompletedDateTime != null )
            {
                lblMainDetails.Text += string.Format( descriptionFormat, "Completed", residencyCompetencyPersonProjectAssignment.CompletedDateTime.Value.ToString( "g" ) );
            }

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}