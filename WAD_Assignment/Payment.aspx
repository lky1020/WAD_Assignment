<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="WAD_Assignment.Payment" %>

<asp:Content ID="Payment" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="payment_wrap">
        <div class="payment_wrapper">
           
            <div class="order_info">
                <div class="order_info_content">
                    <p><b>Order Summary </b></p>
                    <asp:GridView ID="gvPayment" runat="server"  AutoGenerateColumns="false" ForeColor="White" DataKeyNames="ArtId"
                         GridLines ="none" CssClass="pay_gv">
                            <Columns>
                                <%-- GridView CheckBox --%>
                                <asp:TemplateField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div class="pay_gvItem1">
                                            <strong>  
                                             <asp:Label runat="server" Text='<%# Eval("ArtName") %>' Width="60%"></asp:Label>
                                             <asp:TextBox runat="server" ID="artItem_Name" Text='<%# Eval("ArtName") %>' Visible="false"></asp:TextBox>
                                            </strong> 
                                            <br />
                                            <div class="pay_gvItem2">
                                                <asp:Label runat="server" ID="item_order_summary_price" >RM <%# Eval("Price") %> x</asp:Label>
                                                <asp:Label runat="server"> <%# Eval("qtySelected") %></asp:Label>
                                                <asp:TextBox runat="server" ID="item_order_summary_qty" Text='<%# Eval("qtySelected") %>' Visible="false"></asp:TextBox>
                                             </div>
                                         </div>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <div class="pay_gvItem3">
                                        <strong> 
                                            <asp:Label runat="server" >  RM <%# Eval("Subtotal") %></asp:Label>
                                            <asp:TextBox runat="server" ID="order_subtotal" Text='<%# Eval("Subtotal") %>' Visible="false">  </asp:TextBox>
                                        </strong> 
                                            </div>
                                    </ItemTemplate>
                                   </asp:TemplateField> 
                                
                            </Columns>
                       </asp:GridView>
                      
                         <div>
                        <table>
                            <tr>
                                <td>
                                     <asp:Label runat="server" >Total Art Price</asp:Label>
                                     
                                </td>
                                <td>
                                     <asp:Label runat="server" Width="40px"> : </asp:Label>
                                     
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="pay_subtotal" > </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                     <asp:Label runat="server"> Delivery Fees</asp:Label>
                                     
                                </td>
                                <td>
                                     <asp:Label runat="server" Width="40px"> : </asp:Label>
                                     
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="deliverly_fees" > </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                
                                <td>
                                    <strong>
                                      <asp:Label runat="server" >Total Payment</asp:Label>
                                    </strong>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label runat="server" Width="40px"> : </asp:Label>
                                    </strong>
                                     
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label runat="server" ID="total_payment" > </asp:Label>
                                    </strong>
                                </td>
                                
                            </tr>
                        </table>
                       
                    </div>
                          
                </div>
            </div>
    
          <div class="checkout_form">
              

                <p>Payment Section</p>
                      <asp:RadioButtonList ID="RadioButtonList1" runat="server" Font-Size="Medium" ForeColor="#999999" RepeatDirection="Horizontal" Width="280px">
                          <asp:ListItem>Debit Card </asp:ListItem>
                          <asp:ListItem>Credit Card</asp:ListItem>
                      </asp:RadioButtonList>

                   <div class="payment_section">
                        <input type="text" placeholder="Address" />
                    </div>
                    <div class="payment_section">
                        <input type="text" placeholder="Card Number" />
                    </div>
                    <div class="payment_section">
                        <input type="text" placeholder="Cardholder Name" />
                    </div>
                    <div class="payment_section payment_last_section">
                        <div class="payment_item">
                            <input type="text" placeholder="Expiry Date" />
                        </div>
                        <div class="payment_item">
                            <input type="text" placeholder="CVV" />
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="pay_Btn" Text="Pay" class="payment_btn" runat="server" OnClick="pay_Btn_Click" />
                     </div>
                   
            </div>

        </div>
        </div>
</asp:Content>
