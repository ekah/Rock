<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyProjectList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyProjectList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <asp:BoundField DataField="ResidencyCompetency.ResidencyTrack.ResidencyPeriod.Name" HeaderText="Period" SortExpression="ResidencyCompetency.ResidencyTrack.ResidencyPeriod.Name" />
                <asp:BoundField DataField="ResidencyCompetency.ResidencyTrack.Name" HeaderText="Track" SortExpression="ResidencyCompetency.ResidencyTrack.Name" />
                <asp:BoundField DataField="ResidencyCompetency.Name" HeaderText="Competency" SortExpression="ResidencyCompetency.Name" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>