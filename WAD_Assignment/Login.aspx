<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WAD_Assignment.Login" %>

<asp:Content ID="Login" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

        <div class="loginForm">
			<div class="loginbox">
				<h1>Login Account</h1>
				<div class="userPic"></div>	
				
				<asp:ScriptManager ID="scriptmanagerLogin" runat="server"></asp:ScriptManager>
				
				<asp:UpdatePanel ID="updatepnl" runat="server">  
					<ContentTemplate> 
						<div class="loginFormContent">
							<div style="margin-bottom: 15px;">
								<p>Email or Username</p>
								<asp:TextBox ID="txtEmail_Username" runat="server" placeholder="Enter Email/Username" AutoPostBack="true"></asp:TextBox>
								<asp:Label ID="lblEmail_Username" runat="server" CssClass="loginValidation"></asp:Label>
							</div>
							
							<div style="margin-bottom: 15px;">
								<p>Password</p> 
								<div>
									<asp:TextBox ID="txtPassword" runat="server" placeholder="Enter Your Password" TextMode="Password" CssClass="inlinePassword"></asp:TextBox>
									<i class="fas fa-eye inlinePassword" onclick="passwordFunction()"></i>	
								</div>
							
								<asp:Label ID="lblPassword" runat="server" CssClass="loginValidation"></asp:Label>
							</div>

							<asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />	
						</div>
					</ContentTemplate>  
				</asp:UpdatePanel> 
					
				<a href="#">Forget Password</a>
				<br/>
				<a href="Registration.aspx">Create a new Account</a>
			</div>
		</div>

        <img src="img/login_register/nightCity_Login.jpg" />
    </section>

	<script type="text/javascript">
        //Show Password
        function passwordFunction() {
            var x = document.getElementById('<%= txtPassword.ClientID %>');

			if (x.type == "password") {
				x.setAttribute("type", "text");

			} else {
                x.setAttribute("type", "password");
            }
		}
    </script>

</asp:Content>
