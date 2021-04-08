﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WAD_Assignment.ArtWorks
{
    public partial class ArtWorks : System.Web.UI.Page
    {
        int CurrentPage;
        SqlConnection conn;
        string sqlconn;
        SqlDataAdapter dataAdapter;
        DataTable dt;
        SqlCommand command;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindList();
                ViewState["PageCount"] = 0; 
            }
            CurrentPage = (int)this.ViewState["PageCount"];
        }

        private void connection()
        {
            sqlconn = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            conn = new SqlConnection(sqlconn);

        }

        void bindList()
		{
            connection();

            //sorting feature
            dataAdapter = new SqlDataAdapter("Select * from Artist ORDER BY ArtId DESC", conn);
            if (rblCategory.SelectedIndex != -1)
            {
                switch (ddlArtSort.SelectedIndex)
                {
                    //Display all
                    case 0:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY ArtId DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex+1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name asscending
                    case 1:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY ArtName ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex+1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name descending
                    case 2:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY ArtName DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price asscending
                    case 3:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY Price ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price descending
                    case 4:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY Price DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;
                        
                }

                
            }
            else
            {
                switch (ddlArtSort.SelectedIndex)
                {
                    //Display all
                    case 0:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY ArtId DESC", conn);
                        break;

                    //Sort by name asscending
                    case 1:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY ArtName ASC", conn);
                        break;

                    //Sort by name descending
                    case 2:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY ArtName DESC", conn);
                        break;

                    //Sort by price asscending
                    case 3:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY Price ASC", conn);
                        break;

                    //Sort by price descending
                    case 4:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY Price DESC", conn);
                        break;
                }
            }

            dt = new DataTable();
            conn.Open();
            dataAdapter.Fill(dt);

            conn.Close();

            //paging feature
            DataListPaging(dt);
        }

        private void DataListPaging(DataTable dt)
        {
            //PagedDataSource setting
            PagedDataSource PD = new PagedDataSource();

            PD.DataSource = dt.DefaultView;
            PD.PageSize = 6;
            PD.AllowPaging = true;
            PD.CurrentPageIndex = CurrentPage;
            btnFirst.Enabled = !PD.IsFirstPage;
            btnPrevious.Enabled = !PD.IsFirstPage;
            btnNext.Enabled = !PD.IsLastPage;
            btnLast.Enabled = !PD.IsLastPage;
            ViewState["TotalCount"] = PD.PageCount;

            ArtWorkDataList.DataSource = PD;
            ArtWorkDataList.DataBind();
            ViewState["PagedDataSurce"] = dt;
        }

        protected void btnFirstClick_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            ViewState["PageCount"] = CurrentPage;

            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }

        protected void btnPreviousClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["PageCount"];

            if(CurrentPage != 0)
                CurrentPage -= 1;
            ViewState["PageCount"] = CurrentPage;

            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }

        protected void btnNextClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["PageCount"];
            CurrentPage += 1;
            ViewState["PageCount"] = CurrentPage;
            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }

      

        protected void btnLastClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["TotalCount"] - 1;
            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }

        protected void ddlArtSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindList();
        }

        protected void rblCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindList();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            bindList();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rblCategory.SelectedIndex = -1;
            bindList();
        }

        protected void loveBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = sender as ImageButton;
            

            Int32 userID = 0;
            Int32 artID = Convert.ToInt32(imgButton.CommandArgument.ToString());
            // try
            // {
            //Get current user id
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString))
            {
                con.Open();

                //get user id
                string query = "SELECT UserId FROM [dbo].[User] WHERE Role='Customer' AND LoginStatus='Active'";
                using (SqlCommand cmdUser = new SqlCommand(query, con))
                {
                    userID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                }

                con.Close();
                con.Open();
   
                string sql = "INSERT into Wishlist (ArtId, UserId, DateAdded) values('" + artID + "', '" + userID + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";

              
                SqlCommand cmd = new SqlCommand();
       
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;


                cmd.ExecuteNonQuery();
                con.Close();
                imgButton.ImageUrl = "/img/wishlist/heart-icon-active.png";
                Response.Write("<script>alert('Congratulation, Art Added into Wishlist Successfully')</script>");
            //    }
                // }
            }
            //  }
            //   catch
            //   {
            //       Response.Write("<script>alert('Sorry, Fail to Add the Art into Cart')</script>");
            //   }


        }

        protected void addToCartBtn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            Int32 userID = 0;
            Int32 artID = Convert.ToInt32(btn.CommandArgument.ToString());
            Int32 cartID = 0;
            Int32 orderDetailID = 0;
            int qty;
            decimal unitPrice = 0;
            int qtyOrderDetail = 0;
            decimal subtotalOrderDetail = 0;

            // try
            // {

            //Get current user id
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString))
            {

                con.Open();

                string query = "SELECT UserId FROM [dbo].[User] WHERE Role='Customer' AND LoginStatus='Active'";
                using (SqlCommand cmdUser = new SqlCommand(query, con))
                {
                    userID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                }

                if(userID == 0)
                {
                    Response.Write("<script>alert('Please Login first!')</script>");
                }
                else
                {

                SqlCommand cmd1 = new SqlCommand("SELECT Price, Quantity from [Artist] Where ArtId = @ArtId", con);
                cmd1.Parameters.AddWithValue("@ArtId", artID);

                SqlDataReader dtrArt = cmd1.ExecuteReader();
                if (dtrArt.HasRows)
                {
                    while (dtrArt.Read())
                    {
                        qty = (int)dtrArt["Quantity"];
                        unitPrice = (decimal)dtrArt["Price"];
                    }

                }
                con.Close();

                    //   if (qty != 0)
                    //   {

                    //check if cart exist
                    con.Open();
                    string queryCheckCart = "Select CartId FROM [dbo].[Cart] WHERE UserId = '"+ userID+ "'AND status = 'cart'";

                    using (SqlCommand cmdCheckCart = new SqlCommand(queryCheckCart, con))
                    {
                        cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
                    }
                    con.Close();

                    if(cartID == 0)
                    {
                        //insert to create a new cart
                        String status = "cart";
                        string sql = "INSERT into Cart (UserId, status) values('" + userID + "', '" + status+"')";

                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString);
                        SqlCommand cmd = new SqlCommand();
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        //search the new cartid
                        conn.Open();
                        string queryFindCartID = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + userID + "'AND status = 'cart'";

                        using (SqlCommand cmdCheckCart = new SqlCommand(queryFindCartID, conn))
                        {
                            cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
                        }
                        conn.Close();


                        
                    }

                    //get exist order detail

                    con.Open();

                    SqlCommand cmdOrderDetailID = new SqlCommand("SELECT OrderDetailId, qtySelected, Subtotal from [OrderDetails] Where CartId = @CartId AND ArtId = @ArtId", con);
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
                    con.Close();

                    //check whether exist same art (order detail)
                    if (orderDetailID != 0)
                    {
                        //update order details
                        qtyOrderDetail++;
                        subtotalOrderDetail += unitPrice;

                        string sqlUpdateOrder = "UPDATE OrderDetails SET qtySelected = '"+ qtyOrderDetail + "', Subtotal = '" + subtotalOrderDetail + "' WHERE orderDetailID = '" + orderDetailID + "'";

                        SqlCommand cmdUpdateOrder = new SqlCommand();
                        con.Open();
                        cmdUpdateOrder.Connection = con;
                        cmdUpdateOrder.CommandType = CommandType.Text;
                        cmdUpdateOrder.CommandText = sqlUpdateOrder;


                        cmdUpdateOrder.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        //insert order details based on cartid

                        string sqlInsertOrder = "INSERT into OrderDetails (CartId, ArtId, qtySelected, Subtotal) values('" + cartID + "', '" + artID + "', '" + 1 + "', '" + unitPrice + "')";

                        SqlCommand cmdInsertOrder = new SqlCommand();
                        con.Open();
                        cmdInsertOrder.Connection = con;
                        cmdInsertOrder.CommandType = CommandType.Text;
                        cmdInsertOrder.CommandText = sqlInsertOrder;


                        cmdInsertOrder.ExecuteNonQuery();
                        con.Close();
                    }

                    

                   Response.Write("<script>alert('Congratulation, Art Added into Cart Successfully')</script>");
                   }
                    

                    // }
                }
            //  }
            //   catch
            //   {
            //       Response.Write("<script>alert('Sorry, Fail to Add the Art into Cart')</script>");
            //   }



        }
    }
}