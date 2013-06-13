<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyPersonList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyPersonList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfGroupId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" IsPersonList="true">
            <Columns>
                <asp:BoundField DataField="FullName" HeaderText="Name" SortExpression="FullName" />
                <asp:BoundField DataField="CompetencyCount" HeaderText="Competency Count" SortExpression="CompetencyCount" />
                <asp:BoundField DataField="CompletedProjectsTotal" HeaderText="Projects Completed" SortExpression="CompletedProjectsTotal" />
                <asp:BoundField DataField="AssignedProjectsTotal" HeaderText="Total Projects" SortExpression="AssignedProjectsTotal" />

                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>