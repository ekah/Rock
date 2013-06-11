using System;
using System.Collections.Generic;
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

namespace com.ccvonline.Blocks
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    public partial class ResidencyPersonList : RockBlock, IDimmableBlock
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
            NavigateToDetailPage( "personId", 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( "personId", (int)e.RowKeyValue );
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
                var groupMemberService = new GroupMemberService();
                int residencyGroupId = new GroupService().GetByGuid( new Guid( "4B7D22E8-B08C-42DC-B1F1-F2834BC8D1DF" ) ).Id;
                int personId = (int)e.RowKeyValue;

                GroupMember groupMember = groupMemberService.Queryable().Where( a => a.GroupId.Equals( residencyGroupId ) && a.PersonId.Equals( personId ) ).FirstOrDefault();
                if ( groupMember != null )
                {
                    // check if person can be removed from the Group and also check if person can be removed from all the person assigned competencies
                    string errorMessage;
                    if ( !groupMemberService.CanDelete( groupMember, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    var residencyCompetencyPersonService = new ResidencyService<ResidencyCompetencyPerson>();
                    var personCompetencyList  =residencyCompetencyPersonService.Queryable().Where( a => a.PersonId.Equals(personId));
                    foreach (var item in personCompetencyList)
                    {
                        if ( !residencyCompetencyPersonService.CanDelete( item, out errorMessage ) )
                        {
                            mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                            return;
                        }
                    }

                    // if you made it this far, delete all person's assigned competencies, and finally delete from Group
                    foreach ( var item in personCompetencyList )
                    {
                        residencyCompetencyPersonService.Delete( item, CurrentPersonId );
                        residencyCompetencyPersonService.Save( item, CurrentPersonId );
                    }

                    groupMemberService.Delete( groupMember, CurrentPersonId );
                    groupMemberService.Save( groupMember, CurrentPersonId );
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
            var residencyGroupMemberService = new ResidencyService<Rock.Model.GroupMember>();
            int residencyGroupId = new GroupService().GetByGuid( new Guid( "4B7D22E8-B08C-42DC-B1F1-F2834BC8D1DF" ) ).Id;
            List<Person> residentPersonList = residencyGroupMemberService.Queryable().Where( a => a.GroupId.Equals( residencyGroupId ) ).Select(a => a.Person).ToList();

            var residencyCompetencyPersonService = new ResidencyService<ResidencyCompetencyPerson>();
            var residencyCompetencyPersonQry = residencyCompetencyPersonService.Queryable().GroupBy( a => a.Person ).ToList();

            var joinedItems = from person in residentPersonList
                            join competencyList in residencyCompetencyPersonQry on person equals competencyList.Key into gj
                            from subCompetency in gj.DefaultIfEmpty()
                            select new 
                            { 
                                person.Id,
                                person.FullName, 
                                CompetencyCount = (subCompetency == null ? 0 : subCompetency.Count()),
                                CompletedProjectsTotal = (subCompetency == null ? 0 :subCompetency.Select(a => a.ResidencyCompetencyPersonProjects.Select( p=> p.ResidencyCompetencyPersonProjectAssignments).SelectMany(x => x).Where( n=> n.CompletedDateTime != null).Count()).Sum()),
                                AssignedProjectsTotal = (subCompetency == null ? 0 :subCompetency.Select( a => a.ResidencyCompetencyPersonProjects.Select( p => p.ResidencyCompetencyPersonProjectAssignments ).SelectMany( x => x ).Count() ).Sum())
                            };

            
            SortProperty sortProperty = gList.SortProperty;

            if ( sortProperty != null )
            {
                gList.DataSource = joinedItems.AsQueryable().Sort( sortProperty ).ToList();
            }
            else
            {
                gList.DataSource = joinedItems.OrderBy( s => s.FullName ).ToList();
            }

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