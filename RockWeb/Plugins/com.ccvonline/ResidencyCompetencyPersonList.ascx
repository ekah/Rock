﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyPersonList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyCompetencyPersonList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfPersonId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" RowItemText="Competency">
            <Columns>
                <asp:BoundField DataField="ResidencyCompetencyName" HeaderText="Competency" SortExpression="ResidencyCompetencyName" />
                <asp:BoundField DataField="CompletedProjectsTotal" HeaderText="Projects Completed" SortExpression="CompletedProjectsTotal" />
                <asp:BoundField DataField="AssignedProjectsTotal" HeaderText="Total Projects" SortExpression="AssignedProjectsTotal" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>