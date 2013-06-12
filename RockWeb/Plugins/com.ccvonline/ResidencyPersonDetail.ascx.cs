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
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyPersonDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "personId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "personId", int.Parse( itemId ) );
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

            if ( hfPersonId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                NavigateToParentPage();

            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ResidencyService<Person> service = new ResidencyService<Person>();
                Person item = service.Get( hfPersonId.ValueAsInt() );
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
            ResidencyService<Person> service = new ResidencyService<Person>();
            Person item = service.Get( hfPersonId.ValueAsInt() );
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

            Person person;
            ResidencyService<Person> personService = new ResidencyService<Person>();

            int personId = int.Parse( hfPersonId.Value );

            if ( personId == 0 )
            {
                person = new Person();
                personService.Add( person, CurrentPersonId );
            }
            else
            {
                person = personService.Get( personId );
            }

            /*
            person.Name = tbName.Text;
            person.Description = tbDescription.Text;
            person.StartDate = dpStartDate.SelectedDate;
            person.EndDate = dpEndDate.SelectedDate;

            // check for duplicates
            if ( residencyPersonService.Queryable().Count( a => a.Name.Equals( person.Name, StringComparison.OrdinalIgnoreCase ) && !a.Id.Equals( person.Id ) ) > 0 )
            {
                nbWarningMessage.Text = WarningMessage.DuplicateFoundMessage( "name", Person.FriendlyTypeName );
                return;
            }

            if ( !person.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyPersonService.Save( person, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["personId"] = person.Id.ToString();
            NavigateToPage( this.CurrentPage.Guid, qryParams );
             
             */ 
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "personId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            Person person = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                person = new ResidencyService<Person>().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( Person.FriendlyTypeName );
            }
            else
            {
                person = new Person { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( Person.FriendlyTypeName );
            }

            hfPersonId.Value = person.Id.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Person.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( person );
            }
            else
            {
                btnEdit.Visible = true;
                if ( person.Id > 0 )
                {
                    ShowReadonlyDetails( person );
                }
                else
                {
                    ShowEditDetails( person );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="person">The person.</param>
        private void ShowEditDetails( Person person )
        {
            if ( person.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Person.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Person.FriendlyTypeName );
            }

            SetEditMode( true );

            ppPerson.SetValue( person );

            /*
            tbName.Text = ResidencyPerson.Name;
            tbDescription.Text = ResidencyPerson.Description;
            dpStartDate.SelectedDate = ResidencyPerson.StartDate;
            dpEndDate.SelectedDate = ResidencyPerson.EndDate;
             */ 
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="person">The person.</param>
        private void ShowReadonlyDetails( Person person )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Name", person.FullName );
            
            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}