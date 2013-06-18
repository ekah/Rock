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

namespace RockWeb.Plugins.com.ccvonline.Residency
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
                string itemId = PageParameter( "residencyCompetencyPersonId" );
                string personId = PageParameter( "personId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( personId ) )
                    {
                        ShowDetail( "residencyCompetencyPersonId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyCompetencyPersonId", int.Parse( itemId ), int.Parse( personId ) );
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

            if ( hfResidencyCompetencyPersonId.ValueAsInt().Equals( 0 ) )
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
                CompetencyPerson item = service.Get( hfResidencyCompetencyPersonId.ValueAsInt() );
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
            CompetencyPerson item = service.Get( hfResidencyCompetencyPersonId.ValueAsInt() );
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
            CompetencyPerson residencyCompetencyPerson;
            ResidencyService<CompetencyPerson> residencyCompetencyPersonService = new ResidencyService<CompetencyPerson>();

            int residencyCompetencyPersonId = int.Parse( hfResidencyCompetencyPersonId.Value );

            if ( residencyCompetencyPersonId == 0 )
            {
                residencyCompetencyPerson = new CompetencyPerson();
                residencyCompetencyPersonService.Add( residencyCompetencyPerson, CurrentPersonId );
                
                // These inputs are only editable on Add
                residencyCompetencyPerson.PersonId = hfPersonId.ValueAsInt();
                residencyCompetencyPerson.CompetencyId = ddlResidencyCompetency.SelectedValueAsInt() ?? 0;
            }
            else
            {
                residencyCompetencyPerson = residencyCompetencyPersonService.Get( residencyCompetencyPersonId );
            }

            if ( !residencyCompetencyPerson.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyCompetencyPersonService.Save( residencyCompetencyPerson, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyCompetencyPersonId"] = residencyCompetencyPerson.Id.ToString();
            NavigateToPage( this.CurrentPage.Guid, qryParams );
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
            if ( itemKey != "residencyCompetencyPersonId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPerson residencyCompetencyPerson = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetencyPerson = new ResidencyService<CompetencyPerson>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetencyPerson = new CompetencyPerson { Id = 0 };
                residencyCompetencyPerson.PersonId = personId ?? 0;
                residencyCompetencyPerson.Person = new ResidencyService<Person>().Get( residencyCompetencyPerson.PersonId );
            }

            hfResidencyCompetencyPersonId.Value = residencyCompetencyPerson.Id.ToString();
            hfPersonId.Value = residencyCompetencyPerson.PersonId.ToString();

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
                ShowReadonlyDetails( residencyCompetencyPerson );
            }
            else
            {
                // don't allow edit once a Competency has been assign
                btnEdit.Visible = false;
                if ( residencyCompetencyPerson.Id > 0 )
                {
                    ShowReadonlyDetails( residencyCompetencyPerson );
                }
                else
                {
                    ShowEditDetails( residencyCompetencyPerson );
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlResidencyPeriodTrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlResidencyPeriod_SelectedIndexChanged( object sender, EventArgs e )
        {
            int residencyPeriodId = ddlResidencyPeriod.SelectedValueAsInt() ?? 0;
            var residencyTrackQry = new ResidencyService<Track>().Queryable().Where( a => a.PeriodId.Equals( residencyPeriodId ) );

            ddlResidencyTrack.DataSource = residencyTrackQry.OrderBy( a => a.Name ).ToList();
            ddlResidencyTrack.DataBind();
            ddlResidencyTrack_SelectedIndexChanged( null, null );
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlResidencyTrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlResidencyTrack_SelectedIndexChanged( object sender, EventArgs e )
        {
            int residencyTrackId = ddlResidencyTrack.SelectedValueAsInt() ?? 0;
            var residencyCompetencyQry = new ResidencyService<Competency>().Queryable().Where( a => a.TrackId == residencyTrackId );

            // list 
            int personId = hfPersonId.ValueAsInt();
            List<int> assignedCompetencyIds = new ResidencyService<CompetencyPerson>().Queryable().Where( a => a.PersonId.Equals( personId ) ).Select( a => a.CompetencyId ).ToList();

            ddlResidencyCompetency.DataSource = residencyCompetencyQry.Where( a => !assignedCompetencyIds.Contains( a.Id ) ).OrderBy( a => a.Name ).ToList();
            ddlResidencyCompetency.DataBind();
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ddlResidencyPeriod.DataSource = new ResidencyService<Period>().Queryable().OrderBy( a => a.Name ).ToList();
            ddlResidencyPeriod.DataBind();
            ddlResidencyPeriod_SelectedIndexChanged( null, null );
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyCompetencyPerson">The residency project.</param>
        private void ShowEditDetails( CompetencyPerson residencyCompetencyPerson )
        {
            if ( residencyCompetencyPerson.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPerson.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPerson.FriendlyTypeName );
            }

            SetEditMode( true );

            LoadDropDowns();

            lblPersonName.Text = residencyCompetencyPerson.Person.FullName;
            
            ddlResidencyCompetency.SetValue( residencyCompetencyPerson.CompetencyId );
            
            if ( residencyCompetencyPerson.Competency != null )
            {
                lblResidencyPeriod.Text = residencyCompetencyPerson.Competency.Track.Period.Name;
                lblResidencyTrack.Text = residencyCompetencyPerson.Competency.Track.Name;
                lblResidencyCompetency.Text = residencyCompetencyPerson.Competency.Name;
            }
            else
            {
                // shouldn't happen, but just in case
                lblResidencyCompetency.Text = Rock.Constants.None.Text;
            }

            // only allow a Competency to be assigned when in Add mode
            pnlCompetencyLabels.Visible = ( residencyCompetencyPerson.Id != 0 );
            pnlCompetencyDropDownLists.Visible = ( residencyCompetencyPerson.Id == 0 );
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyCompetencyPerson">The residency project.</param>
        private void ShowReadonlyDetails( CompetencyPerson residencyCompetencyPerson )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            

            string residentDetailPageGuid = this.GetAttributeValue( "ResidentDetailPage" );
            string residentHtml = residencyCompetencyPerson.Person.FullName;
            if ( !string.IsNullOrWhiteSpace( residentDetailPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residentDetailPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "personId", residencyCompetencyPerson.PersonId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                residentHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyCompetencyPerson.Person.FullName );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Resident",  residentHtml);
            lblMainDetails.Text += string.Format( descriptionFormat, "Competency", residencyCompetencyPerson.Competency.Name );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
        
}
}