using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            // string strSelect = "Select * from Artist";
            // SqlCommand cmdSelect = new SqlCommand(strSelect, conn);
            // cmdSelect.CommandType = CommandType.StoredProcedure;
            //SELECT * FROM Artist INNER JOIN Category ON Category.CategoryID = 1 AND Artist.Category = 1;
            //"SELECT * FROM Artist ORDER BY Price ASC"

            //sorting feature
            dataAdapter = new SqlDataAdapter("Select * from Artist", conn);
            if (rblCategory.SelectedIndex != -1)
            {
                switch (ddlArtSort.SelectedIndex)
                {
                    //Display all
                    case 0:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex+1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name asscending
                    case 1:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category ORDER BY ArtName ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex+1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name descending
                    case 2:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category ORDER BY ArtName DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price asscending
                    case 3:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category ORDER BY Price ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price descending
                    case 4:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category ORDER BY Price DESC", conn);
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
                        dataAdapter = new SqlDataAdapter("Select * from Artist", conn);
                        break;

                    //Sort by name asscending
                    case 1:
                        dataAdapter = new SqlDataAdapter("Select * from Artist ORDER BY ArtName ASC", conn);
                        break;

                    //Sort by name descending
                    case 2:
                        dataAdapter = new SqlDataAdapter("Select * from Artist ORDER BY ArtName DESC", conn);
                        break;

                    //Sort by price asscending
                    case 3:
                        dataAdapter = new SqlDataAdapter("Select * from Artist ORDER BY Price ASC", conn);
                        break;

                    //Sort by price descending
                    case 4:
                        dataAdapter = new SqlDataAdapter("Select * from Artist ORDER BY Price DESC", conn);
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
            
            
            //Response.Redirect("/WishList.aspx");

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
            int qty;
            decimal unitPrice = 0;

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
                string sql = "INSERT into Cart (UserId, ArtId, qtySelected, Subtotal) values('" + userID + "', '" + artID + "', '" + 1 + "', '" + unitPrice + "')";

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;


                cmd.ExecuteNonQuery();
                conn.Close();

                Response.Write("<script>alert('Congratulation, Art Added into Cart Successfully')</script>");
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