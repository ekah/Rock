<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList.ascx.cs" Inherits="RockWeb.Plugins.com.ccvonline.Residency.CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfResidencyCompetencyPersonProjectAssignmentAssessmentId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="false" OnRowSelected="gList_Edit" DataKeyNames="ResidencyProjectPointOfAssessmentId,ResidencyCompetencyPersonProjectAssignmentAssessmentId" >
            <Columns>
                <asp:BoundField DataField="ResidencyProjectPointOfAssessment.AssessmentOrder" HeaderText="#" SortExpression="ResidencyProjectPointOfAssessment.AssessmentOrder" />
                <asp:BoundField DataField="ResidencyProjectPointOfAssessment.AssessmentText" HeaderText="Text" SortExpression="ResidencyProjectPointOfAssessment.AssessmentText" />
                <asp:BoundField DataField="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating" HeaderText="Rating" SortExpression="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating" />
                <asp:BoundField DataField="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes" HeaderText="Rating Text" SortExpression="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingText" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>