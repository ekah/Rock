<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyPersonProjectAssignmentList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyCompetencyPersonProjectAssignmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfResidencyCompetencyPersonProjectId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <asp:BoundField DataField="ResidencyCompetencyPersonProject.ResidencyProject.Name" HeaderText="Project" SortExpression="ResidencyCompetencyPersonProject.ResidencyProject.Name" />
                <asp:BoundField DataField="AssessorPerson.FullName" HeaderText="Assessor" SortExpression="AssessorPerson.FullName" />
                <Rock:DateTimeField DataField="CompletedDateTime" HeaderText="Completed" SortExpression="CompletedDateTime" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>