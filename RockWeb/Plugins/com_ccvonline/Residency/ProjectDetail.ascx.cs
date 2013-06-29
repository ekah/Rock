//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
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
    [LinkedPage( "Residency Competency Page" )]
    public partial class ProjectDetail : RockBlock, IDetailBlock
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
                string itemId = PageParameter( "projectId" );
                string competencyId = PageParameter( "competencyId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( competencyId ) )
                    {
                        ShowDetail( "projectId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "projectId", int.Parse( itemId ), int.Parse( competencyId ) );
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

            if ( hfProjectId.ValueAsInt().Equals( 0 ) )
            {
                // Cancelling on Add.  Return to Grid
                // if this page was called from the Competency Detail page, return to that
                string competencyId = PageParameter( "competencyId" );
                if ( !string.IsNullOrWhiteSpace( competencyId ) )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["competencyId"] = competencyId;
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
                ResidencyService<Project> service = new ResidencyService<Project>();
                Project item = service.Get( hfProjectId.ValueAsInt() );
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
            ResidencyService<Project> service = new ResidencyService<Project>();
            Project item = service.Get( hfProjectId.ValueAsInt() );
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
            Project project;
            ResidencyService<Project> projectService = new ResidencyService<Project>();

            int projectId = int.Parse( hfProjectId.Value );

            if ( projectId == 0 )
            {
                project = new Project();
                projectService.Add( project, CurrentPersonId );
            }
            else
            {
                project = projectService.Get( projectId );
            }

            project.Name = tbName.Text;
            project.Description = tbDescription.Text;
            project.CompetencyId = hfCompetencyId.ValueAsInt();
            project.MinAssignmentCountDefault = tbMinAssignmentCountDefault.Text.AsInteger( false );

            if ( !project.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                projectService.Save( project, CurrentPersonId );
            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["projectId"] = project.Id.ToString();
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
        /// <param name="competencyId">The residency competency id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? competencyId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "projectId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            Project project = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                project = new ResidencyService<Project>().Get( itemKeyValue );
            }
            else
            {
                project = new Project { Id = 0 };
                project.CompetencyId = competencyId ?? 0;
                project.Competency = new ResidencyService<Competency>().Get( project.CompetencyId );
            }

            hfProjectId.Value = project.Id.ToString();
            hfCompetencyId.Value = project.CompetencyId.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Project.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( project );
            }
            else
            {
                btnEdit.Visible = true;
                if ( project.Id > 0 )
                {
                    ShowReadonlyDetails( project );
                }
                else
                {
                    ShowEditDetails( project );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="project">The residency project.</param>
        private void ShowEditDetails( Project project )
        {
            if ( project.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Project.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Project.FriendlyTypeName );
            }

            SetEditMode( true );

            tbName.Text = project.Name;
            tbDescription.Text = project.Description;
            lblPeriod.Text = project.Competency.Track.Period.Name;
            lblTrack.Text = project.Competency.Track.Name;
            lblCompetency.Text = project.Competency.Name;
            tbMinAssignmentCountDefault.Text = project.MinAssignmentCountDefault.ToString();
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="project">The residency project.</param>
        private void ShowReadonlyDetails( Project project )
        {
            SetEditMode( false );

            string competencyPageGuid = this.GetAttributeValue( "ResidencyCompetencyPage" );
            string competencyHtml = project.Competency.Name;
            if ( !string.IsNullOrWhiteSpace( competencyPageGuid ) )
            {
                var page = new PageService().Get( new Guid( competencyPageGuid ) );
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( "competencyId", project.CompetencyId.ToString() );
                string linkUrl = new PageReference( page.Id, 0, queryString ).BuildUrl();
                competencyHtml = string.Format( "<a href='{0}'>{1}</a>", linkUrl, project.Competency.Name );
            }

            lblMainDetails.Text = new DescriptionList()
                .Add( "Name", string.Format( "{0} - {1}", project.Name, project.Description) )
                .Add( "Competency", competencyHtml )
                .StartSecondColumn()
                .Add( "Period", project.Competency.Track.Period.Name )
                .Add( "Track", project.Competency.Track.Name )
                .Html;
        }

        #endregion
    }
}