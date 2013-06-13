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
                string itemId = PageParameter( "groupMemberId" );
                string groupId = PageParameter( "groupId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( groupId ) )
                    {
                        ShowDetail( "groupMemberId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "groupMemberId", int.Parse( itemId ), int.Parse( groupId ) );
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

            if ( hfGroupMemberId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the Group Detail page, return to that
                string groupId = PageParameter( "groupId" );
                if ( !string.IsNullOrWhiteSpace( groupId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["groupId"] = groupId;
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
                ResidencyService<GroupMember> service = new ResidencyService<GroupMember>();
                GroupMember item = service.Get( hfGroupMemberId.ValueAsInt() );
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
            ResidencyService<GroupMember> service = new ResidencyService<GroupMember>();
            GroupMember item = service.Get( hfGroupMemberId.ValueAsInt() );
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
            GroupMember groupMember;
            ResidencyService<GroupMember> groupMemberService = new ResidencyService<GroupMember>();

            int groupMemberId = hfGroupMemberId.ValueAsInt();

            if ( groupMemberId == 0 )
            {
                groupMember = new GroupMember();
                groupMemberService.Add( groupMember, CurrentPersonId );
                groupMember.GroupId = hfGroupId.ValueAsInt();
                var group = new ResidencyService<Group>().Get( groupMember.GroupId );
                groupMember.GroupRoleId = group.GroupType.DefaultGroupRoleId ?? 0;
            }
            else
            {
                groupMember = groupMemberService.Get( groupMemberId );
            }

            groupMember.PersonId = ppPerson.SelectedValue ?? 0;

            if ( !groupMember.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                groupMemberService.Save( groupMember, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["groupMemberId"] = groupMember.Id.ToString();
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
        /// <param name="groupId">The group id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? groupId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "groupMemberId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            GroupMember groupMember = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                groupMember = new ResidencyService<GroupMember>().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( GroupMember.FriendlyTypeName );
            }
            else
            {
                groupMember = new GroupMember { Id = 0, GroupId = groupId ?? 0 };
                lActionTitle.Text = ActionTitle.Add( GroupMember.FriendlyTypeName );
            }

            hfGroupId.SetValue( groupMember.GroupId );
            hfGroupMemberId.SetValue( groupMember.Id );

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( GroupMember.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( groupMember );
            }
            else
            {
                btnEdit.Visible = true;
                if ( groupMember.Id > 0 )
                {
                    ShowReadonlyDetails( groupMember );
                }
                else
                {
                    ShowEditDetails( groupMember );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="groupMember">The group member.</param>
        private void ShowEditDetails( GroupMember groupMember )
        {
            if ( groupMember.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( GroupMember.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( GroupMember.FriendlyTypeName );
            }

            SetEditMode( true );
            ppPerson.SetValue( groupMember.Person );
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="groupMember">The group member.</param>
        private void ShowReadonlyDetails( GroupMember groupMember )
        {
            SetEditMode( false );

            // make a Description section for nonEdit mode
            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div class='span6'>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Group", groupMember.Group.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Name", groupMember.Person.FullName );

            lblMainDetails.Text += @"
    </dl>
</div>";
        }

        #endregion
    }
}