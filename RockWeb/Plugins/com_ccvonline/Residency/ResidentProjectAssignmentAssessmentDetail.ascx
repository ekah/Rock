<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentProjectAssignmentAssessmentDetail.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentProjectAssignmentAssessmentDetail" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfCompetencyPersonProjectAssignmentAssessmentId" runat="server" />
        <div class="well">
            <div class="row-fluid">
                <asp:Literal ID="lblProjectAssignmentDetails" runat="server" />
            </div>
        </div>
        <h4>Project Assignment Assessment
        </h4>
        <Rock:Grid ID="gList" runat="server" AllowSorting="false" OnRowSelected="gList_Edit" DataKeyNames="ProjectPointOfAssessmentId,CompetencyPersonProjectAssignmentAssessmentId" DisplayType="Light">
            <Columns>
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentOrder" HeaderText="#" />
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentText" HeaderText="Text" />
                <asp:BoundField DataField="CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.Rating" HeaderText="Rating" />
                <asp:BoundField DataField="CompetencyPersonProjectAssignmentAssessmentPointOfAssessment.RatingNotes" HeaderText="Rating Text" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>
