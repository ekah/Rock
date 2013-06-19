<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompetencyPersonProjectDetail.ascx.cs" Inherits="RockWeb.Plugins.com_ccvonline.Residency.CompetencyPersonProjectDetail" %>

<asp:UpdatePanel ID="upCompetencyPersonProjectDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">

            <asp:HiddenField ID="hfCompetencyPersonProjectId" runat="server" />
            <asp:HiddenField ID="hfCompetencyPersonId" runat="server" />

            <div id="pnlEditDetails" runat="server" class="well">

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />
                <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

                <fieldset>
                    <legend>
                        <asp:Literal ID="lActionTitle" runat="server" />
                    </legend>

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
                    
                    <Rock:LabeledText ID="lblPersonName" runat="server" LabelText="Resident" />
                    <Rock:LabeledText ID="lblCompetency" runat="server" LabelText="Competency" />
                    <Rock:LabeledText ID="lblProject" runat="server" LabelText="Project" />
                    <Rock:DataDropDownList ID="ddlProject" runat="server" DataTextField="Name" DataValueField="Id" SourceTypeName="com.ccvonline.Residency.Model.Project, com.ccvonline.Residency" PropertyName="Name" Required="true"/> 

                </fieldset>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </div>

            <fieldset id="fieldsetViewDetails" runat="server">
                <legend>Project - Assignments
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
