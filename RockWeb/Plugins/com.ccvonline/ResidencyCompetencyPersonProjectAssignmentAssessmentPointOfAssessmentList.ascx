<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfResidencyCompetencyPersonProjectAssignmentAssessmentId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="false" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <asp:BoundField DataField="ResidencyProjectPointOfAssessment.AssessmentOrder" HeaderText="Text" SortExpression="ResidencyProjectPointOfAssessment.AssessmentOrder" />
                <asp:BoundField DataField="ResidencyProjectPointOfAssessment.AssessmentText" HeaderText="Text" SortExpression="ResidencyProjectPointOfAssessment.AssessmentText" />
                <asp:BoundField DataField="Rating" HeaderText="Rating" SortExpression="Rating" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>