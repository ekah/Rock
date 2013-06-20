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
    /// 
    /// </summary>
    [LinkedPage( "Residency Competency Person Page" )]
    public partial class CompetencyPersonProjectDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "competencyPersonProjectId" );
                string competencyPersonId = PageParameter( "competencyPersonId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( competencyPersonId ) )
                    {
                        ShowDetail( "competencyPersonProjectId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "competencyPersonProjectId", int.Parse( itemId ), int.Parse( competencyPersonId ) );
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

            if ( hfCompetencyPersonProjectId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the CompetencyPerson Detail page, return to that
                string competencyPersonId = PageParameter( "competencyPersonId" );
                if ( !string.IsNullOrWhiteSpace( competencyPersonId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["competencyPersonId"] = competencyPersonId;
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
                ResidencyService<CompetencyPersonProject> service = new ResidencyService<CompetencyPersonProject>();
                CompetencyPersonProject item = service.Get( hfCompetencyPersonProjectId.ValueAsInt() );
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
            ResidencyService<CompetencyPersonProject> service = new ResidencyService<CompetencyPersonProject>();
            CompetencyPersonProject item = service.Get( hfCompetencyPersonProjectId.ValueAsInt() );
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
            CompetencyPersonProject competencyPersonProject;
            ResidencyService<CompetencyPersonProject> competencyPersonProjectService = new ResidencyService<CompetencyPersonProject>();

            int competencyPersonProjectId = int.Parse( hfCompetencyPersonProjectId.Value );

            if ( competencyPersonProjectId == 0 )
            {
                competencyPersonProject = new CompetencyPersonProject();
                competencyPersonProjectService.Add( competencyPersonProject, CurrentPersonId );

                // these inputs are only editable on Add
                competencyPersonProject.ProjectId = ddlProject.SelectedValueAsInt() ?? 0;
                competencyPersonProject.CompetencyPersonId = hfCompetencyPersonId.ValueAsInt();
            }
            else
            {
                competencyPersonProject = competencyPersonProjectService.Get( competencyPersonProjectId );
            }

            if ( !competencyPersonProject.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                competencyPersonProjectService.Save( competencyPersonProject, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["competencyPersonProjectId"] = competencyPersonProject.Id.ToString();
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
        /// <param name="competencyPersonId">The competency person id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? competencyPersonId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "competencyPersonProjectId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            CompetencyPersonProject competencyPersonProject = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                competencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( itemKeyValue );
            }
            else
            {
                competencyPersonProject = new CompetencyPersonProject { Id = 0 };
                competencyPersonProject.CompetencyPersonId = competencyPersonId ?? 0;
                competencyPersonProject.CompetencyPerson = new ResidencyService<CompetencyPerson>().Get( competencyPersonProject.CompetencyPersonId );
            }

            hfCompetencyPersonProjectId.Value = competencyPersonProject.Id.ToString();
            hfCompetencyPersonId.Value = competencyPersonProject.CompetencyPersonId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( CompetencyPersonProject.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( competencyPersonProject );
            }
            else
            {
                btnEdit.Visible = true;
                if ( competencyPersonProject.Id > 0 )
                {
                    ShowReadonlyDetails( competencyPersonProject );
                }
                else
                {
                    ShowEditDetails( competencyPersonProject );
                }
            }
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            var projectQry = new ResidencyService<Project>().Queryable();

            int competencyPersonId = hfCompetencyPersonId.ValueAsInt();
            CompetencyPerson competencyPerson = new ResidencyService<CompetencyPerson>().Get( competencyPersonId );
            projectQry = projectQry.Where( a => a.CompetencyId == competencyPerson.CompetencyId );

            // list 
            List<int> assignedProjectIds = new ResidencyService<CompetencyPersonProject>().Queryable()
                .Where( a => a.CompetencyPersonId.Equals( competencyPersonId ) )
                .Select( a => a.ProjectId ).ToList();

            var list = projectQry.Where( a => !assignedProjectIds.Contains( a.Id ) ).OrderBy( a => a.Name ).ToList();

            ddlProject.DataSource = list;
            ddlProject.DataBind();
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="competencyPersonProject">The competency person project.</param>
        private void ShowEditDetails( CompetencyPersonProject competencyPersonProject )
        {
            if ( competencyPersonProject.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( CompetencyPersonProject.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( CompetencyPersonProject.FriendlyTypeName );
            }

            LoadDropDowns();

            SetEditMode( true );

            lblPersonName.Text = competencyPersonProject.CompetencyPerson.Person.FullName;
            lblCompetency.Text = competencyPersonProject.CompetencyPerson.Competency.Name;
            ddlProject.SetValue( competencyPersonProject.ProjectId );

            if ( competencyPersonProject.Project != null )
            {
                lblProject.Text = competencyPersonProject.Project.Name;
            }
            else
            {
                // shouldn't happen, but just in case
                lblProject.Text = Rock.Constants.None.Text;
            }

            bool addMode = competencyPersonProject.Id == 0;

            ddlProject.Visible = addMode;
            lblProject.Visible = !addMode;
            lblPersonName.Visible = true;
            lblCompetency.Visible = true;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="competencyPersonProject">The competency person project.</param>
        private void ShowReadonlyDetails( CompetencyPersonProject competencyPersonProject )
        {
            SetEditMode( false );

            string competencyPersonPageGuid = this.GetAttributeValue( "ResidencyCompetencyPersonPage" );
            string competencyPersonHtml = competencyPersonProject.CompetencyPerson.Competency.Name;
            if ( !string.IsNullOrWhiteSpace( competencyPersonPageGuid ) )
            {
                var page = new PageService().Get( new Guid( competencyPersonPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyPersonId", competencyPersonProject.CompetencyPersonId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                competencyPersonHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, competencyPersonProject.CompetencyPerson.Competency.Name );
            }

            lblMainDetails.Text = new DescriptionList()
                .Add( "Resident", competencyPersonProject.CompetencyPerson.Person )
                .Add( "Project", competencyPersonProject.Project.Name )
                .Add( "Competency", competencyPersonHtml )
                .StartSecondColumn()
                .Add( "Period", competencyPersonProject.Project.Competency.Track.Period.Name )
                .Add( "Track", competencyPersonProject.Project.Competency.Track.Name )
                .Html;
        }

        #endregion
    }
}