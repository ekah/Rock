<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentProjectAssessmentDetail.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentProjectAssessmentDetail" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfCompetencyPersonProjectAssessmentId" runat="server" />
        <div class="well">
            <div class="row-fluid">
                <asp:Literal ID="lblProjectDetails" runat="server" />
            </div>
        </div>
        <h4>Project Assessment
        </h4>
        <Rock:Grid ID="gList" runat="server" AllowSorting="false" OnRowSelected="gList_Edit" DataKeyNames="ProjectPointOfAssessmentId,CompetencyPersonProjectAssessmentId" DisplayType="Light">
            <Columns>
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentOrder" HeaderText="#" />
                <asp:BoundField DataField="ProjectPointOfAssessment.AssessmentText" HeaderText="Text" />
                <asp:BoundField DataField="CompetencyPersonProjectAssessmentPointOfAssessment.Rating" HeaderText="Rating" />
                <asp:BoundField DataField="CompetencyPersonProjectAssessmentPointOfAssessment.RatingNotes" HeaderText="Rating Text" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>
