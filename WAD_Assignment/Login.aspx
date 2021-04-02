﻿<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WAD_Assignment.Login" %>
<%@ Register TagPrefix="SuperLogin" TagName="Login" Src="~/LoginControl.ascx" %>

<asp:Content ID="Login" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

        <%-- Login Control --%>
        <SuperLogin:Login ID="ctlLogin" runat="server" />

    </section>

	<script type="text/javascript">
        <%--//Show Password
        function passwordFunction() {
            var x = document.getElementById('<%= ctlLogin.txtPassword.ClientID %>');

			if (x.type == "password") {
				x.setAttribute("type", "text");

			} else {
                x.setAttribute("type", "password");
            }
		}--%>
    </script>

</asp:Content>
