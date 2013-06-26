<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentProjectDetail.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentProjectDetail" %>

<asp:UpdatePanel ID="upCompetencyPersonProjectDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfCompetencyPersonProjectId" runat="server" />

            <fieldset id="fieldsetViewDetails" runat="server">
                <div class="well">
                    <div class="row-fluid">
                        <asp:Literal ID="lblMainDetails" runat="server" />
                    </div>
                    <div class="actions">
                        <asp:LinkButton ID="btnGrade" runat="server" Text="Grade" CssClass="btn btn-primary btn-mini" OnClick="btnGrade_Click" />
                    </div>
                </div>
            </fieldset>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
