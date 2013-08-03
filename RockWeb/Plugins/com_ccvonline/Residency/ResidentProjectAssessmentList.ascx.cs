﻿using System;
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
    [BooleanField( "Show Competency Column" )]
    [BooleanField( "Show Project Column" )]
    [BooleanField( "Show Grid Title" )]
    public partial class ResidentProjectAssessmentList : RockBlock, IDimmableBlock
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
            boundFields["AssessmentDateTime"].NullDisplayText = "not completed";

            boundFields["CompetencyPersonProject.CompetencyPerson.Competency.Track.Name"].Visible = this.GetAttributeValue( "ShowCompetencyColumn" ).AsBoolean();
            boundFields["CompetencyPersonProject.CompetencyPerson.Competency.Name"].Visible = this.GetAttributeValue( "ShowCompetencyColumn" ).AsBoolean();
            boundFields["CompetencyPersonProject.Project.Name"].Visible = this.GetAttributeValue( "ShowProjectColumn" ).AsBoolean();
            lblTitle.Visible = this.GetAttributeValue( "ShowGridTitle" ).AsBoolean();
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
            NavigateToDetailPage( "competencyPersonProjectAssessmentId", e.RowKeyId );
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
            var competencyPersonProjectAssessmentService = new ResidencyService<CompetencyPersonProjectAssessment>();
            int competencyPersonProjectId = hfCompetencyPersonProjectId.ValueAsInt();
            SortProperty sortProperty = gList.SortProperty;

            var qry = competencyPersonProjectAssessmentService.Queryable( "AssessorPerson" );

            if ( competencyPersonProjectId != 0 )
            {
                // limit to specific project (and current person)
                qry = qry.Where( a => a.CompetencyPersonProjectId.Equals( competencyPersonProjectId ) );
            }
            else
            {
                // limit only to current person
                qry = qry.Where( a => a.CompetencyPersonProject.CompetencyPerson.PersonId == this.CurrentPersonId );
            }


            if ( sortProperty != null )
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderByDescending( s => s.AssessmentDateTime ).ThenBy( s => s.AssessorPerson );
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