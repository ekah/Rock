<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentProjectAssessmentList.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentProjectAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfCompetencyPersonProjectId" runat="server" />
        <h4>Project Assessments</h4>
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" RowItemText="Project Assessments" DisplayType="Light">
            <Columns>
                <asp:BoundField DataField="AssessorPerson.FullName" HeaderText="Assessor" SortExpression="AssessorPerson.FullName" />
                <Rock:DateTimeField DataField="AssessmentDateTime" HeaderText="Last Assessed" SortExpression="AssessmentDateTime" />
                <asp:BoundField DataField="OverallRating" HeaderText="Rating" SortExpression="OverallRating" />
                <asp:BoundField DataField="RatingNotes" HeaderText="Rating Notes" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>