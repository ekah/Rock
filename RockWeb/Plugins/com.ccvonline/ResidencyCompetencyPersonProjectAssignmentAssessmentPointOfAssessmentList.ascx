<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfResidencyCompetencyPersonProjectAssignmentAssessmentId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="false" OnRowSelected="gList_Edit" DataKeyNames="ResidencyProjectPointOfAssessmentId,ResidencyCompetencyPersonProjectAssignmentAssessmentId" >
            <Columns>
                <asp:BoundField DataField="ResidencyProjectPointOfAssessment.AssessmentOrder" HeaderText="#" SortExpression="ResidencyProjectPointOfAssessment.AssessmentOrder" />
                <asp:BoundField DataField="ResidencyProjectPointOfAssessment.AssessmentText" HeaderText="Text" SortExpression="ResidencyProjectPointOfAssessment.AssessmentText" />
                <asp:BoundField DataField="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating" HeaderText="Rating" SortExpression="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating" />
                <asp:BoundField DataField="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingText" HeaderText="Rating Text" SortExpression="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingText" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>