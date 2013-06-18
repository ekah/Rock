﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordingList.ascx.cs" Inherits="RockWeb.Plugins.com.ccvonline.CommandCenter.RecordingList" %>

<asp:UpdatePanel ID="upRecordings" runat="server">
    <ContentTemplate>

        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:Grid ID="gRecordings" runat="server" EmptyDataText="No Recordings Found" AllowSorting="true" OnRowSelected="gRecordings_Edit">
            <Columns>
                <asp:BoundField HeaderText="Campus" DataField="Campus" SortExpression="Campus" />
                <asp:BoundField HeaderText="Date" DataField="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField HeaderText="Stream" DataField="StreamName" SortExpression="StreamName" />
                <asp:BoundField HeaderText="Label" DataField="Label" SortExpression="Label" />
                <asp:BoundField HeaderText="Recording" DataField="RecordingName" SortExpression="RecordingName" />
                <asp:BoundField HeaderText="Started" DataField="StartTime" SortExpression="StartTime" DataFormatString="{0:MM/dd/yy hh:mm:ss tt}" />
                <asp:BoundField HeaderText="Stopped" DataField="StopTime" SortExpression="StopTime" DataFormatString="{0:MM/dd/yy hh:mm:ss tt}" />
                <asp:BoundField HeaderText="Length" DataField="Length" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbStart" runat="server" Text="Start" CssClass="start-recording" CommandName="START" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbStop" runat="server" Text="Stop" CssClass="stop-recording" CommandName="STOP" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <Rock:DeleteField OnClick="gRecordings_Delete" />
            </Columns>
        </Rock:Grid>

    </ContentTemplate>
</asp:UpdatePanel>

