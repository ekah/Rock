<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentGradeRequest.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentGradeRequest" %>

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <div class="well">
            <fieldset>
                <legend>
                    <asp:Literal ID="lblLoginTitle" runat="server" Text="Facilitator Login" /></legend>

                <asp:Literal ID="lblLoginInstructions" runat="server" Text="An authorized teacher or facilitator must login to grade this project." />

                <Rock:LabeledTextBox ID="tbUserName" runat="server" LabelText="Username" Required="true" />
                <Rock:LabeledTextBox ID="tbPassword" runat="server" LabelText="Password" Required="true" TextMode="Password" />

                <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert block-message warning" />

                <asp:Button ID="LoginButton" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="LoginButton_Click" />

            </fieldset>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
