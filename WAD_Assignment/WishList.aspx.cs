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
    public partial class WishList : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnFirstWL.Enabled = false;
            btnPreviousWL.Enabled = false;

            if (!IsPostBack)
            {
                refreshdata();
            }
        }

        public void refreshdata()
        {
            Int32 userID = 0;

            //detect current user id
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                string query1 = "Select UserId FROM [dbo].[User] WHERE Role = 'Customer' AND LoginStatus = 'Active' ";
                using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                {
                    userID = ((Int32?)cmd1.ExecuteScalar()) ?? 0;
                }
                conn.Close();

            }

            //pass data into grid
            DataTable dt = new DataTable();
            
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            String query = "Select w.WishlistId, w.UserId, w.ArtId, w.DateAdded, a.ArtName, a.ArtImage, a.Price, a.ArtDescription from [WishList] w INNER JOIN [Artist] a on w.ArtId = a.ArtId Where w.UserId = @userid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userid", userID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                gvWishList.DataSource = dt;
                gvWishList.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                gvWishList.DataSource = dt;
                gvWishList.DataBind();
                gvWishList.Rows[0].Cells.Clear();
                gvWishList.Rows[0].Cells.Add(new TableCell());
                gvWishList.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                gvWishList.Rows[0].Cells[0].Text = "No Item inside Your Wishlist ...";
                gvWishList.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }

        protected void gvWishList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    String query = "DELETE FROM WishList WHERE WishlistId = @WishlistId";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@WishlistId", Convert.ToInt32(gvWishList.DataKeys[e.RowIndex].Value.ToString()));
                    cmd.ExecuteNonQuery();
                    refreshdata();

                    Response.Write("<script>alert('Congratulation, this art had remove from your wishlist successfully')</script>");

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, Fail to Delete the Art from your wishlist')</script>");
            }
        }

        protected void gvWishList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            this.refreshdata();
        }

        protected void btnFirstWL_Click(object sender, EventArgs e)
        {
            gvWishList.PageIndex = 0;
            btnFirstWL.Enabled = false;
            btnPreviousWL.Enabled = false;
            btnLastWL.Enabled = true;
            btnNextWL.Enabled = true;
            this.refreshdata();
        }

        protected void btnPreviousWL_Click(object sender, EventArgs e)
        {

            int i = gvWishList.PageCount;
            if (gvWishList.PageIndex > 0)
            {

                gvWishList.PageIndex = gvWishList.PageIndex - 1;
                btnLastWL.Enabled = true;
            }

            if (gvWishList.PageIndex == 0)
            {
                btnFirstWL.Enabled = false;
            }
            if (gvWishList.PageCount - 1 == gvWishList.PageIndex + 1)
            {
                btnNextWL.Enabled = true;
            }
            if (gvWishList.PageIndex == 0)
            {
                btnPreviousWL.Enabled = false;
            }
            this.refreshdata();
        }

        protected void btnNextWL_Click(object sender, EventArgs e)
        {
            int i = gvWishList.PageIndex + 1;
            if (i <= gvWishList.PageCount)
            {
                gvWishList.PageIndex = i;
                btnLastWL.Enabled = true;
                btnPreviousWL.Enabled = true;
                btnFirstWL.Enabled = true;
            }

            if (gvWishList.PageCount - 1 == gvWishList.PageIndex)
            {
                btnNextWL.Enabled = false;
                btnLastWL.Enabled = false;
            }
            this.refreshdata();
        }

        protected void btnLastWL_Click(object sender, EventArgs e)
        {
            gvWishList.PageIndex = gvWishList.PageCount;
            btnLastWL.Enabled = false;
            btnNextWL.Enabled = false;
            btnFirstWL.Enabled = true;
            this.refreshdata();
        }

        protected void btnContinueWL_Click(object sender, EventArgs e)
        {
            Response.Redirect("ArtWorks.aspx");
        }
    }
}
