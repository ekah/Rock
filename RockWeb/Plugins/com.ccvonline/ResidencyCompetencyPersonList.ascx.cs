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

namespace com.ccvonline.Blocks
{
    /// <summary>
    /// Lists ResidencyCompetencies for a Person
    /// </summary>
    [DetailPage]
    public partial class ResidencyCompetencyPersonList : RockBlock, IDimmableBlock
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
                int? personId = this.PageParameter( "personId" ).AsInteger();
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
        /// <param name="residencyCompetencyPersonId">The residency competency person id.</param>
        protected void gList_ShowEdit( int residencyCompetencyPersonId )
        {
            NavigateToDetailPage( "residencyCompetencyPersonId", residencyCompetencyPersonId, "personId", hfPersonId.ValueAsInt() );
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
                var residencyCompetencyPersonService = new ResidencyService<ResidencyCompetencyPerson>();
                ResidencyCompetencyPerson residencyCompetencyPerson = residencyCompetencyPersonService.Get( (int)e.RowKeyValue );

                if ( residencyCompetencyPerson != null )
                {
                    string errorMessage;
                    if ( !residencyCompetencyPersonService.CanDelete( residencyCompetencyPerson, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    residencyCompetencyPersonService.Delete( residencyCompetencyPerson, CurrentPersonId );
                    residencyCompetencyPersonService.Save( residencyCompetencyPerson, CurrentPersonId );
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
            var residencyCompetencyPersonService = new ResidencyService<ResidencyCompetencyPerson>();
            int personId = hfPersonId.ValueAsInt();
            SortProperty sortProperty = gList.SortProperty;
            var qry = residencyCompetencyPersonService.Queryable();

            qry = qry.Where( a => a.PersonId.Equals( personId ) );

            if ( sortProperty != null )
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderBy( s => s.ResidencyCompetency.Name );
            }

            gList.DataSource = qry.Select( a => new
            {
                Id = a.Id,
                ResidencyCompetencyName = a.ResidencyCompetency.Name,
                CompletedProjectsTotal = a.ResidencyCompetencyPersonProjects.Select( p => p.ResidencyCompetencyPersonProjectAssignments ).SelectMany( x => x ).Where( n => n.CompletedDateTime != null ).Count(),
                AssignedProjectsTotal = a.ResidencyCompetencyPersonProjects.Select( p => p.ResidencyCompetencyPersonProjectAssignments ).SelectMany( x => x ).Count()
            } ).ToList();
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