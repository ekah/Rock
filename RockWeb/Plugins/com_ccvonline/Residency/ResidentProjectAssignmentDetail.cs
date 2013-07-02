using System;
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
    [DetailPage]
    [LinkedPage("Resident Project Page")]
    public partial class ResidentProjectAssignmentDetail : RockBlock, IDimmableBlock
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
            gList.IsDeleteEnabled = false;
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
                int? competencyPersonProjectAssignmentId = this.PageParameter( "competencyPersonProjectAssignmentId" ).AsInteger();
                hfCompetencyPersonProjectAssignmentId.Value = competencyPersonProjectAssignmentId.ToString();
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
            NavigateToDetailPage( "competencyPersonProjectAssignmentAssessmentId", e.RowKeyId );
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
            int competencyPersonProjectAssignmentId = hfCompetencyPersonProjectAssignmentId.ValueAsInt();
            var competencyPersonProjectAssignment = new ResidencyService<CompetencyPersonProjectAssignment>().Get( competencyPersonProjectAssignmentId );

            if ( competencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectId", competencyPersonProjectAssignment.CompetencyPersonProjectId.ToString() );
                NavigateToParentPage( queryString );
                return;
            }

            string residentProjectAssignmentPageGuid = this.GetAttributeValue( "ResidentProjectPage" );
            string projectHtml = string.Format( "{0} - {1}", competencyPersonProjectAssignment.CompetencyPersonProject.Project.Name, competencyPersonProjectAssignment.CompetencyPersonProject.Project.Description );
            if ( !string.IsNullOrWhiteSpace( residentProjectAssignmentPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentProjectAssignmentPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonProjectId", competencyPersonProjectAssignment.CompetencyPersonProjectId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                projectHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, projectHtml );
            }

            lblProjectAssignmentDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProjectAssignment.CompetencyPersonProject.CompetencyPerson.Person )
                .Add( "Project", projectHtml )
                .StartSecondColumn()
                .Add( "Assessor", competencyPersonProjectAssignment.AssessorPerson )
                .Add( "Completed", competencyPersonProjectAssignment.CompletedDateTime )
                .Html;

            var competencyPersonProjectAssignmentAssessmentService = new ResidencyService<CompetencyPersonProjectAssignmentAssessment>();

            SortProperty sortProperty = gList.SortProperty;
            var qry = competencyPersonProjectAssignmentAssessmentService.Queryable();

            qry = qry.Where( a => a.CompetencyPersonProjectAssignmentId.Equals( competencyPersonProjectAssignmentId ) );

            if ( sortProperty != null )
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderByDescending( s => s.AssessmentDateTime );
            }

            gList.DataSource = qry.ToList();
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