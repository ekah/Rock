﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyPersonProjectAssignmentAssessmentDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.ResidencyCompetencyPersonProjectAssignmentAssessmentDetail" %>

<asp:UpdatePanel ID="upResidencyCompetencyPersonProjectAssignmentAssessmentDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfResidencyCompetencyPersonProjectAssignmentAssessmentId" runat="server" />
            <asp:HiddenField ID="hfResidencyCompetencyPersonProjectAssignmentId" runat="server" />

            <div id="pnlEditDetails" runat="server" class="well">

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
                <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

                <fieldset>
                    <legend>
                        <asp:Literal ID="lActionTitle" runat="server" />
                    </legend>

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <Rock:LabeledText ID="lblProjectName" runat="server" LabelText="Project" />
                    <Rock:DateTimePicker ID="dtpAssessmentDateTime" runat="server" LabelText="Assessment Date/Time" />
                    <Rock:NumberBox ID="tbRating" runat="server" LabelText="Rating" />
                    <Rock:DataTextBox ID="tbRatingNotes" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetencyPersonProjectAssignmentAssessment, com.ccvonline.Residency" PropertyName="Rating Notes" TextMode="MultiLine" Rows="3" />
                    <Rock:DataTextBox ID="tbResidentComments" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetencyPersonProjectAssignmentAssessment, com.ccvonline.Residency" PropertyName="ResidentComments" TextMode="MultiLine" Rows="3" />

                </fieldset>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </div>

            <fieldset id="fieldsetViewDetails" runat="server">
                <legend>Project Assignment Assessment - Points of Assessment
                </legend>
                <div class="well">
                    <div class="row-fluid">
                        <Rock:NotificationBox ID="NotificationBox1" runat="server" NotificationBoxType="Info" />
                    </div>
                    <div class="row-fluid">
                        <asp:Literal ID="lblMainDetails" runat="server" />
                    </div>
                    <div class="actions">
                        <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-mini" OnClick="btnEdit_Click" />
                    </div>
                </div>

            </fieldset>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
