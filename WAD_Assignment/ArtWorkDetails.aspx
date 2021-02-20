<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="ArtWorkDetails.aspx.cs" Inherits="WAD_Assignment.ArtWorks.ArtWorkDetails" %>

<asp:Content ID="ArtWorkDetails" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="details-container">

        <div class="details-left">
            <asp:Image ID="dArtDetailsImage" runat="server" ImageUrl="~/img/Artist/summer.png" CssClass="details-image" />
        </div>

        <div class="details-right">
            <%-- Art Name --%>
            <asp:Label ID="dArtName" runat="server" Text="ART WORK NAME" CssClass="details-artname"></asp:Label>
            <p class="details-price-str">PRICE</p>

            <%-- Price --%>
            <asp:Label ID="dArtPrice" runat="server" Text="RM 500.00" CssClass="details-price"></asp:Label>


            <hr style="margin: 20px 2px 0px 2px;" />

            <%-- Add To Cart & WishList Button --%>
            <div style="justify-content: center">
                <table class="details-btn">
                    <tr>
                        <td>
                            <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="art-to-cart-btn add-btn-large"  OnClick="addToCartBtn_Click"/></td>
                        <td>
                            <asp:ImageButton runat="server" AlternateText="Add to WishList" ImageUrl="img/wishlist/heart-icon-inactive.png" CssClass="love-btn-medium" CommandArgument='<%# Eval("ArtId")%>' CommandName="addtowishlist" OnClick="loveBtn_Click"/></td>
                    </tr>
                </table>
            </div>
            <br />
            <hr style="margin: 0px 2px 0px 2px;" />
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
            <asp:Label ID="dAboutArt" runat="server" Text="This series of paintings was made after a trip to New York where Tarek exhibited for the second time. On the spot, he realized live painting and the idea came to him to recover objects, papers, 
                        stickers and all sorts of elements collected in the street. Back in France, he painted several paintings associating these 'little moments of New York' with his imagination. 
                        A recurring character returns to several canvases to create matches. Most of the works have been painted with acrylic, inks, posca and collages."
                CssClass="details-artabout"></asp:Label>
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
            <div class="artist-pro-desc-box">
                <div class="details-artist-left">
                    <div class="artist-img-box">
                        <asp:Image ID="dArtistImage" runat="server" ImageUrl="~/img/Artist/summer.png" CssClass="details-artist-img" />
                    </div>
                </div>

                <%-- Artist Description --%>
                <div class="details-artist-right">
                    <div class="artist-desc-box">
                        <%-- Artist Name --%>
                        <asp:Label ID="dArtistName" runat="server" Text="Lee Ling" CssClass="details-artist-name"></asp:Label>
                        <br /><br />

                        <%-- Artist Bio --%>
                        <asp:Label ID="dBio" CssClass="details-bio" runat="server" Text="Kari Bienert is an Australian painter who excels in the use of color configurations and the art of transforming geometric and curvilinear forms. She explores the unlimited potentiality of chromatic and tonal scales, visual planes and volume within a two-dimensional framework. Viewing her dynamic works of art is an exciting kinetic experience similar to viewing patterns through a kaleidoscope."></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
