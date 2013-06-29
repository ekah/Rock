﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeValues.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonDetail.AttributeValues" %>

<asp:UpdatePanel ID="upAttributeValues" runat="server" class="context-attribute-values">
<ContentTemplate>

    <section class="widget relationships">
        <header class="clearfix">
            <h4 class="pull-left">
                <asp:Literal ID="lCategoryName" runat="server" />
            </h4>
            <div class="actions pull-right" style="display: none;">
                <asp:LinkButton ID="lbEdit" runat="server" CssClass="edit" OnClick="lbEdit_Click"><i class="icon-pencil"></i></asp:LinkButton>
            </div>
        </header>
        <div class="widget-content">
            <ul>
                <asp:PlaceHolder ID="phAttributes" runat="server"></asp:PlaceHolder>
            </ul>
            <asp:Panel ID="pnlActions" runat="server" CssClass="actions" Visible="false">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-mini" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-mini" OnClick="btnCancel_Click" CausesValidation="false" />
            </asp:Panel>
        </div>
    </section>

</ContentTemplate>
</asp:UpdatePanel>

