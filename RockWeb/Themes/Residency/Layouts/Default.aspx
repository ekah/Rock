﻿<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/Themes/Residency/Layouts/Site.Master" 
    AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="main" runat="server">
    
    <!-- Page Header -->
		<header class="navbar navbar-static-top pageheader">
			<div class="navbar-inner">
				<div class="container-fluid">
					<div class="row-fluid">
						<div class="span2 clearfix logo">
	
                            <asp:HyperLink ID="hlLogo" runat="server" CssClass="brand" NavigateUrl="~" ToolTip="Rock ChMS">
                                <asp:Image ID="imgLogo" runat="server" AlternateText="Rock ChMS" ImageUrl="~/Themes/Residency/Assets/Images/residency-logo.svg" CssClass="pageheader-logo" />
                            </asp:HyperLink>
					
						</div>
						
						<div class="span10 clearfix">	
							
							<div class="pageheader-collapse pull-right">
								<a class="btn btn-navbar" data-target=".nav-collapse" data-toggle="collapse">
									<span class="icon-bar"></span>
									<span class="icon-bar"></span>
									<span class="icon-bar"></span>
								</a>
						
								<div class="nav-collapse collapse">
									
									<Rock:Zone ID="Heading" Name="Header" runat="server" />
									
								</div>
							</div> <!-- collapse container -->
							
						</div> <!-- end column -->
					</div> <!-- end row -->

				</div> <!-- end container -->
			</div> <!-- end navbar-inner -->
		</header>
		

		
		<div class="container-fluid body-content">
			<div class="row-fluid">
				<div class="span12">
                       
                    <Rock:PageBreadCrumbs ID="PageBreadCrumbs" runat="server" />

                    <section class="pageoverview clearfix">
                        <Rock:PageIcon ID="PageIcon" runat="server" />
                        <Rock:PageDescription ID="PageDescription" runat="server" />
                    </section>
                       

                    <!-- display any ajax error messages here (use with ajax-client-error-handler.js) -->
                    <div class="alert alert-error ajax-error" style="display:none">
                        <strong>Ooops!</strong>
                        <span class="ajax-error-message"></span>
                    </div>

                    <Rock:Zone ID="Content" runat="server" />
				</div>
			</div>
		</div>
		
		<footer class="page-footer">
			<div class="container-fluid">
				<div class="row-fluid">
					<div class="span12">
						<Rock:Zone ID="Footer" runat="server" />
					</div>
				</div>
			</div>
		</footer>
        
</asp:Content>

