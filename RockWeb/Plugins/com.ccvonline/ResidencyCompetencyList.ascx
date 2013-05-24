<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyCompetencyList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <Rock:DateField DataField="ResidencyTrack.ResidencyPeriod.Name" HeaderText="Period" SortExpression="ResidencyTrack.ResidencyPeriod.Name" />
                <Rock:DateField DataField="ResidencyTrack.Name" HeaderText="Track" SortExpression="ResidencyTrack.Name" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>