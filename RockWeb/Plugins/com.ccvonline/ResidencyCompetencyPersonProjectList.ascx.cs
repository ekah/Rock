using System;
using System.Linq;
using System.Web.UI;
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
    public partial class ResidencyCompetencyPersonProjectList : RockBlock, IDimmableBlock
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
                int? residencyCompetencyPersonId = this.PageParameter( "residencyCompetencyPersonId" ).AsInteger();
                hfResidencyCompetencyPersonId.Value = residencyCompetencyPersonId.ToString();
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
        /// <param name="residencyCompetencyPersonProjectId">The residency competency person project id.</param>
        protected void gList_ShowEdit( int residencyCompetencyPersonProjectId )
        {
            NavigateToDetailPage( "residencyCompetencyPersonProjectId", residencyCompetencyPersonProjectId, "residencyCompetencyPersonId", hfResidencyCompetencyPersonId.ValueAsInt() );
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
                var residencyCompetencyPersonProjectService = new ResidencyService<ResidencyCompetencyPersonProject>();
                ResidencyCompetencyPersonProject residencyCompetencyPersonProject = residencyCompetencyPersonProjectService.Get( (int)e.RowKeyValue );

                if ( residencyCompetencyPersonProject != null )
                {
                    string errorMessage;
                    if ( !residencyCompetencyPersonProjectService.CanDelete( residencyCompetencyPersonProject, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    residencyCompetencyPersonProjectService.Delete( residencyCompetencyPersonProject, CurrentPersonId );
                    residencyCompetencyPersonProjectService.Save( residencyCompetencyPersonProject, CurrentPersonId );
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
            var ResidencyCompetencyPersonProjectService = new ResidencyService<ResidencyCompetencyPersonProject>();
            int ResidencyCompetencyPersonId = hfResidencyCompetencyPersonId.ValueAsInt();
            SortProperty sortProperty = gList.SortProperty;
            var qry = ResidencyCompetencyPersonProjectService.Queryable();

            qry = qry.Where( a => a.ResidencyCompetencyPersonId.Equals( ResidencyCompetencyPersonId ) );

            if ( sortProperty != null )
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderBy( s => s.ResidencyProject.Name );
            }

            var list = qry.Select( a => new 
            {
                Id = a.Id,
                Name = a.ResidencyProject.Name,
                MinAssignmentCount = a.MinAssignmentCount,
                CurrentCompleted = a.ResidencyCompetencyPersonProjectAssignments.Where(b => b.CompletedDateTime != null).Count()
            }).ToList();

            gList.DataSource = list;
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