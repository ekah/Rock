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

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [DetailPage]
    public partial class PersonList : RockBlock, IDimmableBlock
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
            NavigateToDetailPage( "groupMemberId", 0, "groupId", hfGroupId.ValueAsInt() );
        }

        /// <summary>
        /// Handles the Edit event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( "groupMemberId", (int)e.RowKeyValue, "groupId", hfGroupId.ValueAsInt() );
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
                int groupMemberId = (int)e.RowKeyValue;

                GroupMember groupMember = groupMemberService.Get( groupMemberId );
                if ( groupMember != null )
                {
                    // check if person can be removed from the Group and also check if person can be removed from all the person assigned competencies
                    string errorMessage;
                    if ( !groupMemberService.CanDelete( groupMember, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    var competencyPersonService = new ResidencyService<CompetencyPerson>();
                    var personCompetencyList = competencyPersonService.Queryable().Where( a => a.PersonId.Equals( groupMember.PersonId ) );
                    foreach ( var item in personCompetencyList )
                    {
                        if ( !competencyPersonService.CanDelete( item, out errorMessage ) )
                        {
                            mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                            return;
                        }
                    }

                    // if you made it this far, delete all person's assigned competencies, and finally delete from Group
                    foreach ( var item in personCompetencyList )
                    {
                        competencyPersonService.Delete( item, CurrentPersonId );
                        competencyPersonService.Save( item, CurrentPersonId );
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

            int residencyGroupId = PageParameter( "groupId" ).AsInteger() ?? 0;
            hfGroupId.SetValue( residencyGroupId );

            var residencyGroupMemberList = residencyGroupMemberService.Queryable()
                .Where( a => a.GroupId.Equals( residencyGroupId ) ).ToList();

            var competencyPersonService = new ResidencyService<CompetencyPerson>();
            List<IGrouping<int, CompetencyPerson>> competencyPersonQry = competencyPersonService.Queryable().GroupBy( a => a.PersonId ).ToList();

            var groupMemberCompetencies = from groupMember in residencyGroupMemberList
                                          join competencyList in competencyPersonQry on groupMember.PersonId
                                          equals competencyList.Key into groupJoin
                                          from qryResult in groupJoin.DefaultIfEmpty()
                                          select new
                                          {
                                              GroupMember = groupMember,
                                              ResidentCompentencies = qryResult != null ? qryResult.ToList() : null
                                          };

            var dataResult = groupMemberCompetencies.Select( a => new
            {
                Id = a.GroupMember.Id,
                FullName = a.GroupMember.Person.FullName,
                CompetencyCount = a.ResidentCompentencies == null ? 0 : a.ResidentCompentencies.Count(),
                CompletedProjectAssessmentsTotal = a.ResidentCompentencies == null
                    ? 0
                    : a.ResidentCompentencies.SelectMany( cp => cp.CompetencyPersonProjects ).SelectMany( x => x.CompetencyPersonProjectAssessments ).Where( y => y.AssessmentDateTime != null ).Count(),
                MinAssessmentCount = a.ResidentCompentencies == null
                    ? 0
                    : a.ResidentCompentencies.SelectMany( cp => cp.CompetencyPersonProjects ).Select( x => x.MinAssessmentCount ?? 0 ).Sum(),
            } );

            SortProperty sortProperty = gList.SortProperty;

            if ( sortProperty != null )
            {
                gList.DataSource = dataResult.AsQueryable().Sort( sortProperty ).ToList();
            }
            else
            {
                gList.DataSource = dataResult.OrderBy( s => s.FullName ).ToList();
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