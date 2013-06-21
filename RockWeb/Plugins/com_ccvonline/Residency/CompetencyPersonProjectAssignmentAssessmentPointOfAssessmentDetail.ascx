<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentDetail.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentDetail" %>

<asp:UpdatePanel ID="upCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfProjectPointOfAssessmentId" runat="server" />
            <asp:HiddenField ID="hfCompetencyPersonProjectAssignmentAssessmentId" runat="server" />

            <div id="pnlEditDetails" runat="server">

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
                <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

                <fieldset>
                    <legend>
                        <asp:Literal ID="lActionTitle" runat="server" />
                    </legend>

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <Rock:LabeledText ID="lblResident" runat="server" LabelText="Resident" />
                    <Rock:LabeledText ID="lblCompetency" runat="server" LabelText="Competency" />
                    <Rock:LabeledText ID="lblProjectName" runat="server" LabelText="Project" />
                    <Rock:LabeledText ID="lblAssessor" runat="server" LabelText="Assessor" />
                    <Rock:LabeledText ID="lblAssessmentOrder" runat="server" LabelText="Assessment #" />
                    <Rock:LabeledText ID="lblAssessmentText" runat="server" LabelText="Assessment Text" />
                    <Rock:NumberBox ID="tbRating" runat="server" LabelText="Rating" MaximumValue="5" MinimumValue="1" />
                    <Rock:DataTextBox ID="tbRatingNotes" runat="server" SourceTypeName="com.ccvonline.Residency.Model.CompetencyPersonProjectAssignmentAssessmentPointOfAssessment, com.ccvonline.Residency" PropertyName="RatingNotes" TextMode="MultiLine" Rows="3" />
                </fieldset>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
