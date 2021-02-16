﻿<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="WAD_Assignment.Registration" %>

<asp:Content ID="Registration" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

		<asp:ScriptManager ID="scriptmanagerRegister" runat="server"></asp:ScriptManager>  
        <div class="registerbox" id="registerbox">
			<div class="avatar"></div>
			<h1>Register Here</h1>

			<asp:UpdatePanel ID="updatepnl" runat="server">  
				<ContentTemplate> 
					<div class="registerForm">
						<div>
							<asp:Label ID="lblUsername" runat="server" CssClass="registerLabel">Username</asp:Label>
							<asp:TextBox ID="txtUsername" runat="server" placeholder="Enter Your Username" CssClass="txtStyle" AutoPostBack="true"></asp:TextBox>
							<asp:Label ID="lblUsername_Validation" runat="server" CssClass="registerValidation"></asp:Label>
						</div>

						<div>
							<asp:Label ID="lblEmail" runat="server" CssClass="registerLabel">Email Address</asp:Label>
							<asp:TextBox ID="txtEmail" TextMode="Email" runat="server" placeholder="someone@exmaple.com" CssClass="txtStyle" AutoPostBack="true"></asp:TextBox>
							<asp:Label ID="lblEmail_Validation" runat="server" CssClass="registerValidation"></asp:Label>
						</div>

						<div>
							<asp:Label ID="lblPassword" runat="server" CssClass="registerLabel">Password</asp:Label>
							<asp:TextBox ID="txtPassword" TextMode="Password" runat="server" placeholder="Enter Your Password" CssClass="txtStyle"></asp:TextBox>
							<i class="fas fa-eye" onclick="passwordFunction()"></i>	
							<asp:Label ID="lblPassword_Validation" runat="server" CssClass="registerValidation"></asp:Label>
						</div>
						
						<asp:Label ID="lblGender" runat="server" CssClass="registerLabel">Gender</asp:Label>
						<br/>
							<div class="gender">
								<asp:RadioButton GroupName="gender" ID="rdMale" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
								<label id="lblMale" class="registerGenderLabel" onClick="selectMale()">Male</label>

								<asp:RadioButton GroupName="gender" ID="rdFemale" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
								<label id="lblFemale" class="registerGenderLabel" onClick="selectFemale()">Female</label>
							</div>
							<asp:Label ID="lblGender_Validation" runat="server" CssClass="registerValidation"></asp:Label>
						<br/>

						<asp:Label ID="lblRole" runat="server" CssClass="registerLabel">Role</asp:Label>
						<br/>
							<div class="role">
								<asp:RadioButton GroupName="role" ID="rdArtist" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
								<label id="lblArtist" class="registerRoleLabel" onClick="selectArtist()">Artist</label>

								<asp:RadioButton GroupName="role" ID="rdCustomer" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
								<label id="lblCustomer" class="registerRoleLabel" onClick="selectCustomer()">Customer</label>
							</div>
							<asp:Label ID="lblRole_Validation" runat="server" CssClass="registerValidation"></asp:Label>
						<br/>
						
						<asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btnStyle" OnClick="btnRegister_Click"/>
					</div>
				</ContentTemplate>  
			</asp:UpdatePanel> 

			<div style="text-align: center; margin-top: 15px;">
				<a href="Login.aspx">Login</a><br/>
				<a href="Homepage.aspx">Cancel Register</a>
			</div>
		</div>

		<div class="isLogin" id="isLogin">
			<asp:Button ID="btnLogin" runat="server" Text="Login?" CssClass="btnProceedStyle btnLogin"/>
			<asp:Button ID="btnHomepage" runat="server" Text="Homepage?" CssClass="btnProceedStyle btnHomepage" PostBackUrl="~/Homepage.aspx"/>
		</div>

        <img src="img/login_register/nightCity_Register.jpg" />
    </section>

	<script type="text/javascript">
		//Window Load (change ui after register)
		window.addEventListener("load", function () {

			selectMale();
			selectCustomer();

			document.getElementById("isLogin").style.display = "none";

            const queryString = window.location.search;
			const urlParams = new URLSearchParams(queryString);

            if (urlParams.has('successRegister')) {
                const successRegister = urlParams.get('successRegister')

                if (successRegister === "true") {
					document.getElementById("registerbox").style.display = "none";
					document.getElementById("isLogin").style.display = "flex";
				} else {
					document.getElementById("isLogin").style.display = "none";
                    document.getElementById("registerbox").style.display = "block";
                }
            }
		})

        //Show Password
		function passwordFunction() {
            var x = document.getElementById('<%= txtPassword.ClientID %>');

			if (x.type == "password") {
				x.setAttribute("type", "text");

			} else {
                x.setAttribute("type", "password");
            }
		}

		//Select the gender
		function selectMale() {
			document.getElementById("<%=rdMale.ClientID %>").checked = true;
			document.getElementById("lblMale").classList.add("checked");

			document.getElementById("<%=rdFemale.ClientID %>").checked = false;
            document.getElementById("lblFemale").classList.remove("checked");
		}

		function selectFemale() {
			document.getElementById("<%=rdMale.ClientID %>").checked = false;
            document.getElementById("lblMale").classList.remove("checked");

			document.getElementById("<%=rdFemale.ClientID %>").checked = true;
            document.getElementById("lblFemale").classList.add("checked");
		}

		//Select role
		function selectArtist() {
            document.getElementById("<%=rdArtist.ClientID %>").checked = true;
            document.getElementById("lblArtist").classList.add("checked");

            document.getElementById("<%=rdCustomer.ClientID %>").checked = false;
            document.getElementById("lblCustomer").classList.remove("checked");
		}

		function selectCustomer() {
            document.getElementById("<%=rdArtist.ClientID %>").checked = false;
            document.getElementById("lblArtist").classList.remove("checked");

            document.getElementById("<%=rdCustomer.ClientID %>").checked = true;
            document.getElementById("lblCustomer").classList.add("checked");
        }
    </script>

</asp:Content>
