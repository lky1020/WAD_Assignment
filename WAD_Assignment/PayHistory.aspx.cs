using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class PayHistory : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                paymentHistory_refreshdata();

                for(int i = 0; i < gvPayHistory.Rows.Count; i++)
                {
                    int payID = 1000000 + int.Parse((gvPayHistory.Rows[i].FindControl("his_orderID1") as TextBox).Text.Trim());

                    gvPayHistory.Rows[i].Cells[1].Text = "P"+payID.ToString();
                    
                }
            }
        }


        public void paymentHistory_refreshdata()
        {

            Int32 userID = 0;

            if (Session["username"] != null)
            {
                //detect current user id
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    string query1 = "Select UserId FROM [dbo].[User] WHERE Name = '" + Session["username"].ToString() + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                    {
                        userID = ((Int32?)cmd1.ExecuteScalar()) ?? 0;
                    }
                    conn.Close();

                }


                //pass data into grid
                SqlConnection con = new SqlConnection(cs);
                con.Open();


                String query = "Select p.paymentId, o.OrderDetailId, a.ArtName, a.Price, o.qtySelected, o.Subtotal, p.datePaid from [Payment] p " +
                    "INNER JOIN [Cart] c on p.CartId = c.CartId INNER JOIN [OrderDetails] o on o.CartId = c.CartId " +
                    "INNER JOIN [Artist] a on o.ArtId = a.ArtId " +
                    "Where c.UserId = @id " +
                    "ORDER BY p.paymentId DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", userID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvPayHistory.DataSource = dt;
                    gvPayHistory.DataBind();
                    historyEmpty.Visible = false;

                }
                else
                {
                    historyEmpty.Visible = true;
                }
                con.Close();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "HistoryDenied",
                        "alert('Access Denied. Please Login!'); window.location = 'Login.aspx';", true);

            }
        }
    }
}