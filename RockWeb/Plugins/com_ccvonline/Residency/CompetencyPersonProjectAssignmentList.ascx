<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectAssignmentList.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.CompetencyPersonProjectAssignmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfCompetencyPersonProjectId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" RowItemText="Project Assignment">
            <Columns>
                <asp:BoundField DataField="AssessorPerson.FullName" HeaderText="Assessor" SortExpression="AssessorPerson.FullName" />
                <Rock:DateTimeField DataField="CompletedDateTime" HeaderText="Completed" SortExpression="CompletedDateTime" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>