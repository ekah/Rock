<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyPersonProjectAssignmentAssessmentList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyCompetencyPersonProjectAssignmentAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfResidencyCompetencyPersonProjectAssignmentId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <Rock:DateTimeField DataField="AssessmentDateTime" HeaderText="Assessment Date/Time" SortExpression="AssessmentDateTime" />
                <asp:BoundField DataField="Rating" HeaderText="Rating" SortExpression="Rating" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>