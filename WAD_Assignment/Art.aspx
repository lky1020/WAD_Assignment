<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="Art.aspx.cs" Inherits="WAD_Assignment.Art" %>

<asp:Content ID="Art" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--Script for preview Image Start-->
    <script type="text/javascript">  
  
        function showimagepreview(input) {  
  
            if (input.files && input.files[0]) {  
                var reader = new FileReader();  
                reader.onload = function (e) {  
  
                    document.getElementsByTagName("img")[1].setAttribute("src", e.target.result);                     
                }  
                reader.readAsDataURL(input.files[0]);  
            }  
        }  
  
    </script>  
     <!--Script for preview Image End-->

    <div class="bg-Art">
        <img src="img/Artist/bg1.jpg" alt="A Men">
    </div>

     <section class="postArtTbl" id="postTbl">

         <div class="addpostform" id="addpostform">
              <h2 class="postttl">Add a New Art</h2>
              <p class="postSubTtl">Please choose the right category for your art</p>
             <hr class="ArtSepline"/>
              <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Category]"></asp:SqlDataSource>
         </div>
            
         <div class="ViewArt">
            <asp:Button class="btnViewArt" ID="btnViewArt" runat="server" Text="View Art" ForeColor="	#FFFFFF" OnClick="btnViewArt_Click" />
         </div>   
         
        <div class="formAddArt">

            <table class="tblArtInfo">
                <tr>
                    <td>
                        <p> Name of Art </p>
                    </td>
                </tr>

                <tr>
                    <td>
                    <asp:TextBox ID="txtBoxArtName" runat="server" CssClass="txtBoxArtName" OnTextChanged="txtBoxArtName_TextChanged"></asp:TextBox>
                    </td>

                    <td rowspan="12">
                        <div class="artImgBox">
                            <img id="artImg" src="img/homepage/artWork/beautiful_women.jpg" alt="Beautiful Women">
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Category of Art </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:DropDownList ID="ddlCatArt" runat="server" CssClass="ddlCatArt" OnSelectedIndexChanged="ddlCatArt_SelectedIndexChanged" DataSourceID="SqlDataSource1" DataTextField="CategoryName" DataValueField="CategoryID"></asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Description </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtBoxArtDesc" runat="server"  textmode="Multiline"  CssClass="txtBoxArtDesc" OnTextChanged="txtBoxArtDesc_TextChanged"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Price (RM)</p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtBoxArtPrice" type="number" runat="server" CssClass="txtBoxArtName" OnTextChanged="txtBoxArtPrice_TextChanged"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Quantity </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtBoxArtQuantity" type="number" runat="server" CssClass="txtBoxArtName" OnTextChanged="txtBoxArtPrice_TextChanged"></asp:TextBox>
                         
                    </td>
                </tr>


                <tr>
                    <td>
                        <p> Upload Art </p>
                    </td>
                </tr>

                <tr>
                    <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" onchange="showimagepreview(this)"/>
                       
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Button class="btnViewArtSubmit" ID="btnViewArtSubmit" runat="server" Text="Submit" ForeColor="#FFFFFF" OnClick="btnViewArtSubmit_Click1" />
                    </td>
                </tr>

            </table>
        </div>
     </section>
</asp:Content>
