//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Blocks.Crm
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GroupMemberDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string groupId = PageParameter( "groupId" );
                string groupMemberId = PageParameter( "groupMemberId" );
                if ( !string.IsNullOrWhiteSpace( groupMemberId ) )
                {
                    if ( string.IsNullOrWhiteSpace( groupId ) )
                    {
                        ShowDetail( "groupMemberId", int.Parse( groupMemberId ) );
                    }
                    else
                    {
                        ShowDetail( "groupMemberId", int.Parse( groupMemberId ), int.Parse( groupId ) );
                    }
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }

            var groupMember = new GroupMember { GroupId = hfGroupId.ValueAsInt() };
            groupMember.LoadAttributes();
            phAttributes.Controls.Clear();
            Rock.Attribute.Helper.AddEditControls( groupMember, phAttributes, false );
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            int groupId = hfGroupId.ValueAsInt();
            Group group = new GroupService().Get( groupId );
            if ( group != null )
            {
                ddlGroupRole.DataSource = group.GroupType.Roles.OrderBy( a => a.SortOrder ).ToList();
                ddlGroupRole.DataBind();
            }
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
            pnlDetails.Visible = false;
            if ( !itemKey.Equals( "groupMemberId" ) )
            {
                return;
            }

            GroupMember groupMember = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                groupMember = new GroupMemberService().Get( itemKeyValue );
                groupMember.LoadAttributes();
            }
            else
            {
                // only create a new one if parent was specified
                if ( groupId != null )
                {
                    groupMember = new GroupMember { Id = 0 };
                    groupMember.GroupId = groupId.Value;
                    groupMember.Group = new GroupService().Get( groupMember.GroupId );
                    groupMember.GroupRoleId = groupMember.Group.GroupType.DefaultGroupRoleId ?? 0;
                }
            }

            if ( groupMember == null )
            {
                return;
            }

            pnlDetails.Visible = true;
            hfGroupId.Value = groupMember.GroupId.ToString();
            hfGroupMemberId.Value = groupMember.Id.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Group.FriendlyTypeName );
            }

            if ( groupMember.IsSystem )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlySystem( Group.FriendlyTypeName );
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
            if ( groupMember.Id.Equals( 0 ) )
            {
                lActionTitle.Text = ActionTitle.Add( "Group Member to " + groupMember.Group.Name );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Edit( "Group Member for " + groupMember.Group.Name );
            }

            SetEditMode( true );

            LoadDropDowns();

            ppGroupMemberPerson.PersonId = groupMember.PersonId.ToString();
            ppGroupMemberPerson.PersonName = groupMember.Person != null ? groupMember.Person.FullName : None.TextHtml;
            ddlGroupRole.SetValue( groupMember.GroupRoleId );

            phAttributes.Controls.Clear();
            groupMember.LoadAttributes();
            Rock.Attribute.Helper.AddEditControls( groupMember, phAttributes, true );
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="groupMember">The group member.</param>
        private void ShowReadonlyDetails( GroupMember groupMember )
        {
            SetEditMode( false );

            string groupIconHtml = string.Empty;
            var group = groupMember.Group;
            if ( !string.IsNullOrWhiteSpace( group.GroupType.IconCssClass ) )
            {
                groupIconHtml = string.Format( "<i class='{0} icon-large' ></i>", group.GroupType.IconCssClass );
            }
            else
            {
                var appPath = System.Web.VirtualPathUtility.ToAbsolute( "~" );
                string imageUrlFormat = "<img src='" + appPath + "Image.ashx?id={0}&width=50&height=50' />";
                if ( group.GroupType.IconLargeFileId != null )
                {
                    groupIconHtml = string.Format( imageUrlFormat, group.GroupType.IconLargeFileId );
                }
                else if ( group.GroupType.IconSmallFileId != null )
                {
                    groupIconHtml = string.Format( imageUrlFormat, group.GroupType.IconSmallFileId );
                }
            }

            hfGroupId.SetValue( group.Id );
            hfGroupMemberId.SetValue( groupMember.Id );

            lGroupIconHtml.Text = groupIconHtml;
            lReadOnlyTitle.Text = groupMember.Person.FullName;

            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div>
    <dl>";
            lblMainDetails.Text += string.Format( descriptionFormat, "Group Member", groupMember.Person.FullName );
            lblMainDetails.Text += string.Format( descriptionFormat, "Member's Role", groupMember.GroupRole.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Group Name", group.Name );
            lblMainDetails.Text += string.Format( descriptionFormat, "Group Description", group.Description );
            lblMainDetails.Text += @"
    </dl>
</div>";

            groupMember.LoadAttributes();
            Rock.Attribute.Helper.AddDisplayControls( groupMember, phGroupMemberAttributesReadOnly );
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            GroupMemberService groupMemberService = new GroupMemberService();
            GroupMember groupMember = groupMemberService.Get( int.Parse( hfGroupMemberId.Value ) );
            ShowEditDetails( groupMember );
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;

            this.DimOtherBlocks( editable );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            int groupMemberId = int.Parse( hfGroupMemberId.Value );
            GroupMemberService groupMemberService = new GroupMemberService();

            GroupMember groupMember;
            if ( groupMemberId.Equals( 0 ) )
            {
                groupMember = new GroupMember { Id = 0 };
                groupMember.GroupId = hfGroupId.ValueAsInt();
            }
            else
            {
                groupMember = groupMemberService.Get( groupMemberId );
            }

            groupMember.PersonId = int.Parse( ppGroupMemberPerson.PersonId );
            groupMember.GroupRoleId = ddlGroupRole.SelectedValueAsInt() ?? 0;

            groupMember.LoadAttributes();

            Rock.Attribute.Helper.GetEditValues( phAttributes, groupMember );
            Rock.Attribute.Helper.SetErrorIndicators( phAttributes, groupMember );

            if ( !Page.IsValid )
            {
                return;
            }

            if ( !groupMember.IsValid )
            {
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                if ( groupMember.Id.Equals( 0 ) )
                {
                    groupMemberService.Add( groupMember, CurrentPersonId );
                }

                groupMemberService.Save( groupMember, CurrentPersonId );
                Rock.Attribute.Helper.SaveAttributeValues( groupMember, CurrentPersonId );
            } );

            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["groupId"] = hfGroupId.Value;
            NavigateToParentPage( qryString );
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            if ( hfGroupMemberId.Value.Equals( "0" ) )
            {
                // Cancelling on Add.  Return to Grid
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["groupId"] = hfGroupId.Value;
                NavigateToParentPage( qryString );
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                GroupMemberService groupMemberService = new GroupMemberService();
                GroupMember groupMember = groupMemberService.Get( int.Parse( hfGroupMemberId.Value ) );
                ShowReadonlyDetails( groupMember );
            }
        }

        #endregion
    }
}