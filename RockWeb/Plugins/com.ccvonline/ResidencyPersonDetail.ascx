﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyPersonDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.ResidencyPersonDetail" %>

<asp:UpdatePanel ID="upResidencyPersonDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfPersonId" runat="server" />

            <fieldset id="fieldsetViewDetails" runat="server">
                <legend>Resident
                </legend>
                <div class="well">
                    <div class="row-fluid">
                    <div class="row-fluid">
                        <asp:Literal ID="lblMainDetails" runat="server" />
                    </div>
                </div>

            </fieldset>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
