﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyProjectPointOfAssessmentDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.ResidencyProjectPointOfAssessmentDetail" %>

<!-- just to help do css intellisense at design time  -->
<link rel="stylesheet" type="text/css" href="~/CSS/bootstrap.min.css" visible="false" />

<asp:UpdatePanel ID="upResidencyProjectPointOfAssessmentDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">
            
            <asp:HiddenField ID="hfResidencyProjectId" runat="server" />
            <asp:HiddenField ID="hfResidencyProjectPointOfAssessmentId" runat="server" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
            <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </legend>

                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
                <Rock:LabeledText ID="lblAssessmentOrder" runat="server" LabelText="Assessment #" />
                <Rock:DataTextBox ID="tbAssessmentText" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyProjectPointOfAssessment, com.ccvonline.Residency" PropertyName="AssessmentText" TextMode="MultiLine" Rows="3" CssClass="input-xxlarge"/>
            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>