﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResidentProjectAssessmentDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // NOTE:  this is special case of where we need two key fields, and no add or delete
            gList.DataKeyNames = new string[] { "ProjectPointOfAssessmentId", "CompetencyPersonProjectAssessmentId" };
            gList.Actions.ShowAdd = false;
            gList.IsDeleteEnabled = false;
            gList.GridRebind += gList_GridRebind;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                int? competencyPersonProjectAssessmentId = this.PageParameter( "competencyPersonProjectAssessmentId" ).AsInteger();
                if ( competencyPersonProjectAssessmentId.HasValue )
                {
                    ShowDetail( "competencyPersonProjectAssessmentId", competencyPersonProjectAssessmentId.Value );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns></returns>
        public override List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();

            int? competencyPersonProjectAssessmentId = this.PageParameter( pageReference, "competencyPersonProjectAssessmentId" ).AsInteger();
            if ( competencyPersonProjectAssessmentId != null )
            {
                breadCrumbs.Add( new BreadCrumb( "Assessment", pageReference ) );
            }
            else
            {
                // don't show a breadcrumb if we don't have a pageparam to work with
            }

            return breadCrumbs;
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            SetEditMode( false );

            // Cancelling on Edit.  Return to Details
            ResidencyService<CompetencyPersonProjectAssessment> service = new ResidencyService<CompetencyPersonProjectAssessment>();
            CompetencyPersonProjectAssessment item = service.Get( hfCompetencyPersonProjectAssessmentId.ValueAsInt() );
            ShowReadonlyDetails( item );
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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
            pnlEditComments.Visible = editable;
            pnlViewComments.Visible = !editable;
            DimOtherBlocks( editable );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            ResidencyService<CompetencyPersonProjectAssessment> service = new ResidencyService<CompetencyPersonProjectAssessment>();

            int competencyPersonProjectAssessmentId = hfCompetencyPersonProjectAssessmentId.ValueAsInt();
            CompetencyPersonProjectAssessment competencyPersonProjectAssessment = service.Get( competencyPersonProjectAssessmentId );

            competencyPersonProjectAssessment.ResidentComments = tbResidentComments.Text;

            if ( !competencyPersonProjectAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                service.Save( competencyPersonProjectAssessment, CurrentPersonId );
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
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonProjectAssessmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            CompetencyPersonProjectAssessment competencyPersonProjectAssessment = new ResidencyService<CompetencyPersonProjectAssessment>().Get( itemKeyValue );
            
            if ( competencyPersonProjectAssessment.CompetencyPersonProject.CompetencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectId", competencyPersonProjectAssessment.CompetencyPersonProjectId.ToString() );
                NavigateToParentPage( queryString );
                return;
            }

            hfCompetencyPersonProjectAssessmentId.Value = competencyPersonProjectAssessment.Id.ToString();

            ShowReadonlyDetails( competencyPersonProjectAssessment );
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="competencyPersonProjectAssessment">The competency person project assessment.</param>
        private void ShowEditDetails( CompetencyPersonProjectAssessment competencyPersonProjectAssessment )
        {
            SetEditMode( true );
            
            ShowAssessmentDetails( competencyPersonProjectAssessment );
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competencyPersonProjectAssessment">The competency person project assessment.</param>
        private void ShowReadonlyDetails( CompetencyPersonProjectAssessment competencyPersonProjectAssessment )
        {
            SetEditMode( false );

            ShowAssessmentDetails( competencyPersonProjectAssessment );
        }

        /// <summary>
        /// Shows the assessment details.
        /// </summary>
        /// <param name="competencyPersonProjectAssessment">The competency person project assessment.</param>
        private void ShowAssessmentDetails( CompetencyPersonProjectAssessment competencyPersonProjectAssessment )
        {
            string projectText = string.Format( "{0} - {1}", competencyPersonProjectAssessment.CompetencyPersonProject.Project.Name, competencyPersonProjectAssessment.CompetencyPersonProject.Project.Description );

            lblProjectDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProjectAssessment.CompetencyPersonProject.CompetencyPerson.Person )
                .Add( "Competency", competencyPersonProjectAssessment.CompetencyPersonProject.Project.Competency.Name )
                .Add( "Project", projectText )
                .StartSecondColumn()
                .Add( "Assessor", competencyPersonProjectAssessment.AssessorPerson )
                .Add( "Assessment Date/Time", competencyPersonProjectAssessment.AssessmentDateTime )
                .Add( "Rating", competencyPersonProjectAssessment.OverallRating.ToString() )
                .Add( "Rating Notes", competencyPersonProjectAssessment.RatingNotes)
                .Html;

            tbResidentComments.Text = competencyPersonProjectAssessment.ResidentComments;
            lblResidentComments.Text = new DescriptionList()
                .Add( "Resident Comments", (competencyPersonProjectAssessment.ResidentComments ?? string.Empty).Replace("\n", "<br />") )
                .Html;

            BindGrid();
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Handles the Edit event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( "projectPointOfAssessmentId", (int)e.RowKeyValues["ProjectPointOfAssessmentId"], "competencyPersonProjectAssessmentId", (int)e.RowKeyValues["CompetencyPersonProjectAssessmentId"] );
        }

        /// <summary>
        /// Handles the GridRebind event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gList_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            int competencyPersonProjectAssessmentId = hfCompetencyPersonProjectAssessmentId.ValueAsInt();

            CompetencyPersonProjectAssessment competencyPersonProjectAssessment
                = new ResidencyService<CompetencyPersonProjectAssessment>().Get( competencyPersonProjectAssessmentId );

            List<CompetencyPersonProjectAssessmentPointOfAssessment> personPointOfAssessmentList = new ResidencyService<CompetencyPersonProjectAssessmentPointOfAssessment>().Queryable()
                .Where( a => a.CompetencyPersonProjectAssessmentId.Equals( competencyPersonProjectAssessmentId ) ).ToList();

            List<ProjectPointOfAssessment> projectPointOfAssessmentList;
            if ( competencyPersonProjectAssessment != null )
            {
                projectPointOfAssessmentList = new ResidencyService<ProjectPointOfAssessment>().Queryable()
                    .Where( a => a.ProjectId.Equals( competencyPersonProjectAssessment.CompetencyPersonProject.ProjectId ) ).ToList();
            }
            else
            {
                projectPointOfAssessmentList = new List<ProjectPointOfAssessment>();
            }

            var joinedItems = from projectPointOfAssessment in projectPointOfAssessmentList
                              join personPointOfAssessment in personPointOfAssessmentList
                              on projectPointOfAssessment.Id equals personPointOfAssessment.ProjectPointOfAssessmentId into groupJoin
                              from qryResult in groupJoin.DefaultIfEmpty()
                              select new
                              {
                                  // note: two key fields, since we want to show all the Points of Assessment for this Project, even if the person hasn't had a rating on it yet
                                  ProjectPointOfAssessmentId = projectPointOfAssessment.Id,
                                  CompetencyPersonProjectAssessmentId = competencyPersonProjectAssessmentId,
                                  ProjectPointOfAssessment = projectPointOfAssessment,
                                  CompetencyPersonProjectAssessmentPointOfAssessment = personPointOfAssessmentList.FirstOrDefault( a => a.ProjectPointOfAssessmentId.Equals( projectPointOfAssessment.Id ) )
                              };

            gList.DataSource = joinedItems.OrderBy( s => s.ProjectPointOfAssessment.AssessmentOrder ).ToList();

            gList.DataBind();
        }

        #endregion
    }
}