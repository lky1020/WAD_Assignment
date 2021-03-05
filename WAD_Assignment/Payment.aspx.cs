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
                String query = "Select a.ArtId, a.ArtName, a.Price, o.OrderDetailId, o.qtySelected, o.Subtotal from [OrderDetails] o " +
                    "INNER JOIN [Artist] a on o.ArtId = a.ArtId Where o.OrderDetailId = @id";
                //String query = "Select c.CartId, c.ArtId, a.ArtName, a.Price, c.qtySelected, c.Subtotal from [Cart] c INNER JOIN [Artist] a on c.ArtId = a.ArtId Where c.CartId = @id";
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
           // con.Close();


            //retrieve 'pending' cartid
           // con.Open();
            string queryFindCartID = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + currentUser + "'AND status = 'pending'";

            using (SqlCommand cmdCheckCart = new SqlCommand(queryFindCartID, con))
            {
                cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
            }
           // con.Close();


            //update selected item with the cartid
            //con.Open();
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
            //con.Close();

            //insert data to payment table 
            //con.Open();

            //double artPrice = double.Parse((gvPayment.Rows[i].FindControl("item_order_summary_price") as TextBox).Text.Trim());
            // double eachTotalPrice = double.Parse((gvPayment.FindControl("total_payment") as TextBox).Text.Trim())+ 3.0;
           
                string sqlPayment = "INSERT into Payment (CartId, datepaid, total) values('" + cartID + "','"+ DateTime.Now.ToString()+"','"+ totalPay + "')";

                SqlCommand cmdPayment = new SqlCommand();
                
              /*  cmdPayment.Parameters.AddWithValue("@artId", Convert.ToInt32(gvPayment.DataKeys[i].Value.ToString()));
                cmdPayment.Parameters.AddWithValue("@artprice", tempP);
                cmdPayment.Parameters.AddWithValue("@datePaid", DateTime.Now.ToString());
                cmdPayment.Parameters.AddWithValue("@qty", (gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim());
                cmdPayment.Parameters.AddWithValue("@total", tempTP);
              */
                cmdPayment.Connection = con;
                cmdPayment.CommandType = CommandType.Text;
                cmdPayment.CommandText = sqlPayment;
                //con.Open();
                cmdPayment.ExecuteNonQuery();
               



            //update the pending cartid status = 'ordered'
            //con.Open();
            for (int i = 0; i < gvPayment.Rows.Count; i++)
            {
                String queryUpdateCart = "Update Cart SET status = 'ordered' WHERE CartId=@cartid";

                SqlCommand cmdUpdate = new SqlCommand(queryUpdateCart, con);

                cmdUpdate.Parameters.AddWithValue("@cartid", cartID);

                cmdUpdate.ExecuteNonQuery();

                gvPayment.EditIndex = -1;
            }
            con.Close();


            Response.Redirect("PayHistory.aspx");
        }
    }
}