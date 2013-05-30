using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyCompetencyDetail : RockBlock, IDetailBlock
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
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "residencyCompetencyId", int.Parse( itemId ) );
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
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ResidencyService<ResidencyTrack> service = new ResidencyService<ResidencyTrack>();
            var list = service.Queryable().OrderBy( a => a.ResidencyPeriod.Name ).ThenBy( a => a.Name ).ToList();
            ddlTrack.Items.Clear();
            foreach ( var item in list )
            {
                ddlTrack.Items.Add( new ListItem( string.Format( "{1} - {0}", item.Name, item.ResidencyPeriod.Name ), item.Id.ToString() ) );
            }
        }

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
                NavigateToParentPage();
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<ResidencyCompetency> service = new ResidencyService<ResidencyCompetency>();
                ResidencyCompetency item = service.Get( hfResidencyCompetencyId.ValueAsInt() );
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
            ResidencyService<ResidencyCompetency> service = new ResidencyService<ResidencyCompetency>();
            ResidencyCompetency item = service.Get( hfResidencyCompetencyId.ValueAsInt() );
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
            ResidencyCompetency residencyCompetency;
            ResidencyService<ResidencyCompetency> residencyCompetencyService = new ResidencyService<ResidencyCompetency>();

            int residencyCompetencyId = int.Parse( hfResidencyCompetencyId.Value );

            if ( residencyCompetencyId == 0 )
            {
                residencyCompetency = new ResidencyCompetency();
                residencyCompetencyService.Add( residencyCompetency, CurrentPersonId );
            }
            else
            {
                residencyCompetency = residencyCompetencyService.Get( residencyCompetencyId );
            }

            residencyCompetency.Name = tbName.Text;
            residencyCompetency.Description = tbDescription.Text;
            residencyCompetency.ResidencyTrackId = ddlTrack.SelectedValueAsInt().Value;
            residencyCompetency.TeacherOfRecordPersonId = ppTeacherOfRecord.PersonId;
            residencyCompetency.FacilitatorPersonId = ppFacilitator.PersonId;
            residencyCompetency.Goals = tbGoals.Text;
            residencyCompetency.CreditHours = tbCreditHours.Text.AsInteger( false );
            residencyCompetency.SupervisionHours = tbSupervisionHours.Text.AsInteger( false );
            residencyCompetency.ImplementationHours = tbImplementationHours.Text.AsInteger( false );

            // check for duplicates within Period
            if ( residencyCompetencyService.Queryable().Count( a => a.Name.Equals( residencyCompetency.Name, StringComparison.OrdinalIgnoreCase ) && a.ResidencyTrackId.Equals( residencyCompetency.ResidencyTrackId ) && !a.Id.Equals( residencyCompetency.Id ) ) > 0 )
            {
                nbWarningMessage.Text = WarningMessage.DuplicateFoundMessage( "name", ResidencyCompetency.FriendlyTypeName );
                return;
            }

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
            // return if unexpected itemKey 
            if ( itemKey != "residencyCompetencyId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            ResidencyCompetency residencyCompetency = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyCompetency = new ResidencyService<ResidencyCompetency>().Get( itemKeyValue );
            }
            else
            {
                residencyCompetency = new ResidencyCompetency { Id = 0 };
            }

            hfResidencyCompetencyId.Value = residencyCompetency.Id.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ResidencyCompetency.FriendlyTypeName );
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
        private void ShowEditDetails( ResidencyCompetency residencyCompetency )
        {
            if ( residencyCompetency.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( ResidencyCompetency.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( ResidencyCompetency.FriendlyTypeName );
            }

            SetEditMode( true );

            LoadDropDowns();

            tbName.Text = residencyCompetency.Name;
            tbDescription.Text = residencyCompetency.Description;
            ddlTrack.SetValue( residencyCompetency.ResidencyTrackId );
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
        private void ShowReadonlyDetails( ResidencyCompetency residencyCompetency )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Name", residencyCompetency.Name );

            lblMainDetails.Text += string.Format( descriptionFormat, "Period", residencyCompetency.ResidencyTrack.ResidencyPeriod.Name );

            lblMainDetails.Text += string.Format( descriptionFormat, "Track", residencyCompetency.ResidencyTrack.Name );

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