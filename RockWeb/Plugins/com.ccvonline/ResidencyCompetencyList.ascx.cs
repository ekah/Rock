using System;
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

namespace com.ccvonline.Blocks
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    public partial class ResidencyCompetencyList : RockBlock, IDimmableBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            BindFilter();
            rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
            rFilter.DisplayFilterValue += rFilter_DisplayFilterValue;

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
        /// Binds the filter.
        /// </summary>
        protected void BindFilter()
        {
            var residencyPeriodList = new ResidencyService<ResidencyPeriod>().Queryable().OrderBy( a => a.Name ).ToList();
            ddlResidencyPeriod.Items.Clear();
            ddlResidencyPeriod.Items.Add( Rock.Constants.All.ListItem );
            foreach ( var p in residencyPeriodList )
            {
                ddlResidencyPeriod.Items.Add( new ListItem( p.Name, p.Id.ToString() ) );
            }

            int? residencyPeriodId = rFilter.GetUserPreference( "ResidencyPeriod" ).AsInteger( false ) ?? Rock.Constants.All.Id;

            ddlResidencyPeriod.SelectedValue = residencyPeriodId.Value.ToString();

            ddlResidencyPeriod_SelectedIndexChanged( null, null );
        }

        /// <summary>
        /// Rs the filter_ display filter value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void rFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "ResidencyPeriod":
                    int? residencyPeriodId = e.Value.AsInteger( false );
                    if ( residencyPeriodId.HasValue )
                    {
                        e.Value = new ResidencyService<ResidencyPeriod>().Get( residencyPeriodId.Value ).Name;
                        e.Key = e.Key.SplitCase();
                    }

                    break;

                case "ResidencyTrack":
                    int? residencyTrackId = e.Value.AsInteger( false );
                    if ( residencyTrackId.HasValue )
                    {
                        var list = new ResidencyService<ResidencyTrack>().Queryable().ToList();
                        var track = new ResidencyService<ResidencyTrack>().Get( residencyTrackId.Value );
                        e.Value = track.Name;
                        e.Key = e.Key.SplitCase();
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the ApplyFilterClick event of the rFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            if ( ddlResidencyPeriod.SelectedValue.Equals( Rock.Constants.All.IdValue ) )
            {
                rFilter.SaveUserPreference( "ResidencyPeriod", string.Empty );
            }
            else
            {
                rFilter.SaveUserPreference( "ResidencyPeriod", ddlResidencyPeriod.SelectedValue );
            }

            if ( ddlResidencyTrack.SelectedValue.Equals( Rock.Constants.All.IdValue ) )
            {
                rFilter.SaveUserPreference( "ResidencyTrack", string.Empty );
            }
            else
            {
                rFilter.SaveUserPreference( "ResidencyTrack", ddlResidencyTrack.SelectedValue );
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlResidencyPeriod control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlResidencyPeriod_SelectedIndexChanged( object sender, EventArgs e )
        {
            ddlResidencyTrack.Items.Clear();
            ddlResidencyTrack.Items.Add( Rock.Constants.All.ListItem );

            var qry = new ResidencyService<ResidencyTrack>().Queryable();
            
            int residencyPeriodId = ddlResidencyPeriod.SelectedValueAsInt() ?? Rock.Constants.All.Id;
            if ( !residencyPeriodId.Equals( Rock.Constants.All.Id ) )
            {
                ddlResidencyTrack.Enabled = true;
                qry = qry.Where( a => a.ResidencyPeriodId.Equals( residencyPeriodId ) );

                foreach ( var item in qry.OrderBy( a => a.Name ).ToList() )
                {
                    ddlResidencyTrack.Items.Add( new ListItem( item.Name, item.Id.ToString() ) );
                }

                int? residencyTrackId = rFilter.GetUserPreference( "ResidencyTrack" ).AsInteger( false ) ?? Rock.Constants.All.Id;

                ddlResidencyTrack.SetValue( residencyTrackId );
            }
            else
            {
                ddlResidencyTrack.Enabled = false;
                ddlResidencyTrack.SetValue( Rock.Constants.All.IdValue );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                BindGrid();
            }

            base.OnLoad( e );
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
            NavigateToDetailPage( "residencyCompetencyId", 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( "residencyCompetencyId", (int)e.RowKeyValue );
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
                var residencyCompetencyService = new ResidencyService<ResidencyCompetency>();

                ResidencyCompetency residencyCompetency = residencyCompetencyService.Get( (int)e.RowKeyValue );
                if ( residencyCompetency != null )
                {
                    residencyCompetencyService.Delete( residencyCompetency, CurrentPersonId );
                    residencyCompetencyService.Save( residencyCompetency, CurrentPersonId );
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
            var residencyCompetencyService = new ResidencyService<ResidencyCompetency>();
            SortProperty sortProperty = gList.SortProperty;
            IQueryable<ResidencyCompetency> qry = null;

            if ( sortProperty != null )
            {
                qry = residencyCompetencyService.Queryable().Sort( sortProperty );
            }
            else
            {
                qry = residencyCompetencyService.Queryable()
                    .OrderBy( s => s.ResidencyTrack.ResidencyPeriod.Name )
                    .ThenBy( s => s.ResidencyTrack.Name )
                    .ThenBy( s => s.Name );
            }

            int? residencyPeriodId = rFilter.GetUserPreference( "ResidencyPeriod" ).AsInteger( false );
            if ( residencyPeriodId.HasValue )
            {
                qry = qry.Where( a => a.ResidencyTrack.ResidencyPeriodId == residencyPeriodId.Value );
            }
            
            int? residencyTrackId = rFilter.GetUserPreference( "ResidencyTrack" ).AsInteger( false );
            if ( residencyTrackId.HasValue )
            {
                qry = qry.Where( a => a.ResidencyTrackId == residencyTrackId.Value );
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