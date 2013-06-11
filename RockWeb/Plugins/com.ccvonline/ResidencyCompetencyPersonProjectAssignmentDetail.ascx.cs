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
    public partial class ResidencyCompetencyPersonProjectAssignmentDetail : RockBlock, IDetailBlock
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
                ResidencyService<ResidencyCompetencyPersonProjectAssignment> service = new ResidencyService<ResidencyCompetencyPersonProjectAssignment>();
                ResidencyCompetencyPersonProjectAssignment item = service.Get( hfResidencyCompetencyPersonProjectAssignmentId.ValueAsInt() );
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
            ResidencyService<ResidencyCompetencyPersonProjectAssignment> service = new ResidencyService<ResidencyCompetencyPersonProjectAssignment>();
            ResidencyCompetencyPersonProjectAssignment item = service.Get( hfResidencyCompetencyPersonProjectAssignmentId.ValueAsInt() );
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
            ResidencyCompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment;
            ResidencyService<ResidencyCompetencyPersonProjectAssignment> residencyCompetencyPersonProjectAssignmentService = new ResidencyService<ResidencyCompetencyPersonProjectAssignment>();

            int ResidencyCompetencyPersonProjectAssignmentId = int.Parse( hfResidencyCompetencyPersonProjectAssignmentId.Value );

            if ( ResidencyCompetencyPersonProjectAssignmentId == 0 )
            {
                residencyCompetencyPersonProjectAssignment = new ResidencyCompetencyPersonProjectAssignment();
                residencyCompetencyPersonProjectAssignmentService.Add( residencyCompetencyPersonProjectAssignment, CurrentPersonId );
            }
            else
            {
                residencyCompetencyPersonProjectAssignment = residencyCompetencyPersonProjectAssignmentService.Get( ResidencyCompetencyPersonProjectAssignmentId );
            }

            //////////


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
            ResidencyCompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetencyPersonProjectAssignment = new ResidencyService<ResidencyCompetencyPersonProjectAssignment>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetencyPersonProjectAssignment = new ResidencyCompetencyPersonProjectAssignment { Id = 0 };
                residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProjectId = residencyCompetencyPersonProjectId ?? 0;
                residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProject = new ResidencyService<ResidencyCompetencyPersonProject>().Get( residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProjectId );
            }

            hfResidencyCompetencyPersonProjectAssignmentId.Value = residencyCompetencyPersonProjectAssignment.Id.ToString();
            hfResidencyCompetencyPersonProjectId.Value = residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProjectId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ResidencyCompetencyPersonProjectAssignment.FriendlyTypeName );
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
        private void ShowEditDetails( ResidencyCompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment )
        {
            if ( residencyCompetencyPersonProjectAssignment.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( ResidencyCompetencyPersonProjectAssignment.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( ResidencyCompetencyPersonProjectAssignment.FriendlyTypeName );
            }

            SetEditMode( true );

            lblResidentPersonName.Text = residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProject.ResidencyCompetencyPerson.Person.FullName;
            lblProject.Text = residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProject.ResidencyProject.Name;
            ppAssessor.SetValue( residencyCompetencyPersonProjectAssignment.AssessorPerson );
            dtpCompletedDateTime.SelectedDateTime = residencyCompetencyPersonProjectAssignment.CompletedDateTime;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyCompetencyPersonProjectAssignment">The residency project.</param>
        private void ShowReadonlyDetails( ResidencyCompetencyPersonProjectAssignment residencyCompetencyPersonProjectAssignment )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Resident", residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProject.ResidencyCompetencyPerson.Person.FullName );
            lblMainDetails.Text += string.Format( descriptionFormat, "Project", residencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProject.ResidencyProject.Name );

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