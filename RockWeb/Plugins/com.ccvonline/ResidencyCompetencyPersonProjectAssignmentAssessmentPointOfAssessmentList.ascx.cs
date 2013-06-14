using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace com.ccvonline.Blocks
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    public partial class ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList : RockBlock, IDimmableBlock
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
            gList.DataKeyNames = new string[] { "ResidencyProjectPointOfAssessmentId", "ResidencyCompetencyPersonProjectAssignmentAssessmentId" };
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
                int? residencyCompetencyPersonProjectAssignmentAssessmentId = this.PageParameter( "residencyCompetencyPersonProjectAssignmentAssessmentId" ).AsInteger();
                hfResidencyCompetencyPersonProjectAssignmentAssessmentId.Value = residencyCompetencyPersonProjectAssignmentAssessmentId.ToString();
                BindGrid();
            }
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
            gList_ShowEdit( (int)e.RowKeyValues["ResidencyProjectPointOfAssessmentId"], (int)e.RowKeyValues["ResidencyCompetencyPersonProjectAssignmentAssessmentId"] );
        }

        /// <summary>
        /// Gs the list_ show edit.
        /// </summary>
        /// <param name="residencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentId">The residency competency person project assignment assessment point of assessment id.</param>
        protected void gList_ShowEdit( int residencyProjectPointOfAssessmentId, int residencyCompetencyPersonProjectAssignmentAssessmentId )
        {
            NavigateToDetailPage( "residencyProjectPointOfAssessmentId", residencyProjectPointOfAssessmentId, "residencyCompetencyPersonProjectAssignmentAssessmentId", residencyCompetencyPersonProjectAssignmentAssessmentId );
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
            SortProperty sortProperty = gList.SortProperty;
            
            ResidencyCompetencyPersonProjectAssignmentAssessment residencyCompetencyPersonProjectAssignmentAssessment 
                = new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessment>().Get(hfResidencyCompetencyPersonProjectAssignmentAssessmentId.ValueAsInt());

            List<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment> personPointOfAssessmentList = new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Queryable()
                .Where(a => a.ResidencyCompetencyPersonProjectAssignmentAssessmentId.Equals(residencyCompetencyPersonProjectAssignmentAssessment.Id)).ToList();

            List<ResidencyProjectPointOfAssessment> residencyProjectPointOfAssessmentList = new ResidencyService<ResidencyProjectPointOfAssessment>().Queryable()
                .Where(a => a.ResidencyProjectId.Equals(residencyCompetencyPersonProjectAssignmentAssessment.ResidencyCompetencyPersonProjectAssignment.ResidencyCompetencyPersonProject.ResidencyProjectId)).ToList();

            var joinedItems = from residencyProjectPointOfAssessment in residencyProjectPointOfAssessmentList
                          join personPointOfAssessment in personPointOfAssessmentList
                          on residencyProjectPointOfAssessment.Id equals personPointOfAssessment.ResidencyProjectPointOfAssessmentId into groupJoin
                          from qryResult in groupJoin.DefaultIfEmpty()
                          select new
                          {
                              // note: two key fields, since we want to show all the Points of Assessment for this Project, if the person hasn't had a rating on it yet
                              ResidencyProjectPointOfAssessmentId = residencyProjectPointOfAssessment.Id,
                              ResidencyCompetencyPersonProjectAssignmentAssessmentId = residencyCompetencyPersonProjectAssignmentAssessment.Id,
                              ResidencyProjectPointOfAssessment = residencyProjectPointOfAssessment,
                              ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment = personPointOfAssessmentList.FirstOrDefault(a => a.ResidencyProjectPointOfAssessmentId.Equals(residencyProjectPointOfAssessment.Id))
                          };

            if ( sortProperty != null )
            {
                gList.DataSource = joinedItems.AsQueryable().Sort( sortProperty ).ToList();
            }
            else
            {
                gList.DataSource = joinedItems.OrderBy( s => s.ResidencyProjectPointOfAssessment.AssessmentOrder ).ToList();
            }

            gList.DataBind();
        }

        #endregion

        #region IDimmableBlock

        /// <summary>
        /// Sets the dimmed.
        /// </summary>
        /// <param name="dimmed">if set to <c>true</c> [dimmed].</param>
        public void SetDimmed( bool dimmed )
        {
            gList.Enabled = !dimmed;
        }

        #endregion
    }
}