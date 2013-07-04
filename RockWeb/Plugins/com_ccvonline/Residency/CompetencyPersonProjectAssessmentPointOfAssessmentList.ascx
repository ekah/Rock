<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectAssessmentPointOfAssessmentList.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.CompetencyPersonProjectAssessmentPointOfAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfCompetencyPersonProjectAssessmentId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="false" OnRowSelected="gList_Edit" DataKeyNames="ProjectPointOfAssessmentId,CompetencyPersonProjectAssessmentId" >
            <Columns>
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentOrder" HeaderText="#" SortExpression="ProjectPointOfAssessment.AssessmentOrder" />
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentText" HeaderText="Text" SortExpression="ProjectPointOfAssessment.AssessmentText" />
                <asp:BoundField DataField="CompetencyPersonProjectAssessmentPointOfAssessment.Rating" HeaderText="Rating" SortExpression="CompetencyPersonProjectAssessmentPointOfAssessment.Rating" />
                <asp:BoundField DataField="CompetencyPersonProjectAssessmentPointOfAssessment.RatingNotes" HeaderText="Rating Text" SortExpression="CompetencyPersonProjectAssessmentPointOfAssessment.RatingText" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>