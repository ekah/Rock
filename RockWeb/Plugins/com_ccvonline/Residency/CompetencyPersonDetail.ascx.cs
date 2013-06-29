//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// Simple detail form that for a Resident's assignment to a specific Competency
    /// </summary>
    [LinkedPage( "Resident Detail Page" )]
    public partial class CompetencyPersonDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string itemId = PageParameter( "competencyPersonId" );
                string personId = PageParameter( "personId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( personId ) )
                    {
                        ShowDetail( "competencyPersonId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "competencyPersonId", int.Parse( itemId ), int.Parse( personId ) );
                    }
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
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
            SetEditMode( false );

            if ( hfCompetencyPersonId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the ResidencyPerson Detail page, return to that
                string personId = PageParameter( "personId" );
                if ( !string.IsNullOrWhiteSpace( personId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["personId"] = personId;
                    NavigateToParentPage( qryString );
                }
                else
                {
                    NavigateToParentPage();
                }
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<CompetencyPerson> service = new ResidencyService<CompetencyPerson>();
                CompetencyPerson item = service.Get( hfCompetencyPersonId.ValueAsInt() );
                ShowReadonlyDetails( item );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            ResidencyService<CompetencyPerson> service = new ResidencyService<CompetencyPerson>();
            CompetencyPerson item = service.Get( hfCompetencyPersonId.ValueAsInt() );
            ShowEditDetails( item );
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;

            DimOtherBlocks( editable );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            ResidencyService<CompetencyPerson> competencyPersonService = new ResidencyService<CompetencyPerson>();

            int competencyPersonId = int.Parse( hfCompetencyPersonId.Value );
            int trackId = ddlTrack.SelectedValueAsInt() ?? 0;
            int personId = hfPersonId.ValueAsInt();

            if ( competencyPersonId == 0 )
            {
                int selectedId = ddlCompetency.SelectedValueAsInt() ?? 0;
                List<int> competencyToAssignIdList = null;

                if ( selectedId == Rock.Constants.All.Id )
                {
                    // add all the Competencies for this Track that they don't have yet
                    
                    var competencyQry = new ResidencyService<Competency>().Queryable().Where( a => a.TrackId == trackId );

                    // list 
                    
                    List<int> assignedCompetencyIds = new ResidencyService<CompetencyPerson>().Queryable().Where( a => a.PersonId.Equals( personId ) ).Select( a => a.CompetencyId ).ToList();

                    competencyToAssignIdList = competencyQry.Where( a => !assignedCompetencyIds.Contains( a.Id ) ).OrderBy( a => a.Name ).Select( a => a.Id ).ToList();
                }
                else
                {
                    // just add the selected Competency
                    competencyToAssignIdList.Add( selectedId );
                }

                RockTransactionScope.WrapTransaction( () =>
                    {
                        foreach ( var competencyId in competencyToAssignIdList )
                        {

                            CompetencyPerson competencyPerson = new CompetencyPerson();
                            competencyPersonService.Add( competencyPerson, CurrentPersonId );
                            competencyPerson.PersonId = hfPersonId.ValueAsInt();
                            competencyPerson.CompetencyId = competencyId;

                            competencyPersonService.Save( competencyPerson, CurrentPersonId );

                        }
                    } );
            }
            else
            {
                // shouldn't happen, they can only Add
            }

            var qryParams = new Dictionary<string, string>();
            qryParams["personId"] = personId.ToString();
            NavigateToParentPage( qryParams );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            ShowDetail( itemKey, itemKeyValue, null );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        /// <param name="personId">The person id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? personId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPerson competencyPerson = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                competencyPerson = new ResidencyService<CompetencyPerson>().Get( itemKeyValue );
            }
            else
            {
                competencyPerson = new CompetencyPerson { Id = 0 };
                competencyPerson.PersonId = personId ?? 0;
                competencyPerson.Person = new ResidencyService<Person>().Get( competencyPerson.PersonId );
            }

            hfCompetencyPersonId.Value = competencyPerson.Id.ToString();
            hfPersonId.Value = competencyPerson.PersonId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPerson.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( competencyPerson );
            }
            else
            {
                // don't allow edit once a Competency has been assign
                btnEdit.Visible = false;
                if ( competencyPerson.Id > 0 )
                {
                    ShowReadonlyDetails( competencyPerson );
                }
                else
                {
                    ShowEditDetails( competencyPerson );
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlPeriodTrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlPeriod_SelectedIndexChanged( object sender, EventArgs e )
        {
            int periodId = ddlPeriod.SelectedValueAsInt() ?? 0;
            var trackQry = new ResidencyService<Track>().Queryable().Where( a => a.PeriodId.Equals( periodId ) );

            ddlTrack.DataSource = trackQry.OrderBy( a => a.DisplayOrder ).ThenBy( a => a.Name ).ToList();
            ddlTrack.DataBind();
            ddlTrack_SelectedIndexChanged( null, null );
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlTrack_SelectedIndexChanged( object sender, EventArgs e )
        {
            int trackId = ddlTrack.SelectedValueAsInt() ?? 0;
            var competencyQry = new ResidencyService<Competency>().Queryable().Where( a => a.TrackId == trackId );

            // list 
            int personId = hfPersonId.ValueAsInt();
            List<int> assignedCompetencyIds = new ResidencyService<CompetencyPerson>().Queryable().Where( a => a.PersonId.Equals( personId ) ).Select( a => a.CompetencyId ).ToList();

            var competencyNotYetAssignedList = competencyQry.Where( a => !assignedCompetencyIds.Contains( a.Id ) ).OrderBy( a => a.Name ).ToList();

            competencyNotYetAssignedList.Insert( 0, new Competency { Id = Rock.Constants.All.Id, Name = Rock.Constants.All.Text } );

            ddlCompetency.DataSource = competencyNotYetAssignedList;
            ddlCompetency.DataBind();
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ddlPeriod.DataSource = new ResidencyService<Period>().Queryable().OrderBy( a => a.Name ).ToList();
            ddlPeriod.DataBind();
            ddlPeriod_SelectedIndexChanged( null, null );
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="competencyPerson">The competency person.</param>
        private void ShowEditDetails( CompetencyPerson competencyPerson )
        {
            if ( competencyPerson.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( "Competency for Resident" );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( "Competency to Resident" );
            }

            SetEditMode( true );

            LoadDropDowns();

            lblPersonName.Text = competencyPerson.Person.FullName;

            ddlCompetency.SetValue( competencyPerson.CompetencyId );

            if ( competencyPerson.Competency != null )
            {
                lblPeriod.Text = competencyPerson.Competency.Track.Period.Name;
                lblTrack.Text = competencyPerson.Competency.Track.Name;
                lblCompetency.Text = competencyPerson.Competency.Name;
            }
            else
            {
                // shouldn't happen, but just in case
                lblCompetency.Text = Rock.Constants.None.Text;
            }

            // only allow a Competency to be assigned when in Add mode
            pnlCompetencyLabels.Visible = competencyPerson.Id != 0;
            pnlCompetencyDropDownLists.Visible = competencyPerson.Id == 0;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competencyPerson">The competency person.</param>
        private void ShowReadonlyDetails( CompetencyPerson competencyPerson )
        {
            SetEditMode( false );

            string residentDetailPageGuid = this.GetAttributeValue( "ResidentDetailPage" );
            string residentHtml = competencyPerson.Person.FullName;
            if ( !string.IsNullOrWhiteSpace( residentDetailPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentDetailPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "personId", competencyPerson.PersonId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                residentHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, competencyPerson.Person.FullName );
            }

            lblMainDetails.Text = new DescriptionList()
                .Add( "Resident", residentHtml )
                .Add( "Competency", competencyPerson.Competency.Name )
                .Html;
        }

        #endregion
    }
}