using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class Payment : System.Web.UI.Page
    {
        String[] orderItem;
        string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
        double totalPay = 0;
        Int32 currentUser = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                payment_refreshdata();

                //calculate prices
                totalPay += double.Parse(orderItem[orderItem.Length - 1]);
                pay_subtotal.Text = "RM " + orderItem[orderItem.Length - 1];

                totalPay += checkDeliveryFees();
                deliverly_fees.Text = "RM " + checkDeliveryFees().ToString("F");
                total_payment.Text = "RM " + totalPay.ToString("F");

            }
            else
            {
                payment_refreshdata();
                //Response.Write("<script>alert('Passing Data UnSucessfull')</script>");

            }
        }

        public void payment_refreshdata()
        {
            //retrieve pass id of order item from cart
            String str = Request.QueryString["checkedItem"];
            orderItem = str.Split(' ');


            //detect current user id
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                string query1 = "Select UserId FROM [dbo].[User] WHERE Role = 'Customer' AND LoginStatus = 'Active' ";
                using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                {
                    currentUser = ((Int32?)cmd1.ExecuteScalar()) ?? 0;
                }
                conn.Close();

            }

            //pass data into grid
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            DataTable dt = new DataTable();

            for (int i = 0; i < (orderItem.Length - 1); i++)
            {
                String query = "Select a.ArtId, a.ArtName, a.Price, o.OrderDetailId, o.qtySelected, o.Subtotal from [OrderDetails] o " +
                    "INNER JOIN [Artist] a on o.ArtId = a.ArtId Where o.OrderDetailId = @id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", orderItem[i]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                gvPayment.DataSource = dt;
                gvPayment.DataBind();
            }
            con.Close();


        }

       
        public double checkDeliveryFees()
        {

            double deliveryFees = 0;

            //cal delivery fees
            deliveryFees = (orderItem.Length - 1) * 3;
            return deliveryFees;
        }

        protected void pay_Btn_Click(object sender, EventArgs e)
        {
            Int32 cartID = 0;

            SqlConnection con = new SqlConnection(cs);
            con.Open();

            //create new cart for status = 'pending'
            String status = "pending";
            string sql = "INSERT into Cart (UserId, status) values('" + currentUser + "', '" + status + "')";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            cmd.ExecuteNonQuery(); 


            //retrieve 'pending' cartid 
            string queryFindCartID = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + currentUser + "'AND status = 'pending'";

            using (SqlCommand cmdCheckCart = new SqlCommand(queryFindCartID, con))
            {
                cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
            }


            //update selected item with the cartid 
            for (int i = 0; i < gvPayment.Rows.Count; i++)
            {
                String queryUpdate = "Update OrderDetails SET CartId=@cartid, Subtotal=@subtotal WHERE OrderDetailId =@detailid";

                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con);

                double qty = double.Parse((gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim()) * 3;
                double subtotal = qty + double.Parse((gvPayment.Rows[i].FindControl("order_subtotal") as TextBox).Text.Trim());

                cmdUpdate.Parameters.AddWithValue("@cartid", cartID);
                cmdUpdate.Parameters.AddWithValue("@subtotal", subtotal);

                cmdUpdate.Parameters.AddWithValue("@detailid", Convert.ToInt32(gvPayment.DataKeys[i].Value.ToString()));
                cmdUpdate.ExecuteNonQuery();
            
            gvPayment.EditIndex = -1;
            }

            //insert data to payment table 
           
             string sqlPayment = "INSERT into Payment (CartId, datepaid, total) values('" + cartID + "','"+ DateTime.Now.ToString()+"','"+ totalPay + "')";

             SqlCommand cmdPayment = new SqlCommand();
                
              /*  cmdPayment.Parameters.AddWithValue("@artId", Convert.ToInt32(gvPayment.DataKeys[i].Value.ToString()));
                cmdPayment.Parameters.AddWithValue("@artprice", tempP);
                cmdPayment.Parameters.AddWithValue("@datePaid", DateTime.Now.ToString());
                cmdPayment.Parameters.AddWithValue("@qty", (gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim());
              */
                cmdPayment.Connection = con;
                cmdPayment.CommandType = CommandType.Text;
                cmdPayment.CommandText = sqlPayment;
                cmdPayment.ExecuteNonQuery();
               
            for (int i = 0; i < gvPayment.Rows.Count; i++)
            {
                String queryUpdateCart = "Update Cart SET status = 'ordered' WHERE CartId=@cartid";

                SqlCommand cmdUpdate = new SqlCommand(queryUpdateCart, con);

                cmdUpdate.Parameters.AddWithValue("@cartid", cartID);

                cmdUpdate.ExecuteNonQuery();

                gvPayment.EditIndex = -1;
            }

            //retrieve art qty left 
            int quantity = 0;
            for (int i = 0; i < gvPayment.Rows.Count; i++)
            {
                quantity = 0;
                string queryArtQty = "SELECT Quantity FROM Artist WHERE ArtId = (SELECT ArtId FROM OrderDetails WHERE OrderDetailId = @od_Id); ";


                using (SqlCommand cmdArtQty = new SqlCommand(queryArtQty, con))
                {

                    cmdArtQty.Parameters.AddWithValue("@od_Id", gvPayment.DataKeys[i].Value.ToString());
                    quantity = ((Int32?)cmdArtQty.ExecuteScalar()) ?? 0;
                    quantity -= int.Parse((gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim());

                }

                //update qty & availability of the art
                if (quantity <= 0)
                {
                    //update art qty & availability
                    String queryUpdateQty = "Update Artist SET Quantity = 0, Availability = '0' WHERE ArtId = (SELECT ArtId FROM OrderDetails WHERE OrderDetailId = @od_Id);";
                    SqlCommand cmdUpdateArtQty = new SqlCommand(queryUpdateQty, con);

                    cmdUpdateArtQty.Parameters.AddWithValue("@od_Id", gvPayment.DataKeys[i].Value.ToString());

                    cmdUpdateArtQty.ExecuteNonQuery();

                    gvPayment.EditIndex = -1;
                }
                else
                {
                    //update art qty left 
                    String queryUpdateQty = "Update Artist SET Quantity = @qty WHERE ArtId = (SELECT ArtId FROM OrderDetails WHERE OrderDetailId = @od_Id);";
                    SqlCommand cmdUpdateArtQty = new SqlCommand(queryUpdateQty, con);

                    cmdUpdateArtQty.Parameters.AddWithValue("@qty", quantity);
                    cmdUpdateArtQty.Parameters.AddWithValue("@od_Id", gvPayment.DataKeys[i].Value.ToString());

                    cmdUpdateArtQty.ExecuteNonQuery();

                    gvPayment.EditIndex = -1;
                }
                
            }

            con.Close();


            //send email then redirect to payment history
            sendEmail();

        }

        //send receipt to customer through email
        private void sendEmail()
        {
            
            if (Page.IsValid)
            {

                String emailOrderInfo = "";
                String artName, unitPrice, qty; 
                for (int i = 0; i < gvPayment.Rows.Count; i++)
                {
                    artName = (gvPayment.Rows[i].FindControl("artItem_Name") as TextBox).Text.Trim();
                    unitPrice = (gvPayment.Rows[i].FindControl("item_order_summary_price") as TextBox).Text.Trim();
                    qty = (gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim(); 
                    emailOrderInfo += "<br/><br/>" + artName + "<br/> RM "+ unitPrice+" x "+ qty ;

                } 

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("quadCoreTest@gmail.com");

                    mail.To.Add("quadCoreTest@gmail.com");
                    mail.Subject = "Quad-Core Art Gallery Receipt";

                    mail.Body = "<b><u>Purchase Information</u></b>" + emailOrderInfo+
                        "<br/><br/>Delevery Fees = RM "+ deliverly_fees.Text +
                        "<br/><br/>Total         = RM " + total_payment.Text +
                        "<br/><br/><br/>  Thank you!" +
                        "<br/><br/> Receipt Generated By: QUAD-CORE AUTO SYSTEM";


                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = Encoding.UTF8;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("quadCoreTest@gmail.com", "quad_core");
                        smtp.EnableSsl = true;

                        try
                        {
                            smtp.Send(mail);

                            //pop up massage then redirect to payment history
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "Email Status",
                                                "alert('Your receipt has been send to your email.');window.location ='PayHistory.aspx';",
                                                true);

                        }
                        catch (Exception)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Email Status", "alert('Sorry, Quad-Core ASG Email Account Down. Please Contact Quad-Core AWS!')", true);
                        }
                    }
                }
            }
        }

    }
}