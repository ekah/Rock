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

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResidentProjectPointOfAssessmentList : RockBlock, IDimmableBlock
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
            var competencyPersonProjectService = new ResidencyService<CompetencyPersonProject>();
            int competencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();
            CompetencyPersonProject competencyPersonProject = competencyPersonProjectService.Get( competencyPersonProjectId );

            if ( competencyPersonProject.CompetencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["competencyPersonId"] = competencyPersonProject.CompetencyPersonId.ToString();
                NavigateToParentPage( qryString );
                return;
            }

            if ( competencyPersonProject != null )
            {
                gList.DataSource = competencyPersonProject.Project.ProjectPointOfAssessments
                    .OrderBy( s => s.AssessmentOrder ).ToList();
                gList.DataBind();
            }
            else
            {
                NavigateToParentPage();
                return;
            }
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