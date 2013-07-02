<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentProjectAssignmentAssessmentList.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentProjectAssignmentAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfCompetencyPersonProjectAssignmentId" runat="server" />
        <div class="well">
            <div class="row-fluid">

                <asp:Literal ID="lblProjectDetails" runat="server" />

            </div>
        </div>
        <h4>Project Assignment Assessments
        </h4>
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" RowItemText="Assessment" DisplayType="Light">
            <Columns>
                <Rock:DateTimeField DataField="AssessmentDateTime" HeaderText="Assessment Date/Time" SortExpression="AssessmentDateTime" />
                <asp:BoundField DataField="OverallRating" HeaderText="Rating" SortExpression="OverallRating" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>
