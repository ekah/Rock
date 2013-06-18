﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResidencyProjectPointOfAssessmentList.ascx.cs" Inherits="com.ccvonline.Blocks.ResidencyProjectPointOfAssessmentList" %>

<asp:UpdatePanel ID="upList" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <asp:HiddenField ID="hfResidencyProjectId" runat="server" />
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
