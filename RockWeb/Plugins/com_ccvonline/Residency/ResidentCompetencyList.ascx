<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentCompetencyList.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentCompetencyList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <h3>
            <asp:Literal ID="lblPersonName" runat="server" /></h3>
        <asp:Repeater ID="rpTracks" runat="server" OnItemDataBound="rpTracks_ItemDataBound">
            <ItemTemplate>
                <h4><%# DataBinder.Eval( Container.DataItem, "Name") %>
                </h4>
                <Rock:Grid ID="gCompetencyList" runat="server" AllowSorting="false" OnRowSelected="gCompetencyList_RowSelected">
                    <Columns>
                        <asp:BoundField DataField="CompetencyName" HeaderText="Competency" SortExpression="CompetencyName" ItemStyle-Width="50%" />
                        <asp:BoundField DataField="CompletedProjectsTotal" HeaderText="Projects Completed" SortExpression="CompletedProjectsTotal" />
                        <asp:BoundField DataField="AssignedProjectsTotal" HeaderText="Total Projects" SortExpression="AssignedProjectsTotal" />
                    </Columns>
                </Rock:Grid>

            </ItemTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
