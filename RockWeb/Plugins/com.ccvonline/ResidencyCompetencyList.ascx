<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyCompetencyList" %>

<asp:UpdatePanel ID="upList" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gList" />
    </Triggers>
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />

        <Rock:GridFilter ID="rFilter" runat="server">
            <asp:UpdatePanel ID="upFilter" runat="server">
                <ContentTemplate>
                    <Rock:LabeledDropDownList ID="ddlResidencyPeriod" runat="server" LabelText="Period" AutoPostBack="true" OnSelectedIndexChanged="ddlResidencyPeriod_SelectedIndexChanged" />
                    <Rock:LabeledDropDownList ID="ddlResidencyTrack" runat="server" LabelText="Track" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </Rock:GridFilter>

        <Rock:Grid ID="gList" runat="server" AllowSorting="true" OnRowSelected="gList_Edit" DataKeyNames="Id">
            <Columns>
                <Rock:DateField DataField="ResidencyTrack.ResidencyPeriod.Name" HeaderText="Period" SortExpression="ResidencyTrack.ResidencyPeriod.Name" />
                <Rock:DateField DataField="ResidencyTrack.Name" HeaderText="Track" SortExpression="ResidencyTrack.Name" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>
