//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;
using System.Web.UI.WebControls;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    public partial class ResidentCompetencyDetail : RockBlock, IDetailBlock
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
                string competencyPersonId = PageParameter( "competencyPersonId" );
                if ( !string.IsNullOrWhiteSpace( competencyPersonId ) )
                {
                    ShowDetail( "competencyPersonId", int.Parse( competencyPersonId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            hfCompetencyPersonId.Value = itemKeyValue.ToString();

            CompetencyPerson competencyPerson = new ResidencyService<CompetencyPerson>().Get(hfCompetencyPersonId.ValueAsInt());

            if ( competencyPerson.PersonId != CurrentPersonId )
            {
                // somebody besides the Resident is logged in
                NavigateToParentPage();
                return;
            }

            lblCompetencyName.Text = competencyPerson.Competency.Name;
            lblFacilitator.Text = competencyPerson.Competency.FacilitatorPerson != null ? competencyPerson.Competency.FacilitatorPerson.FullName : Rock.Constants.None.TextHtml;
            lblDescription.Text = !string.IsNullOrWhiteSpace( competencyPerson.Competency.Description ) ? competencyPerson.Competency.Description : Rock.Constants.None.TextHtml;
            lblGoals.Text = (competencyPerson.Competency.Goals ?? string.Empty).Replace( "\n", "<br>" );

            gProjectList.DataKeyNames = new string[] { "Id" };
            gProjectList.Actions.ShowAdd = false;
            gProjectList.GridRebind += gProjectList_GridRebind;

            lbProjects_Click( lbProjects, null );
        }

        /// <summary>
        /// Handles the Click event of the PillLabel control.
        /// </summary>
        /// <param name="button">The source of the event.</param>
        private void ShowPillPanel( LinkButton button )
        {
            liProjects.Attributes["class"] = button == lbProjects ? "active" : string.Empty;
            liGoals.Attributes["class"] = button == lbGoals ? "active" : string.Empty;
            liNotes.Attributes["class"] = button == lbNotes ? "active" : string.Empty;

            pnlProjects.Visible = button == lbProjects;
            pnlGoals.Visible = button == lbGoals;

            // only show the Notes block when the Notes pill is selected
            this.HideBlockType( "Notes", button != lbNotes );
        }

        /// <summary>
        /// Handles the Click event of the lbGoals control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbGoals_Click( object sender, EventArgs e )
        {
            ShowPillPanel( sender as LinkButton );
        }

        /// <summary>
        /// Handles the Click event of the lbNotes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbNotes_Click( object sender, EventArgs e )
        {
            ShowPillPanel( sender as LinkButton );
        }

        /// <summary>
        /// Handles the Click event of the lbProjects control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbProjects_Click( object sender, EventArgs e )
        {
            ShowPillPanel( sender as LinkButton );

            BindProjectListGrid();
        }

        /// <summary>
        /// Binds the project list grid.
        /// </summary>
        protected void BindProjectListGrid()
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
        /// Handles the GridRebind event of the gProjectList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gProjectList_GridRebind( object sender, EventArgs e )
        {
            BindProjectListGrid();
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
    }
}