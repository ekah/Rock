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
    /// 
    /// </summary>
    [LinkedPage( "Residency Track Page" )]
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
                string itemId = PageParameter( "residencyCompetencyId" );
                string residencyTrackId = PageParameter( "residencyTrackId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( residencyTrackId ) )
                    {
                        ShowDetail( "residencyCompetencyId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "residencyCompetencyId", int.Parse( itemId ), int.Parse( residencyTrackId ) );
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

            if ( hfResidencyCompetencyId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the ResidencyTrack Detail page, return to that
                string residencyTrackId = PageParameter( "residencyTrackId" );
                if ( !string.IsNullOrWhiteSpace( residencyTrackId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["residencyTrackId"] = residencyTrackId;
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
                Competency item = service.Get( hfResidencyCompetencyId.ValueAsInt() );
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
            Competency item = service.Get( hfResidencyCompetencyId.ValueAsInt() );
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
            Competency residencyCompetency;
            ResidencyService<Competency> residencyCompetencyService = new ResidencyService<Competency>();

            int residencyCompetencyId = int.Parse( hfResidencyCompetencyId.Value );

            if ( residencyCompetencyId == 0 )
            {
                residencyCompetency = new Competency();
                residencyCompetencyService.Add( residencyCompetency, CurrentPersonId );
            }
            else
            {
                residencyCompetency = residencyCompetencyService.Get( residencyCompetencyId );
            }

            residencyCompetency.Name = tbName.Text;
            residencyCompetency.Description = tbDescription.Text;
            residencyCompetency.TrackId = hfResidencyTrackId.ValueAsInt();
            residencyCompetency.TeacherOfRecordPersonId = ppTeacherOfRecord.PersonId;
            residencyCompetency.FacilitatorPersonId = ppFacilitator.PersonId;
            residencyCompetency.Goals = tbGoals.Text;
            residencyCompetency.CreditHours = tbCreditHours.Text.AsInteger( false );
            residencyCompetency.SupervisionHours = tbSupervisionHours.Text.AsInteger( false );
            residencyCompetency.ImplementationHours = tbImplementationHours.Text.AsInteger( false );

            if ( !residencyCompetency.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyCompetencyService.Save( residencyCompetency, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["residencyCompetencyId"] = residencyCompetency.Id.ToString();
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
        /// <param name="residencyTrackId">The residency track id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyTrackId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyCompetencyId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            Competency residencyCompetency = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetency = new ResidencyService<Competency>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetency = new Competency { Id = 0 };
                residencyCompetency.TrackId = residencyTrackId ?? 0;
                residencyCompetency.Track = new ResidencyService<Track>().Get( residencyCompetency.TrackId );
            }

            hfResidencyCompetencyId.Value = residencyCompetency.Id.ToString();
            hfResidencyTrackId.Value = residencyCompetency.TrackId.ToString();

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
                ShowReadonlyDetails( residencyCompetency );
            }
            else
            {
                btnEdit.Visible = true;
                if ( residencyCompetency.Id > 0 )
                {
                    ShowReadonlyDetails( residencyCompetency );
                }
                else
                {
                    ShowEditDetails( residencyCompetency );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="residencyCompetency">The residency competency.</param>
        private void ShowEditDetails( Competency residencyCompetency )
        {
            if ( residencyCompetency.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Competency.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Competency.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = residencyCompetency.Name;
            tbDescription.Text = residencyCompetency.Description;
            lblPeriod.Text = residencyCompetency.Track.Period.Name;
            lblTrack.Text = residencyCompetency.Track.Name;
            ppTeacherOfRecord.SetValue( residencyCompetency.TeacherOfRecordPerson );
            ppFacilitator.SetValue( residencyCompetency.FacilitatorPerson );
            tbGoals.Text = residencyCompetency.Goals;
            tbCreditHours.Text = residencyCompetency.CreditHours.ToString();
            tbSupervisionHours.Text = residencyCompetency.SupervisionHours.ToString();
            tbImplementationHours.Text = residencyCompetency.ImplementationHours.ToString();
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="residencyCompetency">The residency competency.</param>
        private void ShowReadonlyDetails( Competency residencyCompetency )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Name", residencyCompetency.Name );

            lblMainDetails.Text += string.Format( descriptionFormat, "Period", residencyCompetency.Track.Period.Name );

            string residencyTrackPageGuid = this.GetAttributeValue( "ResidencyTrackPage" );
            string trackHtml = residencyCompetency.Track.Name;
            if ( !string.IsNullOrWhiteSpace( residencyTrackPageGuid ) )
            {
                var page = new PageService().Get( new Guid( residencyTrackPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "residencyTrackId", residencyCompetency.TrackId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                trackHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, residencyCompetency.Track.Name );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Track", trackHtml );

            if ( !string.IsNullOrWhiteSpace( residencyCompetency.Description ) )
            {
                lblMainDetails.Text += string.Format( descriptionFormat, "Description", residencyCompetency.Description );
            }

            if ( residencyCompetency.TeacherOfRecordPerson != null )
            {
                lblMainDetails.Text += string.Format( descriptionFormat, "Teacher of Record", residencyCompetency.TeacherOfRecordPerson.FullName );
            }

            if ( residencyCompetency.FacilitatorPerson != null )
            {
                lblMainDetails.Text += string.Format( descriptionFormat, "Facilitator", residencyCompetency.FacilitatorPerson.FullName );
            }

            lblMainDetails.Text += string.Format( descriptionFormat, "Credit Hours", residencyCompetency.CreditHours.ToString() );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}