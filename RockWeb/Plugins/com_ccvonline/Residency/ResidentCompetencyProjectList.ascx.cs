using System;
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
    public partial class ResidentCompetencyProjectList : RockBlock, IDimmableBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gProjectList.DataKeyNames = new string[] { "Id" };
            gProjectList.Actions.ShowAdd = false;
            gProjectList.GridRebind += gList_GridRebind;
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
                int? competencyPersonId = this.PageParameter( "competencyPersonId" ).AsInteger();
                hfCompetencyPersonId.Value = competencyPersonId.ToString();
                BindGrid();
            }
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Binds the project list grid.
        /// </summary>
        protected void BindGrid()
        {
            var competencyPersonProjectService = new ResidencyService<CompetencyPersonProject>();
            int competencyPersonId = hfCompetencyPersonId.ValueAsInt();
            SortProperty sortProperty = gProjectList.SortProperty;
            var qry = competencyPersonProjectService.Queryable();

            qry = qry.Where( a => a.CompetencyPersonId.Equals( competencyPersonId ) );

            if ( sortProperty != null )
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderBy( s => s.Project.Name ).ThenBy( s => s.Project.Description );
            }

            var resultList = qry.ToList().Select( a => new
            {
                Id = a.Id,
                Name = a.Project.Name,
                Description = a.Project.Description,
                MinAssessmentCount = a.MinAssessmentCount ?? a.Project.MinAssessmentCountDefault,
                AssessmentCompleted = a.CompetencyPersonProjectAssessments.Where( b => b.AssessmentDateTime != null ).Count(),
                AssessmentRemaining = Math.Max( a.MinAssessmentCount ?? a.Project.MinAssessmentCountDefault - a.CompetencyPersonProjectAssessments.Where( b => b.AssessmentDateTime != null ).Count() ?? 0, 0 )
            } ).ToList();

            gProjectList.DataSource = resultList;
            gProjectList.DataBind();
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

        /// <summary>
        /// Handles the RowSelected event of the gProjectList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.Controls.RowEventArgs"/> instance containing the event data.</param>
        protected void gProjectList_RowSelected( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
            NavigateToDetailPage( "competencyPersonProjectId", e.RowKeyId );
        }

        #endregion

        #region IDimmableBlock

        /// <summary>
        /// Sets the dimmed.
        /// </summary>
        /// <param name="dimmed">if set to <c>true</c> [dimmed].</param>
        public void SetDimmed( bool dimmed )
        {
            gProjectList.Enabled = !dimmed;
        }

        #endregion
    }
}