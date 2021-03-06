﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    public partial class CompetencyPersonProjectAssessmentPointOfAssessmentList : RockBlock, IDimmableBlock
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
                hfCompetencyPersonProjectAssessmentId.Value = competencyPersonProjectAssessmentId.ToString();
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
            gList_ShowEdit( (int)e.RowKeyValues["ProjectPointOfAssessmentId"], (int)e.RowKeyValues["CompetencyPersonProjectAssessmentId"] );
        }

        /// <summary>
        /// Gs the list_ show edit.
        /// </summary>
        /// <param name="competencyPersonProjectAssessmentPointOfAssessmentId">The residency competency person project assessment point of assessment id.</param>
        protected void gList_ShowEdit( int projectPointOfAssessmentId, int competencyPersonProjectAssessmentId )
        {
            NavigateToDetailPage( "projectPointOfAssessmentId", projectPointOfAssessmentId, "competencyPersonProjectAssessmentId", competencyPersonProjectAssessmentId );
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

            int competencyPersonProjectAssessmentId = hfCompetencyPersonProjectAssessmentId.ValueAsInt();

            List<CompetencyPersonProjectAssessmentPointOfAssessment> personPointOfAssessmentList = new ResidencyService<CompetencyPersonProjectAssessmentPointOfAssessment>().Queryable()
                .Where( a => a.CompetencyPersonProjectAssessmentId.Equals( competencyPersonProjectAssessmentId ) ).ToList();

            CompetencyPersonProjectAssessment competencyPersonProjectAssessment
                = new ResidencyService<CompetencyPersonProjectAssessment>().Get( competencyPersonProjectAssessmentId );

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

            foreach ( var item in projectPointOfAssessmentList )
            {
                if ( item.PointOfAssessmentTypeValue != null )
                {
                    item.PointOfAssessmentTypeValue.LoadAttributes();
                }
            }

            var joinedItems = from projectPointOfAssessment in projectPointOfAssessmentList
                              join personPointOfAssessment in personPointOfAssessmentList
                              on projectPointOfAssessment.Id equals personPointOfAssessment.ProjectPointOfAssessmentId into groupJoin
                              from qryResult in groupJoin.DefaultIfEmpty()
                              select new
                              {
                                  // note: two key fields, since we want to show all the Points of Assessment for this Project, if the person hasn't had a rating on it yet
                                  ProjectPointOfAssessmentId = projectPointOfAssessment.Id,
                                  CompetencyPersonProjectAssessmentId = competencyPersonProjectAssessmentId,
                                  ProjectPointOfAssessmentColor = projectPointOfAssessment.PointOfAssessmentTypeValue != null ? projectPointOfAssessment.PointOfAssessmentTypeValue.GetAttributeValue( "Color" ) : string.Empty,
                                  ProjectPointOfAssessment = projectPointOfAssessment,
                                  CompetencyPersonProjectAssessmentPointOfAssessment = personPointOfAssessmentList.FirstOrDefault( a => a.ProjectPointOfAssessmentId.Equals( projectPointOfAssessment.Id ) )
                              };

            if ( sortProperty != null )
            {
                gList.DataSource = joinedItems.AsQueryable().Sort( sortProperty ).ToList();
            }
            else
            {
                gList.DataSource = joinedItems.OrderBy( s => s.ProjectPointOfAssessment.AssessmentOrder ).ToList();
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