<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="WishList.aspx.cs" Inherits="WAD_Assignment.WishList" %>

<asp:Content ID="WishList" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        
        <div>
            <img src="img/wishlist/wishlist_header.png" alt="" class="artwork-gallery-header-bg" />

            <div id="wishlist-header-text" class="wishlist-header-text">
                WishList
            </div>
            <p class="wishlist-header-quoto">Art is a line around your thoughts.</p>
            <!-- Continue Add Art -->
            <asp:Button ID="btnContinueWL" runat="server" Text="View Art"  CssClass="btn-wlArt" OnClick="btnContinueWL_Click"/>
        </div>
<div class="wl-container">
         

        <div class="wishlist-content">
            <!-- WishList Table -->
            <div class="wl-table-box">
            <asp:GridView ID="gvWishList" runat="server" AutoGenerateColumns="False" DataKeyNames="WishlistId" ShowHeaderWhenEmpty="True" CssClass="gv-wishList" AllowPaging="true" PageSize="5" OnRowDeleting="gvWishList_RowDeleting">
                <PagerSettings Mode="NextPreviousFirstLast" Visible="false" />
                <Columns>
                    <%-- WishList ID Col--%>
                    <asp:TemplateField HeaderText="ID" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("WishListId") %>' runat="server" />
                        </ItemTemplate>                  
                    </asp:TemplateField>

                    <%-- GridView ArtImage --%>
                    <asp:TemplateField HeaderText="Art" ItemStyle-Width="20%" HeaderStyle-Width="20%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="wl_artImg" runat="server">
                            <img src='<%# Eval("ArtImage")%>' alt='<%#Eval("ArtImage")%>' height="160"  
                                width="160" style="margin:20px"/>  
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- GridView ArtName & Desc --%>
                    <asp:TemplateField HeaderText="Description" HeaderStyle-Width="40%" ItemStyle-Width="40%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="wl_artName" runat="server"> 
                                <h3><%#Eval("ArtName")%></h3>
                            </asp:Label>
                            <br />
                            <asp:Label ID="wl_artDes" runat="server"> 
                                <%#Eval("ArtDescription")%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- GridView Art Price --%>
                    <asp:TemplateField HeaderText="Price" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="wl_price" runat="server"> 
                                 RM <%# String.Format("{0:0.00}", Eval("Price")) %>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- GridView Data Added --%>
                    <asp:TemplateField HeaderText="Date Added" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>     
                            <asp:Label ID="wl_dateAdded" Text='<%# Eval("DateAdded", "{0:dd/MM/yyyy}") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <%-- Action --%>
                    <asp:TemplateField HeaderText=" " ItemStyle-Width="14%" HeaderStyle-Width="14%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton class="wl_page_deleteBtn" ImageUrl="img/wishlist/icons8-delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="40px" Height="40px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
            <br />
            </div></div>
            <!-- Paging -->
            <table class="paging" id="paging">
                <tr>
                    <td>
                        <asp:Button ID="btnFirstWL" runat="server" Text="FIRST" OnClick="btnFirstWL_Click" CssClass="page-btn" />
                    </td>

                    <td>
                        <asp:Button ID="btnPreviousWL" runat="server" Text="PREV" OnClick="btnPreviousWL_Click" CssClass="page-btn" />
                    </td>

                    <td>
                        <asp:Button ID="btnNextWL" runat="server" Text="NEXT" OnClick="btnNextWL_Click" CssClass="page-btn" />
                    </td>

                    <td>
                        <asp:Button ID="btnLastWL" runat="server" Text="LAST" OnClick="btnLastWL_Click" CssClass="page-btn" />
                    </td>
                </tr>
            </table>
        
    </section>
</asp:Content>
