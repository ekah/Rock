<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectList.ascx.cs" Inherits="RockWeb.Plugins.com.ccvonline.Residency.CompetencyPersonProjectList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfResidencyCompetencyPersonId" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="MinAssignmentCount" HeaderText="Min Assignment Count" SortExpression="MinAssignmentCount" />
                <asp:BoundField DataField="CurrentCompleted" HeaderText="Current Completed" SortExpression="CurrentCompleted" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>