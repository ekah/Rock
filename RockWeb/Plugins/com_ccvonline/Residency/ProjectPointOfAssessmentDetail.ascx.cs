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
using Rock.Web;
using Rock.Web.UI;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProjectPointOfAssessmentDetail : RockBlock, IDetailBlock
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
                int? itemId = PageParameter( "projectPointOfAssessmentId" ).AsInteger( true );
                int? projectId = PageParameter( "projectId" ).AsInteger( true );
                if ( itemId != null )
                {
                    if ( projectId == null )
                    {
                        ShowDetail( "projectPointOfAssessmentId", itemId.Value );
                    }
                    else
                    {
                        ShowDetail( "projectPointOfAssessmentId", itemId.Value, projectId.Value );
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

            int? projectPointOfAssessmentId = this.PageParameter( pageReference, "projectPointOfAssessmentId" ).AsInteger();
            if ( projectPointOfAssessmentId != null )
            {
                breadCrumbs.Add( new BreadCrumb( "Point of Assessment", pageReference ) );
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
            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["projectId"] = hfProjectId.Value;
            NavigateToParentPage( qryString );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            ProjectPointOfAssessment projectPointOfAssessment;
            ResidencyService<ProjectPointOfAssessment> projectPointOfAssessmentService = new ResidencyService<ProjectPointOfAssessment>();

            int projectPointOfAssessmentId = int.Parse( hfProjectPointOfAssessmentId.Value );

            if ( projectPointOfAssessmentId == 0 )
            {
                projectPointOfAssessment = new ProjectPointOfAssessment();
                projectPointOfAssessment.AssessmentOrder = lblAssessmentOrder.Text.AsInteger().Value;
                projectPointOfAssessment.ProjectId = hfProjectId.ValueAsInt();
                projectPointOfAssessmentService.Add( projectPointOfAssessment, CurrentPersonId );
            }
            else
            {
                projectPointOfAssessment = projectPointOfAssessmentService.Get( projectPointOfAssessmentId );
            }

            projectPointOfAssessment.CompetencyTypeValueId = ddlCompetencyTypeValue.SelectedValueAsInt();
            projectPointOfAssessment.AssessmentText = tbAssessmentText.Text;

            if ( !projectPointOfAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                projectPointOfAssessmentService.Save( projectPointOfAssessment, CurrentPersonId );
            } );

            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["projectId"] = hfProjectId.Value;
            NavigateToParentPage( qryString );
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            var list = new Rock.Model.DefinedValueService().GetByDefinedTypeGuid( new Guid( com.ccvonline.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE ) )
                .OrderBy( a => a.Name ).ToList();

            list.Insert( 0, new DefinedValue { Id = Rock.Constants.None.Id, Name = Rock.Constants.None.Text } );

            ddlCompetencyTypeValue.DataSource = list;
            ddlCompetencyTypeValue.DataBind();
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
        /// Shows the detail
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        /// <param name="projectId">The residency project id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? projectId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "projectPointOfAssessmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            LoadDropDowns();

            // Load depending on Add(0) or Edit
            ProjectPointOfAssessment projectPointOfAssessment = null;
            var projectPointOfAssessmentService = new ResidencyService<ProjectPointOfAssessment>();

            string projectName = new ResidencyService<Project>().Queryable()
                .Where( a => a.Id.Equals( projectId.Value ) )
                .Select( a => a.Name ).FirstOrDefault();

            if ( !itemKeyValue.Equals( 0 ) )
            {
                projectPointOfAssessment = projectPointOfAssessmentService.Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( "Point of Assessment for " + projectName );
            }
            else
            {
                // don't try add if there wasn't a projectId specified
                if ( projectId != null )
                {
                    projectPointOfAssessment = new ProjectPointOfAssessment { Id = 0, ProjectId = projectId.Value };

                    int maxAssessmentOrder = projectPointOfAssessmentService.Queryable()
                        .Where( a => a.ProjectId.Equals( projectPointOfAssessment.ProjectId ) )
                        .Select( a => a.AssessmentOrder ).DefaultIfEmpty( 0 ).Max();

                    projectPointOfAssessment.AssessmentOrder = maxAssessmentOrder + 1;

                    lActionTitle.Text = ActionTitle.Add( "Point of Assessment for " + projectName );
                }
            }

            if ( projectPointOfAssessment == null )
            {
                return;
            }

            hfProjectPointOfAssessmentId.Value = projectPointOfAssessment.Id.ToString();
            hfProjectId.Value = projectId.ToString();
            lblAssessmentOrder.Text = projectPointOfAssessment.AssessmentOrder.ToString();
            tbAssessmentText.Text = projectPointOfAssessment.AssessmentText;
            ddlCompetencyTypeValue.SetValue( projectPointOfAssessment.CompetencyTypeValueId );

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ProjectPointOfAssessment.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( ProjectPointOfAssessment.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            tbAssessmentText.ReadOnly = readOnly;
            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}