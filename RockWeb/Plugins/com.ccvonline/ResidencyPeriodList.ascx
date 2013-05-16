﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyPeriodList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyPeriodList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <Rock:DateField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" />
                <Rock:DateField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>