﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyTrackDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.ResidencyTrackDetail" %>

<asp:UpdatePanel ID="upResidencyTrackDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfResidencyTrackId" runat="server" />

            <div id="pnlEditDetails" runat="server" class="well">

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
                <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

                <fieldset>
                    <legend>
                        <asp:Literal ID="lActionTitle" runat="server" />
                    </legend>

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyTrack, com.ccvonline.Residency" PropertyName="Name" />
                    <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyTrack, com.ccvonline.Residency" PropertyName="Description" TextMode="MultiLine" Rows="3" />
                    <Rock:DataDropDownList ID="ddlPeriod" runat="server" DataValueField="Id" DataTextField="Name" SourceTypeName="com.ccvonline.Residency.Model.ResidencyTrack, com.ccvonline.Residency" PropertyName="ResidencyPeriodId" LabelText="Period" />

                </fieldset>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </div>

            <fieldset id="fieldsetViewDetails" runat="server">
                <legend>Track - Competencies
                </legend>
                <div class="well">
                    <div class="row-fluid">
                        <Rock:NotificationBox ID="NotificationBox1" runat="server" NotificationBoxType="Info" />
                    </div>
                    <div class="row-fluid">
                        <asp:Literal ID="lblMainDetails" runat="server" />
                    </div>
                    <div class="actions">
                        <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-mini" OnClick="btnEdit_Click" />
                    </div>
                </div>

            </fieldset>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
