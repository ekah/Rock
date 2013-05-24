<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyTrackList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyTrackList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id" >
            <Columns>
                <asp:BoundField DataField="ResidencyPeriod.Name" HeaderText="Period" SortExpression="ResidencyPeriod.Name" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>