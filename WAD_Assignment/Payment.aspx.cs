using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                String query = "Select c.CartId, c.ArtId, a.ArtName, a.Price, c.qtySelected, c.Subtotal from [Cart] c INNER JOIN [Artist] a on c.ArtId = a.ArtId Where c.CartId = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", orderItem[i]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
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

            SqlConnection con = new SqlConnection(cs);
            con.Open();

            for (int i = 0; i < gvPayment.Rows.Count; i++)
            {
                int tempP = int.Parse((gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim());
                double tempTP = double.Parse((gvPayment.Rows[i].FindControl("order_subtotal") as TextBox).Text.Trim());


                SqlCommand cmd = new SqlCommand("sp_createPayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", currentUser);
                cmd.Parameters.AddWithValue("@artId", Convert.ToInt32(gvPayment.DataKeys[i].Value.ToString()));
                cmd.Parameters.AddWithValue("@artprice", tempP);
                cmd.Parameters.AddWithValue("@datePaid", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@qty", (gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim());
                cmd.Parameters.AddWithValue("@total", tempTP);
                //con.Open();
                cmd.ExecuteNonQuery();
                //(gvCart.Rows[e.RowIndex].FindControl("cart_qtySelect") as TextBox).Text.Trim())
            }
            con.Close();

            //delete the data from cart db

            con.Open();

            for (int i = 0; i < (orderItem.Length - 1); i++)
            {
                String query = "DELETE FROM Cart WHERE CartId = @cartid";

                SqlCommand cmdDel = new SqlCommand(query, con);
                cmdDel.Parameters.AddWithValue("@cartid", orderItem[i]);
                cmdDel.ExecuteNonQuery();


            }
            con.Close();

            Response.Redirect("PayHistory.aspx");
        }
    }
}