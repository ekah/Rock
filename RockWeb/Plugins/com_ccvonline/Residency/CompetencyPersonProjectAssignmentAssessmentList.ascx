<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectAssignmentAssessmentList.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.CompetencyPersonProjectAssignmentAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfCompetencyPersonProjectAssignmentId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" RowItemText="Assessment" >
            <Columns>
                <Rock:DateTimeField DataField="AssessmentDateTime" HeaderText="Assessment Date/Time" SortExpression="AssessmentDateTime" />
                <asp:BoundField DataField="OverallRating" HeaderText="Rating" SortExpression="OverallRating" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>