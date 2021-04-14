﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
         
            //pass data into grid
            DataTable dt = new DataTable();
            
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            String query = "Select w.WishlistId, w.UserId, w.ArtId, w.DateAdded, a.ArtName, a.ArtImage, a.Price, a.Quantity, a.ArtDescription, a.Availability from [WishList] w INNER JOIN [Artist] a on w.ArtId = a.ArtId Where w.UserId = @userid ORDER BY w.WishlistId DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userid", Session["userId"]);

            con.Close();

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                gvWishList.DataSource = dt;
                gvWishList.DataBind();
                checkAvailability();

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

        protected void checkAvailability()
        {
            int stock;
            Boolean available = true;

            SqlConnection con = new SqlConnection(cs);
            

            for (int i = 0; i < gvWishList.Rows.Count; i++)
            {
                string queryArtAvailable = "SELECT Availability, Quantity FROM Artist WHERE ArtId = (SELECT ArtId FROM WishList WHERE WishlistId = @WishlistId)";

                using (SqlCommand cmdArtAvailable = new SqlCommand(queryArtAvailable, con))
                {
                    cmdArtAvailable.Parameters.AddWithValue("@WishlistId", gvWishList.DataKeys[i].Value.ToString());
                    con.Open();

                    SqlDataReader dtrArt = cmdArtAvailable.ExecuteReader();

                    if (dtrArt.HasRows)
                    {
                        while (dtrArt.Read())
                        {
                            available = (Boolean)dtrArt["Availability"];
                            stock = (int)dtrArt["Quantity"];

                            if (stock == 0 || !available)
                            {
                                CheckBox chkbox = gvWishList.Rows[i].FindControl("chkItems") as CheckBox;
                                chkbox.Enabled = false;

                                Label lblDescription = gvWishList.Rows[i].FindControl("wl_artDes") as Label;
                                lblDescription.Text = "Item is not available";

                            }
                        }
                    }
                    con.Close();

                   
                }

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

        protected void wl_artImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = sender as ImageButton;
            Int32 artID = Convert.ToInt32(imgButton.CommandArgument.ToString());

            Response.Redirect("ArtWorkDetails.aspx?ArtId="+ artID);
 
        }

        //Select all
        protected void CheckBoxHead_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chckheader = (CheckBox)gvWishList.HeaderRow.FindControl("checkBoxHead");

            foreach (GridViewRow row in gvWishList.Rows)
            {
                CheckBox chckrw = (CheckBox)row.FindControl("chkItems");

                if (chckheader.Checked == true && row.Cells[0].Enabled == true && chckrw.Enabled == true)
                    chckrw.Checked = true;
                else
                    chckrw.Checked = false;

            }
        }

        protected void insertCart(Int32 artID, decimal unitPrice)
        {
            Int32 cartID = 0;
            Int32 orderDetailID = 0;

            int qtyOrderDetail = 0;
            decimal subtotalOrderDetail = 0;

            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            string queryCheckCart = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + Session["userId"] + "'AND status = 'cart'";

            using (SqlCommand cmdCheckCart = new SqlCommand(queryCheckCart, conn))
            {
                cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
            }
            conn.Close();

            if (cartID == 0)
            {
                //insert to create a new cart
                String status = "cart";
                string sql = "INSERT into Cart (UserId, status) values('" + Session["username"] + "', '" + status + "')";

                
                SqlCommand cmd = new SqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                conn.Close();

                //search the new cartid
                conn.Open();
                string queryFindCartID = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + Session["username"] + "'AND status = 'cart'";

                using (SqlCommand cmdCheckCart = new SqlCommand(queryFindCartID, conn))
                {
                    cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
                }
                conn.Close();



            }

            //get exist order detail

            conn.Open();

            SqlCommand cmdOrderDetailID = new SqlCommand("SELECT OrderDetailId, qtySelected, Subtotal from [OrderDetails] Where CartId = @CartId AND ArtId = @ArtId", conn);
            cmdOrderDetailID.Parameters.AddWithValue("@CartId", cartID);
            cmdOrderDetailID.Parameters.AddWithValue("@ArtId", artID);

            SqlDataReader dtrOrderDetail = cmdOrderDetailID.ExecuteReader();
            if (dtrOrderDetail.HasRows)
            {
                while (dtrOrderDetail.Read())
                {
                    orderDetailID = (Int32)dtrOrderDetail["OrderDetailId"];
                    qtyOrderDetail = (int)dtrOrderDetail["qtySelected"];
                    subtotalOrderDetail = (decimal)dtrOrderDetail["Subtotal"];
                }

            }
            conn.Close();

            conn.Open();

            //check whether exist same art (order detail)
            if (orderDetailID != 0)
            {
                //update order details
                qtyOrderDetail++;
                subtotalOrderDetail += unitPrice;

                string sqlUpdatetOrder = "UPDATE  OrderDetails SET qtySelected = " + qtyOrderDetail + ", Subtotal = " + subtotalOrderDetail + " WHERE OrderDetailId = " + orderDetailID;

                SqlCommand cmdInsertOrder = new SqlCommand();

                cmdInsertOrder.Connection = conn;
                cmdInsertOrder.CommandType = CommandType.Text;
                cmdInsertOrder.CommandText = sqlUpdatetOrder;


                cmdInsertOrder.ExecuteNonQuery();
            }
            else
            {
                //insert order details based on cartid

                string sqlInsertOrder = "INSERT into OrderDetails (CartId, ArtId, qtySelected, Subtotal) values('" + cartID + "', '" + artID + "', '" + 1 + "', '" + unitPrice + "')";

                SqlCommand cmdInsertOrder = new SqlCommand();

                cmdInsertOrder.Connection = conn;
                cmdInsertOrder.CommandType = CommandType.Text;
                cmdInsertOrder.CommandText = sqlInsertOrder;


                cmdInsertOrder.ExecuteNonQuery();

            }

            conn.Close();

        }

        protected void removeItem(Int32 wishlistID)
        {
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            String query = "DELETE FROM WishList WHERE WishlistId = @WishlistId";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@WishlistId", wishlistID);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        //Add to Cart button
        protected void addToCartBtn_Click(object sender, EventArgs e)
        {
            bool haveItemChk = false;

            for (int i = 0; i < gvWishList.Rows.Count; i++)
            {

                CheckBox chkb = (CheckBox)gvWishList.Rows[i].Cells[0].FindControl("chkItems");

                try
                {
                    if (chkb.Checked)
                    {
                        //get wishlist id
                        Label lblWishlist = (Label)gvWishList.Rows[i].Cells[0].FindControl("lblWishlistID");
                        Int32 wishlistID = Convert.ToInt32(lblWishlist.Text);

                        //get artID
                        ImageButton artImg = (ImageButton)gvWishList.Rows[i].Cells[0].FindControl("wl_artImg");
                        Int32 artID = Convert.ToInt32(artImg.CommandArgument.ToString());

                        //get unit price
                        Label lblPrice = (Label)gvWishList.Rows[i].Cells[0].FindControl("wl_price");
                        decimal unitPrice = Convert.ToDecimal(lblPrice.Text);
                        try
                        {
                            insertCart(artID, unitPrice);
                            removeItem(wishlistID);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
                        }
                        
                        haveItemChk = true;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Server down, please contact QUAD-CORE ASG. Customer Services.')</script>");
                    System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
                }
            }

            if (haveItemChk)
            {
                //print successfully message
                Response.Write("<script>alert('Congratulation, Art in Wishlist Deleted Successfully')</script>");
                refreshdata();
            }
            else
            {
                //print error message
                Response.Write("<script>alert('No item selected')</script>");
            }
        }
    }
}
