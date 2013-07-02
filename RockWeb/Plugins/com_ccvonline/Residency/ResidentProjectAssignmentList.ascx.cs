using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public partial class ResidentProjectAssignmentList : RockBlock, IDimmableBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gList.DataKeyNames = new string[] { "id" };
            gList.Actions.ShowAdd = false;
            gList.GridRebind += gList_GridRebind;

            gList.Actions.ShowAdd = false;
            gList.IsDeleteEnabled = false;

            Dictionary<string, BoundField> boundFields = gList.Columns.OfType<BoundField>().ToDictionary( a => a.DataField );
            boundFields["AssessorPerson.FullName"].NullDisplayText = Rock.Constants.None.TextHtml;
            boundFields["CompletedDateTime"].NullDisplayText = "not completed";
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
                int? competencyPersonProjectId = this.PageParameter( "competencyPersonProjectId" ).AsInteger();
                hfCompetencyPersonProjectId.Value = competencyPersonProjectId.ToString();
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
            NavigateToDetailPage( "competencyPersonProjectAssignmentId", e.RowKeyId );
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
            var competencyPersonProjectAssignmentService = new ResidencyService<CompetencyPersonProjectAssignment>();
            int competencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();
            SortProperty sortProperty = gList.SortProperty;
            var assignmentsQry = competencyPersonProjectAssignmentService.Queryable();

            assignmentsQry = assignmentsQry.Where( a => a.CompetencyPersonProjectId.Equals( competencyPersonProjectId ) );
            List<CompetencyPersonProjectAssignment> competencyPersonProjectAssignmentList = assignmentsQry.ToList();

            var competencyPersonProjectAssignmentAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>();
            var assessmentsQry = competencyPersonProjectAssignmentAssessmentService.Queryable().GroupBy(a => a.CompetencyPersonProjectAssignmentId);

            var joinQry = from assignment in competencyPersonProjectAssignmentList
                          join assessments in assessmentsQry on assignment.Id
                          equals assessments.Key into groupJoin
                          from qryResult in groupJoin.DefaultIfEmpty()
                          select new
                          {
                              assignment.Id,
                              assignment.AssessorPerson,
                              assignment.CompletedDateTime,
                              AssessmentCount = qryResult != null ? qryResult.Count() : 0,
                              AssessmentDateTime = qryResult != null ? qryResult.Max( a => a.AssessmentDateTime ) : null
                          };

            if ( sortProperty != null )
            {
                joinQry = joinQry.AsQueryable().Sort( sortProperty );
            }
            else
            {
                joinQry = joinQry.OrderBy( s => s.CompletedDateTime ).ThenBy( s => s.AssessorPerson );
            }

            gList.DataSource = joinQry.ToList();
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