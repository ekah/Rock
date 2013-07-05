﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    [DetailPage]
    public partial class ResidentCompetencyList : RockBlock, IDimmableBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
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
                BindRepeater();
            }
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Handles the RowSelected event of the gCompetencyList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gCompetencyList_RowSelected( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( "competencyPersonId", e.RowKeyId );
        }

        /// <summary>
        /// Handles the GridRebind event of the gCompetencyList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gCompetencyList_GridRebind( object sender, EventArgs e )
        {
            // shouldn't happen
            throw new NotImplementedException();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Binds the repeater.
        /// </summary>
        private void BindRepeater()
        {
            lblPersonName.Text = this.CurrentPerson.FullName;
            var trackService = new ResidencyService<Track>();
            var competencyPersonService = new ResidencyService<CompetencyPerson>();
            int currentPersonId = this.CurrentPersonId ?? 0;

            List<int> residentCompetencyIds = competencyPersonService.Queryable().Where( a => a.PersonId.Equals( currentPersonId ) )
                .Select( x => x.CompetencyId ).Distinct().ToList();

            var qryPersonTracks = trackService.Queryable().Where( a => residentCompetencyIds.Any( rc => a.Competencies.Select( c => c.Id ).Contains( rc ) ) )
                .OrderBy( o => o.DisplayOrder );

            rpTracks.DataSource = qryPersonTracks.ToList();
            rpTracks.DataBind();
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rpTracks control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rpTracks_ItemDataBound( object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e )
        {
            Track track = e.Item.DataItem as Track;
            if ( track != null )
            {
                int trackId = track.Id;
                int currentPersonId = this.CurrentPersonId ?? 0;
                Grid gCompetencyList = e.Item.FindControl( "gCompetencyList" ) as Grid;
                var competencyPersonService = new ResidencyService<CompetencyPerson>();

                var qryPersonCompetencies = competencyPersonService.Queryable()
                    .Where( a => a.PersonId.Equals( currentPersonId ) && a.Competency.TrackId.Equals( trackId ) )
                    .Select( a => new
                    {
                        Id = a.Id,
                        CompetencyName = a.Competency.Name,
                        CompletedProjectAssessmentsTotal = a.CompetencyPersonProjects.Select( p => p.CompetencyPersonProjectAssessments ).SelectMany( x => x ).Where( n => n.AssessmentDateTime != null ).Count(),
                        MinProjectAssessmentsTotal = a.CompetencyPersonProjects.Select( p => p.MinAssessmentCount ?? p.Project.MinAssessmentCountDefault ?? 0 ).DefaultIfEmpty().Sum()
                    } )
                    .OrderBy( o => o.CompetencyName );

                gCompetencyList.DataKeyNames = new string[] { "Id" };
                gCompetencyList.DisplayType = GridDisplayType.Light;
                gCompetencyList.Actions.ShowAdd = false;
                gCompetencyList.RowSelected += gCompetencyList_RowSelected;
                gCompetencyList.GridRebind += gCompetencyList_GridRebind;
                gCompetencyList.DataSource = qryPersonCompetencies.ToList();
                gCompetencyList.DataBind();
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
            foreach ( var item in rpTracks.Items.OfType<RepeaterItem>() )
            {
                foreach ( var grid in item.Controls.OfType<Grid>() )
                {
                    grid.Enabled = !dimmed;
                }

                foreach ( var label in item.Controls.OfType<Label>() )
                {
                    label.Enabled = !dimmed;
                }
            }
        }

        #endregion
    }
}