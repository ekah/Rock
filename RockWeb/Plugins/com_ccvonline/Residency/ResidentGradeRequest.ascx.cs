//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Communication;
using Rock.Model;
using Rock.Security;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [LinkedPage( "Resident Grade Detail Page" )]
    [GroupField( "Residency Grader Security Role" )]
    public partial class ResidentGradeRequest : RockBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            // clear the session residentGraderSessionKey just in case
            Session["residentGraderSessionKey"] = string.Empty;

            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                hfCompetencyPersonProjectId.Value = this.PageParameter( "competencyPersonProjectId" );
                LoadDropDowns();
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

            int? competencyPersonProjectId = this.PageParameter( pageReference, "competencyPersonProjectId" ).AsInteger();
            if ( competencyPersonProjectId != null )
            {
                breadCrumbs.Add( new BreadCrumb( "Grade Request", pageReference ) );
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
        /// Loads the drop downs.
        /// </summary>
        protected void LoadDropDowns()
        {
            string groupId = this.GetAttributeValue( "ResidencyGraderSecurityRole" );

            List<Person> facilitatorList = new List<Person>();

            Group residencyGraderSecurityRole = new GroupService().Get( groupId.AsInteger() ?? 0 );
            if ( residencyGraderSecurityRole != null )
            {
                foreach ( var groupMember in residencyGraderSecurityRole.Members )
                {
                    facilitatorList.Add( groupMember.Person );
                }
            }

            CompetencyPersonProject competencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( hfCompetencyPersonProjectId.ValueAsInt() );
            if ( competencyPersonProject != null )
            {
                if ( competencyPersonProject.Project.Competency.TeacherOfRecordPerson != null )
                {
                    if ( !facilitatorList.Contains( competencyPersonProject.Project.Competency.TeacherOfRecordPerson ) )
                    {
                        facilitatorList.Add( competencyPersonProject.Project.Competency.TeacherOfRecordPerson );
                    }
                }
            }

            if ( facilitatorList.Any() )
            {
                nbSendMessage.Text = string.Empty;
                ddlFacilitators.DataSource = facilitatorList;
                ddlFacilitators.DataBind();
            }
            else
            {
                nbSendMessage.NotificationBoxType = Rock.Web.UI.Controls.NotificationBoxType.Error;
                nbSendMessage.Text = "No facilitators configured";
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
        /// Handles the Click event of the btnLogin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnLogin_Click( object sender, EventArgs e )
        {
            if ( !Page.IsValid )
            {
                // controls will render messages
                return;
            }

            nbWarningMessage.Text = string.Empty;

            var userLoginService = new UserLoginService();
            var userLogin = userLoginService.GetByUserName( tbUserName.Text );
            if ( userLogin != null && userLogin.ServiceType == AuthenticationServiceType.Internal )
            {
                foreach ( var serviceEntry in AuthenticationContainer.Instance.Components )
                {
                    var component = serviceEntry.Value.Value;
                    string componentName = component.GetType().FullName;

                    if (
                        userLogin.ServiceName == componentName &&
                        component.AttributeValues.ContainsKey( "Active" ) &&
                        bool.Parse( component.AttributeValues["Active"][0].Value )
                    )
                    {
                        if ( component.Authenticate( userLogin, tbPassword.Text ) )
                        {
                            string groupId = this.GetAttributeValue( "ResidencyGraderSecurityRole" );

                            Group residencyGraderSecurityRole = new GroupService().Get( groupId.AsInteger() ?? 0 );
                            if ( residencyGraderSecurityRole != null )
                            {
                                // Grader must either by member of ResidencyGraderSecurityRole or the Teacher of Record for this project's competency
                                bool userAuthorizedToGrade = residencyGraderSecurityRole.Members.Any( a => a.PersonId == userLogin.PersonId );
                                if ( !userAuthorizedToGrade )
                                {
                                    CompetencyPersonProject competencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( hfCompetencyPersonProjectId.ValueAsInt() );
                                    if ( competencyPersonProject != null )
                                    {
                                        userAuthorizedToGrade = competencyPersonProject.Project.Competency.TeacherOfRecordPersonId.Equals( userLogin.PersonId );
                                    }

                                    if ( competencyPersonProject.CompetencyPerson.PersonId != CurrentPersonId )
                                    {
                                        // somebody besides the Resident is logged in
                                        NavigateToParentPage();
                                        return;
                                    }
                                }

                                if ( userAuthorizedToGrade )
                                {
                                    string gradeDetailPageGuid = this.GetAttributeValue( "ResidentGradeDetailPage" );
                                    if ( !string.IsNullOrWhiteSpace( gradeDetailPageGuid ) )
                                    {
                                        var page = new PageService().Get( new Guid( gradeDetailPageGuid ) );
                                        if ( page != null )
                                        {
                                            string identifier = hfCompetencyPersonProjectId.Value + "|" + userLogin.Guid + "|" + DateTime.Now.Ticks;
                                            string residentGraderSessionKey = Rock.Security.Encryption.EncryptString( identifier );
                                            Session["residentGraderSessionKey"] = residentGraderSessionKey;
                                            var queryString = new Dictionary<string, string>();
                                            queryString.Add( "competencyPersonProjectId", hfCompetencyPersonProjectId.Value );

                                            NavigateToPage( page.Guid, queryString );

                                            return;
                                        }
                                    }

                                    nbWarningMessage.Text = "Ooops! Grading page not configured.";
                                    return;
                                }
                                else
                                {
                                    nbWarningMessage.Text = "User not authorized to grade this project";
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            nbWarningMessage.Text = "Invalid Login Information";
        }

        /// <summary>
        /// Handles the Click event of the btnSendRequest control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSendRequest_Click( object sender, EventArgs e )
        {
            if ( !Page.IsValid )
            {
                // controls will render error messages
                return;
            }

            int personId = ddlFacilitators.SelectedValueAsInt() ?? 0;

            Person facilitator = new PersonService().Get( personId );

            if ( facilitator == null )
            {
                ddlFacilitators.ShowErrorMessage( "Facilitator not found" );
                return;
            }

            string gradeDetailPageGuid = this.GetAttributeValue( "ResidentGradeDetailPage" );

            CompetencyPersonProject competencyPersonProject = new ResidencyService<CompetencyPersonProject>().Get( hfCompetencyPersonProjectId.ValueAsInt() );

            var userLoginService = new UserLoginService();
            var facilitatorUserLogin = userLoginService.GetByPersonId( facilitator.Id ).FirstOrDefault();

            Uri gradeDetailPageUrl = null;
            if ( !string.IsNullOrWhiteSpace( gradeDetailPageGuid ) )
            {
                PageCache pageCache = PageCache.Read( new Guid( gradeDetailPageGuid ) );
                if ( pageCache != null )
                {
                    Dictionary<string, string> queryString = new Dictionary<string, string>();

                    int routeId = 0;
                    {
                        routeId = pageCache.PageRoutes.FirstOrDefault().Key;
                    }

                    // set Ticks (3rd part) to 0 since this is an emailed request
                    string identifier = hfCompetencyPersonProjectId.Value + "|" + facilitatorUserLogin.Guid + "|0";
                    string gradeKey = Rock.Security.Encryption.EncryptString( identifier );

                    queryString.Add( "competencyPersonProjectId", hfCompetencyPersonProjectId.Value );
                    queryString.Add( "gradeKey", Server.UrlEncode(gradeKey) );

                    PageReference pageReference = new PageReference( pageCache.Id, routeId, queryString );

                    Uri rootUri = new Uri(this.RootPath);
                    gradeDetailPageUrl = new Uri( rootUri, pageReference.BuildUrl() );
                }
            }
            else
            {
                nbWarningMessage.Text = "Ooops! Grading page not configured.";
                return;
            }

            var mergeObjects = new Dictionary<string, object>();
            mergeObjects.Add( "Facilitator", facilitator.ToDictionary() );
            mergeObjects.Add( "Resident", competencyPersonProject.CompetencyPerson.Person.ToDictionary() );
            mergeObjects.Add( "Project", competencyPersonProject.Project.ToDictionary() );

            mergeObjects.Add( "GradeDetailPageUrl", gradeDetailPageUrl.ToString() );

            var recipients = new Dictionary<string, Dictionary<string, object>>();
            
            recipients.Add( facilitator.Email, mergeObjects );

            Email email = new Email( com.ccvonline.SystemGuid.EmailTemplate.RESIDENCY_PROJECT_GRADE_REQUEST );
            email.Send( recipients );
        }

        #endregion
    }
}