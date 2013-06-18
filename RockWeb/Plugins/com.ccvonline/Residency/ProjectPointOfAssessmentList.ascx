﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectPointOfAssessmentList.ascx.cs" Inherits="RockWeb.Plugins.com.ccvonline.Residency.ProjectPointOfAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfProjectId" runat="server" />
        <Rock:Grid ID="gList" runat="server" OnRowSelected="gList_Edit" DataKeyNames="Id">
            <Columns>
                <Rock:ReorderField />
                <asp:BoundField DataField="AssessmentOrder" HeaderText="#" />
                <asp:BoundField DataField="AssessmentText" HeaderText="Text" />
                <Rock:DeleteField OnClick="gList_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>
