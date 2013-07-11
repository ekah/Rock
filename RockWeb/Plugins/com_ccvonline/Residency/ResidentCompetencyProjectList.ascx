<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentCompetencyProjectList.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentCompetencyProjectList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfCompetencyPersonId" runat="server" />
        <Rock:Grid ID="gProjectList" runat="server" AllowSorting="true" OnRowSelected="gProjectList_RowSelected" DataKeyNames="Id" RowItemText="Project" DisplayType="Light">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="MinAssessmentCount" HeaderText="Min # Assessments" SortExpression="MinAssessmentCount" />
                <asp:BoundField DataField="AssessmentCompleted" HeaderText="Completed" SortExpression="AssessmentCompleted" />
                <Rock:BadgeField DataField="AssessmentRemaining" HeaderText="Remaining" SortExpression="AssessmentRemaining" WarningMin="1" SuccessMax="0" SuccessMin="0" ImportantMin="9999" InfoMin="9999" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>
