<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WAD_Assignment.Profile" %>

<asp:Content ID="Profile" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

        <asp:ScriptManager ID="scriptmanagerProfile" runat="server"></asp:ScriptManager>

        <div class="profileBar" id="profileBar">
            <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Conditional">  
			    <ContentTemplate> 
                    <div id="profileContent" class="profileContent" >
                        <div class="profileBarImgBox" id="profileBarImgBox">
                            <img id="userPic" src="img/userPic/user_default.png" alt="Profile Pic" />
                        </div>

                        <div class="editIcon">
                            <div id="editIconBox" class="editIconBox"></div>
                        </div>

                        <div class="profileBarContent">
                            <div id="profileName">
                                <asp:Label ID="lblProfileName" runat="server" Text="testing"></asp:Label>
                            </div>

                            <div id="userBio" class="userBio">
                                <textarea id="txtAreaUserBio" readonly="readonly" runat="server"></textarea>
                            </div>

                            <div id="editBio" class="editBio">
                                Edit Your BIO here:<br />
                                <textarea maxlength="50" id="txtAreaEditBio" runat="server"></textarea><br />
                                <label id="lblBioMaxLen" style="font-size: 12px;">Only 50 Character are Allowed</label>
                            </div>

                            <div id="profileBarBtnStyle">
                                <div style="display: inline; width: 350px;">
                                    <asp:Button ID="btnEditBio" CssClass="editBioBtnStyle" runat="server" Text="Edit Bio" OnClick="btnEditBio_Click"></asp:Button>
                                    <asp:Button ID="btnCancelEditBio" CssClass="editBioBtnStyle" runat="server" Text="Cancel" OnClick="btnCancelEditBio_Click"/>
                                </div>
                                
                                <Button ID="btnEditPassword" Class="profileBtn profileBarBtnStyle" >Edit Password</Button>
                                <asp:Button ID="btnManageArt" CssClass="profileBtn profileBarBtnStyle" runat="server" Text="Manage Art"></asp:Button>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>  
		    </asp:UpdatePanel> 
        </div>

        <div class="fileUploadBox">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">  
			    <ContentTemplate> 
                    <p style="text-align: center; font-weight: 700; font-size: 24px; color: #ff0157;">Upload Images:</p>

                    <div class="fileUploadBoxContent">

                        <div class="previewImgBox" id="previewImgBox">
                            <img id="previewPic" src="img/userPic/user_default.png" alt="Profile Pic" />
                        </div>


                        <div class="fileUploadBtnStyle">
                            <asp:Button ID="btnBrowsePic" runat="server" CssClass="btnUploadPic" Text="Browse Picture" OnClientClick="browsePic()"/><br />
                        </div>
                
                        <div class="btnUploadPicStyle">
                            <asp:Button ID="btnUploadPic" runat="server" CssClass="btnUploadPic" Text="Upload Picture" OnClick="btnUploadPic_Click"/><br />
                        </div>
                    </div>
                </ContentTemplate>  

                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUploadPic" />
                </Triggers>
		    </asp:UpdatePanel> 

            <asp:FileUpload ID="fileUpProfilePic" runat="server" onChange="profilePicPreview(this)" style="display:none;"/>
        </div>

        <img src="img/Profile/white_texture.jpg" />
    </section>

	<script type="text/javascript">
        //Initialize the UI
        window.addEventListener("load", function () {
            //Change the color of logo and toggle to black, to display it
            document.getElementById('logo').style.color = '#111'; 
            document.getElementById('toggle').style.filter = 'invert()';
            document.getElementById('header').style.backgroundColor = '#fff';
            document.getElementById('editBio').style.display = "none";

            //Cancel Edit Bio will not display before user click edit bio btn
            undisplayCancelEditButton();

            //Hide the file upload box
            //$(".fileUploadBox").hide();
        });

        function undisplayCancelEditButton() {
            //Initialize the btnEditBio
            document.getElementById('<%=btnEditBio.ClientID %>').style.width = "350px";

            document.getElementById('<%=btnCancelEditBio.ClientID %>').style.display = "none";
        }

        function changeColorForCancelBtn() {
            document.getElementById('<%=btnCancelEditBio.ClientID %>').style.background = '#8a2be2';
        }

        //Preview the profile pic
        function profilePicPreview(input) {
            if (input.files && input.files[0]) {
                var filedr = new FileReader();

                filedr.onload = function (e) {
                    $('#previewPic').attr('src', e.target.result);
                }

                filedr.readAsDataURL(input.files[0]);
            }
        }

        function browsePic() {
            document.getElementById('<%=fileUpProfilePic.ClientID %>').click();
        }

        const editIconBox = document.querySelector('.editIconBox');

        editIconBox.addEventListener('click', () => {

            $(".fileUploadBox").animate({
                height: "toggle"
            }, 750, function () {
                $(".fileUploadBox").show();
            });
        });

        $(document).mouseup(function (e) {
            if ($(e.target).closest(".fileUploadBox").length === 0) {

                $(".fileUploadBox").hide();

            }
        }); 

    </script>

</asp:Content>
