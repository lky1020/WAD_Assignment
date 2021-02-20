<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="ArtWorkDetails.aspx.cs" Inherits="WAD_Assignment.ArtWorks.ArtWorkDetails" %>

<asp:Content ID="ArtWorkDetails" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <img src="img/artwork/art_about.jpg" class="art-details-bg" />
    <div class="details-container">

        <div class="details-left">
            <asp:Image ID="dArtDetailsImage" runat="server" CssClass="details-image" />
        </div>

        <div class="details-right">
            <%-- Art Name --%>
            <asp:Label ID="dArtName" runat="server" CssClass="details-artname"></asp:Label>
            <p class="details-price-str">PRICE</p>

            <%-- Price --%>
            <asp:Label ID="dArtPrice" runat="server" CssClass="details-price"></asp:Label>


            <hr style="margin: 20px 2px 0px 2px;" />

            <%-- Add To Cart & WishList Button --%>
            <div style="justify-content: center">
                <table class="details-btn">
                    <tr>
                        <td>
                            <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="art-to-cart-btn add-btn-large" OnClick="addToCartBtn_Click" /></td>
                        <td>
                            <asp:ImageButton runat="server" AlternateText="Add to WishList" ImageUrl="img/wishlist/heart-icon-inactive.png" CssClass="love-btn-medium" CommandArgument='<%# Eval("ArtId")%>' CommandName="addtowishlist" OnClick="loveBtn_Click" /></td>
                    </tr>
                </table>
            </div>
            <br />
            <hr style="margin: 0px 2px 0px 2px;" />
        </div>

        <div>
            <asp:ImageButton ID="detailsCancelBtn" runat="server" ImageUrl="~/img/artwork/icons8-cancel.png"  OnClick="detailsCancelBtn_Click" CssClass="cancel-btn"/>
        </div>
    </div>

    <%-- Art Description--%>
    <div class="details-box">

        <table class="details-about">
            <tr>
                <td>
                    <img src="img/artwork/icons8-paint-brush.png" /></td>
                <td style="color: white;"><span>A</span>bout the art</td>
            </tr>
        </table>

        <div class="details-desc-box">
            <asp:Label ID="dAboutArt" runat="server" CssClass="details-artabout"></asp:Label>
        </div>
    </div>

    <%-- Artist Description--%>
    <div class="details-box">
        <div class="details-box-border">
            <table class="details-about">
                <tr>
                    <td>
                        <img src="img/artwork/icons8-artist.png" /></td>
                    <td style="color: white;"><span>A</span>rtist</td>
                </tr>
            </table>

            <%-- Artist Image --%>
            <table style="width: 550px; margin-left:auto;margin-right:auto;">
                <tr>
                    <td style="width: 220px;">
                            <asp:Image ID="dArtistImage" runat="server" CssClass="details-artist-img" />
                    </td>

                    <%-- Artist Description --%>
                    <td style="height: 60px;padding-left: 5%;">
                        <%-- Artist Name --%>
                        <asp:Label ID="dArtistName" runat="server" CssClass="details-artist-name"></asp:Label><br />
                        <asp:Label ID="dBio" CssClass="details-bio" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>


</asp:Content>
