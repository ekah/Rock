<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidentCompetencyDetail.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.ResidentCompetencyDetail" %>

<asp:UpdatePanel ID="upDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">
            <asp:HiddenField ID="hfCompetencyPersonId" runat="server" />

            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </legend>

                <div class="row-fluid">
                    <h3>
                        <asp:Literal runat="server" ID="lblCompetencyName" />
                    </h3>
                    <div class="well">
                        <dl>
                            <dt>Facilitator</dt>
                            <dd>
                                <asp:Literal runat="server" ID="lblFacilitator" /></dd>
                            <dt>Description</dt>
                            <dd>
                                <asp:Literal runat="server" ID="lblDescription" />
                            </dd>
                        </dl>
                    </div>
                </div>

                <ul class="nav nav-pills">
                    <li runat="server" id="liProjects" class="active">
                        <asp:LinkButton ID="lbProjects" runat="server" CssClass="show-pill" Text="Projects" OnClick="lbProjects_Click" />
                    </li>
                    <li runat="server" id="liGoals" class="">
                        <asp:LinkButton ID="lbGoals" runat="server" CssClass="show-pill" Text="Goals" OnClick="lbGoals_Click" />
                    </li>
                    <li runat="server" id="liNotes" class="">
                        <asp:LinkButton ID="lbNotes" runat="server" CssClass="show-pill" Text="Notes" OnClick="lbNotes_Click" />
                    </li>
                </ul>

                <div class="row-fluid">

                    <asp:Panel ID="pnlProjects" runat="server" Visible="true">
                        <Rock:Grid ID="gProjectList" runat="server" AllowSorting="true" OnRowSelected="gProjectList_RowSelected" DataKeyNames="Id" RowItemText="Project" DisplayType="Light">
                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                <asp:BoundField DataField="AssignedCount" HeaderText="Assignments" SortExpression="AssignedCount" />
                                <asp:BoundField DataField="AssignedCompleted" HeaderText="Completed" SortExpression="AssignedCompleted" />
                                <Rock:BadgeField DataField="AssignedRemaining" HeaderText="Remaining" SortExpression="AssignedRemaining" WarningMin="1" SuccessMax="0" SuccessMin="0" ImportantMin="9999" InfoMin="9999" />
                            </Columns>
                        </Rock:Grid>
                    </asp:Panel>

                    <asp:Panel ID="pnlGoals" runat="server" Visible="false">
                        ToDo:  Goals
                    </asp:Panel>

                    <asp:Panel ID="pnlNotes" runat="server" Visible="false">
                        ToDo:  Notes
                    </asp:Panel>
                </div>

            </fieldset>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
