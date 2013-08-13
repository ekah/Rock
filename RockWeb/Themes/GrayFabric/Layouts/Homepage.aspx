﻿<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="Site.Master" 
    AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="main" runat="server">
    <link href="/Themes/GrayFabric/Css/flexslider.css" rel="stylesheet">

    <!-- band to provide gray bar at the top of the page -->
    <div id="band">
	  </div>

    <div class="container">
	  	<header>
		  	<div class="row-fluid identity">
			  	<div class="span6">
			  		<img class="logo" src="./assets/img/rocksolidchurchlogo.svg">
                    <asp:HyperLink ID="hlLogo" runat="server" NavigateUrl="~" >
                        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Themes/GrayFabric/Assets/Images/rocksolidchurchlogo.svg" CssClass="logo" />
                    </asp:HyperLink>
			  	</div>
			  	<div class="span3 service-times">
			  		<i class="icon-time"></i> <span class="bold">Service Times</span>
				  	<br><span class="light">Sunday 9am, 10:30am and Noon</span>
			  	</div>
			  	<div class="span3 my-account">My Account</div>
			</div>
	  		
	  		<div class="navbar">
	  			<div class="navbar-inner">
			  		
			  		<!-- .btn-navbar is used as the toggle for collapsed navbar content -->
					<a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
					</a>
			  		
			  		<div class="nav-collapse collapse">
			  		<ul class="nav">
			  			<li><a href="#">Home</a></li>
			  			<li class="divider-vertical"></li>
			  			<li><a href="#">About</a></li>
			  			<li class="divider-vertical"></li>
			  			<li><a href="#">Contact</a></li>
			  			<li class="divider-vertical"></li>
			  		</ul>
			  		</div>
	  			</div>
	  		</div>

	  	</header>

    </div>


    <section class="promo-slider container">	
	  		
	  		<div class="flexslider">
			  <ul class="slides">
			    <li>
			      <a href="#"><img src="./assets/img/mock-header1.jpg" /></a>
			    </li>
			    <li>
			      <a href="#"><img src="./assets/img/mock-header2.jpg" /></a>
			    </li>
			    <li>
			      <a href="#"><img src="./assets/img/mock-header3.jpg" /></a>
			    </li>
			    <li>
			      <a href="#"><img src="./assets/img/mock-header4.jpg" /></a>
			    </li>
			  </ul>
			</div>
			<img class="slider-shadow" src="./assets/img/slider-shadow.png" />

	  	</section>
	  </div>
  	  
  	  <div class="separator"></div>	  
  	  
  	  <section class="promo-secondary container">
  	  	<ul>
  	  		<li><a href="#"><img src="./assets/img/mock-promo-sub1.jpg" /></a></li>
  	  		<li><a href="#"><img src="./assets/img/mock-promo-sub2.jpg" /></a></li>
  	  		<li><a href="#"><img src="./assets/img/mock-promo-sub3.jpg" /></a></li>
  	  		<li><a href="#"><img src="./assets/img/mock-promo-sub4.jpg" /></a></li>
  	  	</ul>
  	  </section>
  	  
  	<section class="contact container">
  		<div class="row-fluid">
  			<section class="span6 social">
	  			<span class="bold">Be Social, Share!</span> Like, comment, tweet, pin and share!
	  			<div class="icons">	
	  				<a href=""><i class="icon-twitter-sign"></i></a>
	  				<a href=""><i class="icon-facebook-sign"></i></a>
	  				<a href=""><i class="icon-google-plus-sign"></i></a>
	  				<a href=""><i class="icon-pinterest-sign"></i></a>
	  				<a href=""><i class="icon-linkedin-sign"></i></a>
	  			</div>
  			</section>
  			<section class="traditional span6">
	  			<div class="map">
	  				<img src="http://maps.google.com/maps/api/staticmap?center=33.590795,-112.126459&zoom=13&markers=33.590795,-112.126459&size=225x225&sensor=false" />
	  			</div>
	  			<div class="info">
	  				<span class="bold">Rock Solid Church</span><br />
	  				<span class="light">
	  					<p>3120 W Cholla St<br />
	  					Phoenix, AZ 85029</p>
	  					
	  					<p>(623) 555-1234</p>
	  					<p><a href="mailto:sample@rockchms.com">info@rockchms.com</a></p>
	  				</span>
	  			</div>
  			</section>
  		</div>
  	</section>  



    <footer>
  		<div class="container">
	  		<section class="author light">
	  			<span>Design by <a href="http://www.voracitysolutions.com">Voracity Solutions</a></span> 
	  			<span>Powered by <a href="http://www.rockchms.com">Rock ChMS</a></span>
	  		</section>
	  		<section class="actions light right-align">
	  			<a>Login</a>
	  		</section>
  		</div>
  	</footer>
   
	<!-- Scripts -->
    <script type="text/javascript" src="../Themes/GrayFabric/Scripts/jquery.flexslider-min.js"></script>
    
     <script>

         $(document).ready(function () {
             $('.flexslider').flexslider({
                 animation: "slide",
                 nextText: '<i class="icon-chevron-right"></i>',
                 prevText: '<i class="icon-chevron-left"></i>',

             });
         });

    </script>

    


    <Rock:Zone ID="Heading" Name="Header" runat="server" />
    <Rock:Zone ID="Menu" runat="server" />	
    

    <Rock:Zone ID="Zone1" runat="server" />
    <Rock:Zone ID="Content" runat="server" />
    <Rock:Zone ID="Footer" runat="server" />


    
                    <!-- display any ajax error messages here (use with ajax-client-error-handler.js) -->
                    <div class="alert alert-error ajax-error" style="display:none">
                        <strong>Ooops!</strong>
                        <span class="ajax-error-message"></span>
                    </div>

     
		
		
        
</asp:Content>

