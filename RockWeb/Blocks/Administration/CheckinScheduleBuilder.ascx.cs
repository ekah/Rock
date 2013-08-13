//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckinScheduleBuilder : RockBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            pCategory.EntityTypeId = EntityTypeCache.GetId( typeof( Rock.Model.Schedule ) ) ?? 0;

            rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
            rFilter.DisplayFilterValue += rFilter_DisplayFilterValue;
            BindFilter();

            if ( !Page.IsPostBack )
            {
                AddScheduleColumns();
            }

            gGroupLocationSchedule.DataKeyNames = new string[] { "GroupLocationId" };
            gGroupLocationSchedule.Actions.ShowAdd = false;
            gGroupLocationSchedule.IsDeleteEnabled = false;
            gGroupLocationSchedule.GridRebind += gGroupLocationSchedule_GridRebind;
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

        /// <summary>
        /// Adds the schedule columns.
        /// </summary>
        private void AddScheduleColumns()
        {
            ScheduleService scheduleService = new ScheduleService();

            // limit Schedules to ones that have a CheckInStartOffsetMinutes
            var scheduleQry = scheduleService.Queryable().Where( a => a.CheckInStartOffsetMinutes != null );

            // limit Schedules to the Category from the Filter
            int? scheduleCategoryId = pCategory.SelectedValueAsInt() ?? Rock.Constants.All.Id;
            if ( scheduleCategoryId != Rock.Constants.All.Id )
            {
                scheduleQry = scheduleQry.Where( a => a.CategoryId == scheduleCategoryId );
            }
            else
            {
                // NULL (or 0) means Shared, so specifically filter so to show only Schedules with CategoryId NULL
                scheduleQry = scheduleQry.Where( a => a.CategoryId == null );
            }

            var scheduleList = scheduleQry.OrderBy( a => a.Name ).Select( a => new { a.Id, a.Name } ).ToList();

            foreach ( var item in scheduleList )
            {
                string dataFieldName = string.Format( "scheduleField_{0}", item.Id );

                CheckBoxEditableField field = new CheckBoxEditableField { HeaderText = item.Name, DataField = dataFieldName };
                gGroupLocationSchedule.Columns.Add( field );
            }
        }

        #endregion

        #region Grid Filter

        /// <summary>
        /// Binds any needed data to the Grid Filter also using the user's stored
        /// preferences.
        /// </summary>
        private void BindFilter()
        {
            ddlGroupType.Items.Clear();
            ddlGroupType.Items.Add( Rock.Constants.All.ListItem );

            GroupTypeService groupTypeService = new GroupTypeService();
            var groupTypeList = groupTypeService.Queryable().ToList();
            foreach ( var groupType in groupTypeList )
            {
                ddlGroupType.Items.Add( new ListItem( groupType.Name, groupType.Id.ToString() ) );
            }

            ddlGroupType.SetValue( rFilter.GetUserPreference( "Group Type" ) );

            var filterCategory = new CategoryService().Get( rFilter.GetUserPreference( "Category" ).AsInteger() ?? 0 );
            pCategory.SetValue( filterCategory );

            // TODO: Replace with Location Picker
            ddlParentLocation.Items.Clear();
            ddlParentLocation.Items.Add( Rock.Constants.All.ListItem );
            LocationService locationService = new LocationService();
            var locationList = locationService.Queryable().OrderBy( a => a.Name ).ToList();
            foreach ( var location in locationList )
            {
                ddlParentLocation.Items.Add( new ListItem( location.Name, location.Id.ToString() ) );
            }

            ddlParentLocation.SetValue( rFilter.GetUserPreference( "Parent Location" ) );
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Handles the ApplyFilterClick event of the rFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rFilter.SaveUserPreference( "Group Type", ddlGroupType.SelectedValue );
            rFilter.SaveUserPreference( "Category", pCategory.SelectedValue );
            rFilter.SaveUserPreference( "Parent Location", ddlParentLocation.SelectedValue );

            BindGrid();
        }

        /// <summary>
        /// Rs the filter_ display filter value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void rFilter_DisplayFilterValue( object sender, Rock.Web.UI.Controls.GridFilter.DisplayFilterValueArgs e )
        {
            int itemId = e.Value.AsInteger() ?? 0;
            if ( itemId == Rock.Constants.All.Id )
            {
                e.Value = Rock.Constants.All.Text;
            }
            else if ( itemId == Rock.Constants.None.Id )
            {
                e.Value = Rock.Constants.None.TextHtml;
            }
            else
            {
                switch ( e.Key )
                {
                    case "Group Type":

                        var groupType = new GroupTypeService().Get( itemId );
                        if ( groupType != null )
                        {
                            e.Value = groupType.Name;
                        }

                        break;

                    case "Category":

                        var category = new CategoryService().Get( itemId );
                        if ( category != null )
                        {
                            e.Value = category.Name;
                        }

                        break;

                    case "Parent Location":

                        var location = new LocationService().Get( itemId );
                        if ( location != null )
                        {
                            e.Value = location.Name;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Handles the GridRebind event of the gGroupLocationSchedule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void gGroupLocationSchedule_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        protected void BindGrid()
        {
            var groupLocationService = new GroupLocationService();

            var groupLocationQry = groupLocationService.Queryable();

            int groupTypeId = ddlGroupType.SelectedValueAsInt() ?? Rock.Constants.All.Id;
            if ( groupTypeId != Rock.Constants.All.Id )
            {
                groupLocationQry = groupLocationQry.Where( a => a.Group.GroupTypeId == groupTypeId );
            }

            var qryList = groupLocationQry.Select( a =>
                new
                {
                    GroupLocationId = a.Id,
                    GroupName = a.Group.Name,
                    LocationName = a.Location.Name,
                    ScheduleIdList = a.Schedules.Select( s => s.Id ),
                    a.LocationId
                } ).ToList();

            int parentLocationId = ddlParentLocation.SelectedValueAsInt() ?? Rock.Constants.All.Id;
            if ( parentLocationId != Rock.Constants.All.Id )
            {
                var descendantLocationIds = new LocationService().GetAllDescendents(parentLocationId).Select(a => a.Id);
                qryList = qryList.Where( a => descendantLocationIds.Contains( a.LocationId ) ).ToList();
            }

            // put stuff in a datatable so we can dynamically have columns for each Schedule
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add( "GroupLocationId" );
            dataTable.Columns.Add( "GroupName" );
            dataTable.Columns.Add( "LocationName" );
            foreach ( var field in gGroupLocationSchedule.Columns.OfType<CheckBoxEditableField>() )
            {
                dataTable.Columns.Add( field.DataField, typeof( bool ) );
            }

            foreach ( var row in qryList )
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["GroupLocationId"] = row.GroupLocationId;
                dataRow["GroupName"] = row.GroupName;
                dataRow["LocationName"] = row.LocationName;
                foreach ( var field in gGroupLocationSchedule.Columns.OfType<CheckBoxEditableField>() )
                {
                    int scheduleId = int.Parse( field.DataField.Replace( "scheduleField_", string.Empty ) );
                    dataRow[field.DataField] = row.ScheduleIdList.Any( a => a == scheduleId );
                }

                dataTable.Rows.Add( dataRow );
            }

            gGroupLocationSchedule.DataSource = dataTable;
            gGroupLocationSchedule.DataBind();
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            using ( new UnitOfWorkScope() )
            {
                GroupLocationService groupLocationService = new GroupLocationService();
                ScheduleService scheduleService = new ScheduleService();
                
                RockTransactionScope.WrapTransaction( () =>
                {
                    var gridViewRows = gGroupLocationSchedule.Rows;
                    foreach ( GridViewRow row in gridViewRows.OfType<GridViewRow>() )
                    {
                        int groupLocationId = int.Parse( gGroupLocationSchedule.DataKeys[row.RowIndex].Value as string );
                        GroupLocation groupLocation = groupLocationService.Get( groupLocationId );
                        if ( groupLocation != null )
                        {
                            foreach ( var fieldCell in row.Cells.OfType<DataControlFieldCell>())
                            {
                                CheckBoxEditableField checkBoxTemplateField = fieldCell.ContainingField as CheckBoxEditableField;
                                if ( checkBoxTemplateField != null )
                                {
                                    CheckBox checkBox = fieldCell.Controls[0] as CheckBox;
                                    string dataField = ( fieldCell.ContainingField as CheckBoxEditableField ).DataField;
                                    int scheduleId = int.Parse( dataField.Replace( "scheduleField_", string.Empty ) );

                                    // update GroupLocationSchedule depending on if the Schedule is Checked or not
                                    if ( checkBox.Checked )
                                    {
                                        // This schedule is selected, so if GroupLocationSchedule doesn't already have this schedule, add it
                                        if ( !groupLocation.Schedules.Any( a => a.Id == scheduleId ) )
                                        {
                                            var schedule = scheduleService.Get( scheduleId );
                                            groupLocation.Schedules.Add( schedule );
                                        }
                                    }
                                    else
                                    {
                                        // This schedule is not selected, so if GroupLocationSchedule has this schedule, delete it
                                        if ( groupLocation.Schedules.Any( a => a.Id == scheduleId ) )
                                        {
                                            groupLocation.Schedules.Remove( groupLocation.Schedules.FirstOrDefault( a => a.Id == scheduleId ) );
                                        }
                                    }
                                }
                            }

                            groupLocationService.Save( groupLocation, this.CurrentPersonId );
                        }
                    }

                } );
            }

            NavigateToParentPage();
        }

        #endregion
    }
}