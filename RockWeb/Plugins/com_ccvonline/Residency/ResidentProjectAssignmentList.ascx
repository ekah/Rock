<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentProjectAssignmentList.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentProjectAssignmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfCompetencyPersonProjectId" runat="server" />
        <h4>Project Assignments</h4>
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" RowItemText="Project Assignment" DisplayType="Light">
            <Columns>
                <asp:BoundField DataField="AssessorPerson.FullName" HeaderText="Assessor" SortExpression="AssessorPerson.FullName" />
                <Rock:DateTimeField DataField="CompletedDateTime" HeaderText="Completed" SortExpression="CompletedDateTime" />
                <asp:BoundField DataField="AssessmentCount" HeaderText="# Assessments" SortExpression="AssessmentDateTime" />
                <Rock:DateTimeField DataField="AssessmentDateTime" HeaderText="Last Assessed" SortExpression="AssessmentDateTime" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>