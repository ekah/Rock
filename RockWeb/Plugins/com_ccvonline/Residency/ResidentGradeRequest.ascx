<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentGradeRequest.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentGradeRequest" %>

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfCompetencyPersonProjectId" runat="server" />
        <div class="well">
            <fieldset>
                <legend>
                    <asp:Literal ID="lblLoginTitle" runat="server" Text="Facilitator Login" /></legend>

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
                
                <asp:Literal ID="lblLoginInstructions" runat="server" Text="The teacher of this competency or an authorized grader must login to grade this project." />

                <Rock:LabeledTextBox ID="tbUserName" runat="server" LabelText="Username" Required="true" />
                <Rock:LabeledTextBox ID="tbPassword" runat="server" LabelText="Password" Required="true" TextMode="Password" AutoCompleteType="Disabled" />

                <Rock:NotificationBox ID="nbWarningMessage" ClientIDMode="Static" runat="server" NotificationBoxType="Warning" />

                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />

            </fieldset>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
