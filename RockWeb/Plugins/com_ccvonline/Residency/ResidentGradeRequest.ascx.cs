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
using Rock.Model;
using Rock.Security;
using Rock.Web.UI;

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

        #endregion
    }
}