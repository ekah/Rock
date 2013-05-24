//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock;
using System.Web.UI.WebControls;

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
            var list = service.Queryable().OrderBy( a => a.ResidencyPeriod.Name ).ThenBy( a => a.Name) .ToList();
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
            NavigateToParentPage();
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

            int residencyCompetencyId = int.Parse( hfResidencyCompetencyId.Value ); ;

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
            residencyCompetency.CreditHours = tbCreditHours.Text.AsInteger(false);
            residencyCompetency.SupervisionHours = tbSupervisionHours.Text.AsInteger( false );
            residencyCompetency.ImplementationHours = tbImplementationHours.Text.AsInteger( false );

            // check for duplicates within Period
            if ( residencyCompetencyService.Queryable().Count( a => a.Name.Equals( residencyCompetency.Name, StringComparison.OrdinalIgnoreCase ) && a.ResidencyTrackId.Equals(residencyCompetency.ResidencyTrackId) && !a.Id.Equals( residencyCompetency.Id )  ) > 0 )
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

            NavigateToParentPage();
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
                lActionTitle.Text = ActionTitle.Edit( ResidencyCompetency.FriendlyTypeName );
            }
            else
            {
                residencyCompetency = new ResidencyCompetency { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( ResidencyCompetency.FriendlyTypeName );
            }

            hfResidencyCompetencyId.Value = residencyCompetency.Id.ToString();

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
                lActionTitle.Text = ActionTitle.View( ResidencyCompetency.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            tbName.ReadOnly = readOnly;
            tbDescription.ReadOnly = readOnly;
            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}