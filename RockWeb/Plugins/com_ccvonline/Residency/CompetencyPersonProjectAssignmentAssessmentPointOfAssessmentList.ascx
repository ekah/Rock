<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfCompetencyPersonProjectAssignmentAssessmentId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="false" OnRowSelected="gList_Edit" DataKeyNames="ProjectPointOfAssessmentId,CompetencyPersonProjectAssignmentAssessmentId" >
            <Columns>
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentOrder" HeaderText="#" SortExpression="ProjectPointOfAssessment.AssessmentOrder" />
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentText" HeaderText="Text" SortExpression="ProjectPointOfAssessment.AssessmentText" />
                <asp:BoundField DataField="CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating" HeaderText="Rating" SortExpression="CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating" />
                <asp:BoundField DataField="CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes" HeaderText="Rating Text" SortExpression="CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingText" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>