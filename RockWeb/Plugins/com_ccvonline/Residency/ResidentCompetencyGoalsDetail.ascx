﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentCompetencyGoalsDetail.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentCompetencyGoalsDetail" %>

<asp:UpdatePanel ID="upDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server" Visible="false">
            <blockquote>
                <p>
                    <asp:Literal ID="lblGoals" runat="server" />
                </p>
            </blockquote>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
