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
    /// 
    /// </summary>
    public partial class CompetencyDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "competencyId" );
                string trackId = PageParameter( "trackId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( trackId ) )
                    {
                        ShowDetail( "competencyId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "competencyId", int.Parse( itemId ), int.Parse( trackId ) );
                    }
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns></returns>
        public override List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();

            int? competencyId = this.PageParameter(pageReference, "competencyId" ).AsInteger();
            if ( competencyId != null )
            {
                Competency competency = new ResidencyService<Competency>().Get( competencyId.Value );
                if ( competency != null )
                {
                    breadCrumbs.Add( new BreadCrumb( competency.Name, pageReference ) );
                }
                else
                {
                    breadCrumbs.Add( new BreadCrumb( "Competency", pageReference ) );
                }
            }
            else
            {
                // don't show a breadcrumb if we don't have a pageparam to work with
            }

            return breadCrumbs;
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

            if ( hfCompetencyId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the Track Detail page, return to that
                string trackId = PageParameter( "trackId" );
                if ( !string.IsNullOrWhiteSpace( trackId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["trackId"] = trackId;
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
                ResidencyService<Competency> service = new ResidencyService<Competency>();
                Competency item = service.Get( hfCompetencyId.ValueAsInt() );
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
            ResidencyService<Competency> service = new ResidencyService<Competency>();
            Competency item = service.Get( hfCompetencyId.ValueAsInt() );
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
            Competency competency;
            ResidencyService<Competency> competencyService = new ResidencyService<Competency>();

            int competencyId = int.Parse( hfCompetencyId.Value );

            if ( competencyId == 0 )
            {
                competency = new Competency();
                competencyService.Add( competency, CurrentPersonId );
            }
            else
            {
                competency = competencyService.Get( competencyId );
            }

            competency.Name = tbName.Text;
            competency.Description = tbDescription.Text;
            competency.TrackId = hfTrackId.ValueAsInt();
            competency.TeacherOfRecordPersonId = ppTeacherOfRecord.PersonId;
            competency.FacilitatorPersonId = ppFacilitator.PersonId;
            competency.Goals = tbGoals.Text;
            competency.CreditHours = tbCreditHours.Text.AsInteger( false );
            competency.SupervisionHours = tbSupervisionHours.Text.AsInteger( false );
            competency.ImplementationHours = tbImplementationHours.Text.AsInteger( false );

            if ( !competency.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                competencyService.Save( competency, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["competencyId"] = competency.Id.ToString();
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
        /// <param name="trackId">The residency track id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? trackId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            Competency competency = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                competency = new ResidencyService<Competency>().Get( itemKeyValue );
            }
            else
            {
                competency = new Competency { Id = 0 };
                competency.TrackId = trackId ?? 0;
                competency.Track = new ResidencyService<Track>().Get( competency.TrackId );
            }

            hfCompetencyId.Value = competency.Id.ToString();
            hfTrackId.Value = competency.TrackId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Competency.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( competency );
            }
            else
            {
                btnEdit.Visible = true;
                if ( competency.Id > 0 )
                {
                    ShowReadonlyDetails( competency );
                }
                else
                {
                    ShowEditDetails( competency );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="competency">The residency competency.</param>
        private void ShowEditDetails( Competency competency )
        {
            if ( competency.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Competency.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Competency.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = competency.Name;
            tbDescription.Text = competency.Description;
            lblPeriod.Text = competency.Track.Period.Name;
            lblTrack.Text = competency.Track.Name;
            ppTeacherOfRecord.SetValue( competency.TeacherOfRecordPerson );
            ppFacilitator.SetValue( competency.FacilitatorPerson );
            tbGoals.Text = competency.Goals;
            tbCreditHours.Text = competency.CreditHours.ToString();
            tbSupervisionHours.Text = competency.SupervisionHours.ToString();
            tbImplementationHours.Text = competency.ImplementationHours.ToString();
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competency">The residency competency.</param>
        private void ShowReadonlyDetails( Competency competency )
        {
            SetEditMode( false );

            lblMainDetails.Text = new DescriptionList()
                .Add( "Name", competency.Name )
                .Add( "Description", competency.Description )
                .Add( "Period", competency.Track.Period.Name )
                .Add( "Track", competency.Track.Name )
                .StartSecondColumn()
                .Add( "Teacher of Record", competency.TeacherOfRecordPerson )
                .Add( "Facilitator", competency.FacilitatorPerson )
                .Add( "Credit Hours", competency.CreditHours )
                .Html;
        }

        #endregion
    }
}