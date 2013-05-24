﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyCompetencyDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.ResidencyCompetencyDetail" %>

<!-- just to help do css intellisense at design time  -->
<link rel="stylesheet" type="text/css" href="~/CSS/bootstrap.min.css" visible="false" />

<asp:UpdatePanel ID="upResidencyCompetencyDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfResidencyCompetencyId" runat="server" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
            <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </legend>

                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                <div class="row-fluid">
                    <div class="span6">
                        <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetency, com.ccvonline.Residency" PropertyName="Name" />
                        <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetency, com.ccvonline.Residency" PropertyName="Description" TextMode="MultiLine" Rows="3" />
                        <Rock:LabeledDropDownList ID="ddlTrack" runat="server" LabelText="Track" />
                        <Rock:PersonPicker ID="ppTeacherOfRecord" runat="server" LabelText="Teacher of Record" />
                        <Rock:PersonPicker ID="ppFacilitator" runat="server" LabelText="Facilitator" />
                    </div>
                    <div class="span6">
                        <Rock:DataTextBox ID="tbGoals" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetency, com.ccvonline.Residency" PropertyName="Goals" TextMode="MultiLine" Rows="3" />
                        <Rock:DataTextBox ID="tbCreditHours" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetency, com.ccvonline.Residency" PropertyName="CreditHours" CssClass="input-mini" />
                        <Rock:DataTextBox ID="tbSupervisionHours" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetency, com.ccvonline.Residency" PropertyName="SupervisionHours" CssClass="input-mini"/>
                        <Rock:DataTextBox ID="tbImplementationHours" runat="server" SourceTypeName="com.ccvonline.Residency.Model.ResidencyCompetency, com.ccvonline.Residency" PropertyName="ImplementationHours" CssClass="input-mini"/>
                    </div>
                </div>

            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click"  />
            </div>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
