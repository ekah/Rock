﻿using System;
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
    [DetailPage]
    public partial class TrackList : RockBlock, IDimmableBlock
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
            gList.GridReorder += gList_GridReorder;

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
                int? periodId = this.PageParameter( "periodId" ).AsInteger();
                hfPeriodId.Value = periodId.ToString();
                BindGrid();
            }
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Handles the GridReorder event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridReorderEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gList_GridReorder( object sender, GridReorderEventArgs e )
        {
            int oldIndex = e.OldIndex;
            int newIndex = e.NewIndex;
            int periodId = hfPeriodId.ValueAsInt();

            var trackService = new ResidencyService<Track>();
            var items = trackService.Queryable()
                .Where( a => a.PeriodId.Equals( periodId ) )
                .OrderBy( a => a.DisplayOrder ).ToList();

            RockTransactionScope.WrapTransaction( () =>
            {
                Track movedItem = items[oldIndex];
                items.RemoveAt( oldIndex );
                if ( newIndex >= items.Count )
                {
                    items.Add( movedItem );
                }
                else
                {
                    items.Insert( newIndex, movedItem );
                }

                int order = 1;
                foreach ( Track item in items )
                {
                    if ( item != null )
                    {
                        if ( item.DisplayOrder != order )
                        {
                            // temporarily, set the order to negative in case another row has this value.
                            item.DisplayOrder = -order;
                            trackService.Save( item, CurrentPersonId );
                        }
                    }

                    order++;
                }

                foreach ( Track item in items )
                {
                    if ( item != null )
                    {
                        if ( item.DisplayOrder < 0 )
                        {
                            // update the value back to positive now that all the rows have their new order
                            item.DisplayOrder = -item.DisplayOrder;
                            trackService.Save( item, CurrentPersonId );
                        }
                    }
                }
            } );

            BindGrid();
        }

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
        /// <param name="projectPointOfAssessmentId">The residency project point of assessment id.</param>
        protected void gList_ShowEdit( int projectPointOfAssessmentId )
        {
            NavigateToDetailPage( "trackId", projectPointOfAssessmentId, "periodId", hfPeriodId.ValueAsInt() );
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
                var trackService = new ResidencyService<Track>();
                Track track = trackService.Get( (int)e.RowKeyValue );

                if ( track != null )
                {
                    string errorMessage;
                    if ( !trackService.CanDelete( track, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    trackService.Delete( track, CurrentPersonId );
                    trackService.Save( track, CurrentPersonId );
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
            var trackService = new ResidencyService<Track>();
            int periodId = hfPeriodId.ValueAsInt();
            SortProperty sortProperty = gList.SortProperty;
            var qry = trackService.Queryable();

            qry = qry.Where( a => a.PeriodId.Equals( periodId ) );

            if ( sortProperty != null )
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderBy( s => s.DisplayOrder ).ThenBy( s => s.Name );
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