﻿using System;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// Lists ResidencyCompetencies for a Person
    /// </summary>
    [DetailPage]
    public partial class CompetencyPersonList : RockBlock, IDimmableBlock
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
            gList.Actions.ShowAdd = true;
            gList.Actions.AddClick += gList_Add;
            gList.GridRebind += gList_GridRebind;

            // Block Security and special attributes (RockPage takes care of "View")
            bool canAddEditDelete = IsUserAuthorized( "Edit" );
            gList.Actions.ShowAdd = canAddEditDelete;
            gList.IsDeleteEnabled = canAddEditDelete;
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
                // allow this block to work with either a personId or groupMemberId parameter
                int personId = this.PageParameter( "personId" ).AsInteger() ?? 0;
                if ( personId == 0 )
                {
                    int groupMemberId = this.PageParameter( "groupMemberId" ).AsInteger() ?? 0;
                    personId = new ResidencyService<GroupMember>().Queryable().Where( a => a.Id.Equals( groupMemberId ) ).Select( a => a.PersonId ).FirstOrDefault();
                }

                hfPersonId.Value = personId.ToString();
                BindGrid();
            }
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Handles the Add event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gList_Add( object sender, EventArgs e )
        {
            gList_ShowEdit( 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Edit( object sender, RowEventArgs e )
        {
            gList_ShowEdit( (int)e.RowKeyValue );
        }

        /// <summary>
        /// Gs the list_ show edit.
        /// </summary>
        /// <param name="competencyPersonId">The residency competency person id.</param>
        protected void gList_ShowEdit( int competencyPersonId )
        {
            NavigateToDetailPage( "competencyPersonId", competencyPersonId, "personId", hfPersonId.ValueAsInt() );
        }

        /// <summary>
        /// Handles the Delete event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Delete( object sender, RowEventArgs e )
        {
            RockTransactionScope.WrapTransaction( () =>
            {
                var competencyPersonService = new ResidencyService<CompetencyPerson>();
                CompetencyPerson competencyPerson = competencyPersonService.Get( (int)e.RowKeyValue );

                if ( competencyPerson != null )
                {
                    string errorMessage;
                    if ( !competencyPersonService.CanDelete( competencyPerson, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    competencyPersonService.Delete( competencyPerson, CurrentPersonId );
                    competencyPersonService.Save( competencyPerson, CurrentPersonId );
                }
            } );

            BindGrid();
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
            var competencyPersonService = new ResidencyService<CompetencyPerson>();
            int personId = hfPersonId.ValueAsInt();
            SortProperty sortProperty = gList.SortProperty;
            var qry = competencyPersonService.Queryable()
                .Where( a => a.PersonId.Equals( personId ) )
                .Select( a => new
                {
                    Id = a.Id,
                    TrackDisplayOrder = a.Competency.Track.DisplayOrder,
                    TrackName = a.Competency.Track.Name,
                    CompetencyName = a.Competency.Name,
                    CompletedProjectsTotal = a.CompetencyPersonProjects.Select( p => p.CompetencyPersonProjectAssignments ).SelectMany( x => x ).Where( n => n.CompletedDateTime != null ).Count(),
                    AssignedProjectsTotal = a.CompetencyPersonProjects.Select( p => p.CompetencyPersonProjectAssignments ).SelectMany( x => x ).Count()
                } );

            if ( sortProperty != null )
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderBy( s => s.TrackDisplayOrder ).ThenBy( s => s.TrackName ).ThenBy( s => s.CompetencyName );
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