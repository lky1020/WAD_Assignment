<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="PayHistory.aspx.cs" Inherits="WAD_Assignment.PayHistory" %>

<asp:Content ID="Payment" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <div class="payHis_page">
        <div class="payHis_page_title">
            <h1> PURCHASE HISTORY </h1>
        </div>
        <br /> <br />
        <%-- Payment History GridView (Table) --%>

        <asp:GridView ID="gvPayHistory" runat="server"  AutoGenerateColumns="false" ForeColor="White" DataKeyNames="paymentId"
             GridLines ="none" CssClass="payHis_gv">
            <Columns>
                <%-- GridView ArtImage --%>
                <asp:TemplateField ItemStyle-Width="14%" HeaderText="Date Paid" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate >  
                        <asp:Label ID="his_artImg" runat="server">
                            <%# Eval("datePaid", "{0:MM/dd/yyyy}") %>
                        </asp:Label>  
                    </ItemTemplate>
                </asp:TemplateField> 

                <%-- Art Name --%>
                <asp:TemplateField ItemStyle-Width="20%" HeaderText="Art Name" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                        <asp:Label ID="his_artName" runat="server"> 
                            <p style="font-size:18px"><%#Eval("ArtName")%></p>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <%-- Unit Price --%>
                <asp:TemplateField ItemStyle-Width="20%" HeaderText="Unit Price" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                            <asp:Label runat="server"> RM <%# Eval("Price") %></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <%-- Qty --%>
                <asp:TemplateField ItemStyle-Width="20%" HeaderText="Qty" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                        <asp:Label ID="his_itemqty" runat="server"> 
                            <%#Eval("qty")%>
                        </asp:Label> 
                    </ItemTemplate>
                </asp:TemplateField>

               
                <%-- GridView Art Subtotal --%>
                <asp:TemplateField ItemStyle-Width="11%" HeaderText="Paid Amount" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                         <asp:Label runat="server"> RM <%# Eval("total") %></asp:Label>
                        </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>

