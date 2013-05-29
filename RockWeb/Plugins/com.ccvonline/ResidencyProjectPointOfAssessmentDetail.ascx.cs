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
    public partial class ResidencyProjectPointOfAssessmentDetail : RockBlock, IDetailBlock
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
                int? itemId = PageParameter( "residencyProjectPointOfAssessmentId" ).AsInteger( true );
                int? projectId = PageParameter( "residencyProjectId" ).AsInteger( true );
                if ( itemId != null )
                {
                    if ( projectId == null )
                    {
                        ShowDetail( "residencyProjectPointOfAssessmentId", itemId.Value );
                    }
                    else
                    {
                        ShowDetail( "residencyProjectPointOfAssessmentId", itemId.Value, projectId.Value );
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
            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["residencyProjectId"] = hfResidencyProjectId.Value;
            NavigateToParentPage( qryString );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            ResidencyProjectPointOfAssessment residencyProjectPointOfAssessment;
            ResidencyService<ResidencyProjectPointOfAssessment> residencyProjectPointOfAssessmentService = new ResidencyService<ResidencyProjectPointOfAssessment>();

            int ResidencyProjectPointOfAssessmentId = int.Parse( hfResidencyProjectPointOfAssessmentId.Value ); ;

            if ( ResidencyProjectPointOfAssessmentId == 0 )
            {
                residencyProjectPointOfAssessment = new ResidencyProjectPointOfAssessment();
                residencyProjectPointOfAssessment.AssessmentOrder = lblAssessmentOrder.Text.AsInteger().Value;
                residencyProjectPointOfAssessment.ResidencyProjectId = hfResidencyProjectId.ValueAsInt();
                residencyProjectPointOfAssessmentService.Add( residencyProjectPointOfAssessment, CurrentPersonId );
            }
            else
            {
                residencyProjectPointOfAssessment = residencyProjectPointOfAssessmentService.Get( ResidencyProjectPointOfAssessmentId );
            }

            residencyProjectPointOfAssessment.AssessmentText = tbAssessmentText.Text;

            if ( !residencyProjectPointOfAssessment.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                residencyProjectPointOfAssessmentService.Save( residencyProjectPointOfAssessment, CurrentPersonId );
            } );

            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["residencyProjectId"] = hfResidencyProjectId.Value;
            NavigateToParentPage( qryString );
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
        /// <param name="residencyProjectId">The residency project id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? residencyProjectId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "residencyProjectPointOfAssessmentId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            ResidencyProjectPointOfAssessment residencyProjectPointOfAssessment = null;
            var residencyProjectPointOfAssessmentService = new ResidencyService<ResidencyProjectPointOfAssessment>();

            string residencyProjectName = new ResidencyService<ResidencyProject>().Queryable()
                .Where( a => a.Id.Equals( residencyProjectId.Value ) )
                .Select( a => a.Name ).FirstOrDefault();

            if ( !itemKeyValue.Equals( 0 ) )
            {
                residencyProjectPointOfAssessment = residencyProjectPointOfAssessmentService.Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( "Point of Assessment for " + residencyProjectName );
            }
            else
            {
                // don't try add if there wasn't a residencyProjectId specified
                if ( residencyProjectId != null )
                {
                    residencyProjectPointOfAssessment = new ResidencyProjectPointOfAssessment { Id = 0, ResidencyProjectId = residencyProjectId.Value };
                    
                    int maxAssessmentOrder = residencyProjectPointOfAssessmentService.Queryable()
                        .Where( a => a.ResidencyProjectId.Equals( residencyProjectPointOfAssessment.ResidencyProjectId ) )
                        .Select( a => a.AssessmentOrder ).DefaultIfEmpty( 0 ).Max();

                    residencyProjectPointOfAssessment.AssessmentOrder = maxAssessmentOrder + 1;
                    
                    lActionTitle.Text = ActionTitle.Add( "Point of Assessment for " + residencyProjectName );
                }
            }

            if ( residencyProjectPointOfAssessment == null )
            {
                return;
            }

            hfResidencyProjectPointOfAssessmentId.Value = residencyProjectPointOfAssessment.Id.ToString();
            hfResidencyProjectId.Value = residencyProjectId.ToString();
            lblAssessmentOrder.Text = residencyProjectPointOfAssessment.AssessmentOrder.ToString();
            tbAssessmentText.Text = residencyProjectPointOfAssessment.AssessmentText;

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ResidencyProjectPointOfAssessment.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( ResidencyProjectPointOfAssessment.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            tbAssessmentText.ReadOnly = readOnly;
            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}