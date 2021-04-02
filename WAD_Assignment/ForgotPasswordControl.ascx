﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForgotPasswordControl.ascx.cs" Inherits="WAD_Assignment.ForgotPasswordControl" %>

<div class="ForgotPasswordForm">
	<div class="ForgotPasswordBox">
		<h1>Forgot Password</h1>
		<div class="userPic"></div>	
				
		<asp:ScriptManager ID="scriptmanagerForgotPassword" runat="server"/>
				
		<asp:UpdatePanel ID="updatepnl" runat="server">  
			<ContentTemplate> 
				<div class="ForgotPasswordFormContent">
					<div style="margin-bottom: 15px;">
						<p>Email</p>
						<asp:TextBox ID="txtEmail" runat="server" placeholder="Enter Your Email"></asp:TextBox>
								
						<asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
							ControlToValidate="txtEmail" ErrorMessage="Please Enter Your Email" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>

						<asp:CustomValidator ID="cvEmail" runat="server" 
							ControlToValidate="txtEmail"
							OnServerValidate="cvEmail_ServerValidate"
							ForeColor="Red"
							Display="Dynamic">
						</asp:CustomValidator>

					</div>
							
					<div style="margin-bottom: 15px;">
						<p>Reset PIN</p> 
						<div>
							<asp:TextBox ID="txtResetPin" runat="server" placeholder="Enter Your PIN" TextMode="Number"></asp:TextBox>
						</div>
							
						<asp:Label ID="lblPin" runat="server" CssClass="ForgotPasswordValidation"></asp:Label>
					</div>

					<asp:Button ID="btnReset" runat="server" Text="Request Pin" OnClientClick="changeRequestText()"  OnClick="btnReset_Click"/>	
					<input type="hidden" id="hiddenResetPasswordValue" runat="server" />
				</div>
			</ContentTemplate>  
		</asp:UpdatePanel> 
					
		<a href="Login.aspx">Proceed to Login</a>
		<br />
		<a href="Homepage.aspx">Proceed to Homepage</a>
	</div>
</div>

<img src="img/login_register/nightCity_Login.jpg" style="left: 0; top: 0" />

<script type="text/javascript">
	//Change the text when user click the btn
	function changeRequestText() {

        if (document.getElementById('<%=txtEmail.ClientID%>').value != "" && document.getElementById('<%=txtResetPin.ClientID%>').value === "") {
			document.getElementById('<%=btnReset.ClientID%>').value = "Sending PIN";
			document.getElementById('<%=btnReset.ClientID%>').style.cursor = "default";

		} else if (document.getElementById('<%=btnReset.ClientID%>').value === "Enter New Password") {
            document.getElementById('<%=btnReset.ClientID%>').value = "Getting Password";
            document.getElementById('<%=btnReset.ClientID%>').style.cursor = "default";
        }
        else if (document.getElementById('<%=btnReset.ClientID%>').value === "Reset Password"){
            document.getElementById('<%=btnReset.ClientID%>').value = "Resetting Password";
            document.getElementById('<%=btnReset.ClientID%>').style.cursor = "default";
        }
	}

	//Store the user's new password (to be call in code behind)
    function storeResetPassword(newPassword) {
		document.getElementById("<%=hiddenResetPasswordValue.ClientID%>").value = newPassword;
	}
</script>