using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WAD_Assignment.ArtWorks
{
    public partial class ArtWorkDetails : System.Web.UI.Page
    {

        string constr = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.GetArtDetails();
            }
        }

        private void GetArtDetails()
        {
            // create connection
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            // retrieve data
            SqlCommand cmd = new SqlCommand("SELECT a.ArtName, a.ArtImage, a.Price, a.ArtDescription, a.Quantity, u.Name, u.Bio, u.ProfileImg from[Artist] a INNER JOIN[User] u on a.UserId = u.UserId Where a.ArtId = @ArtId", con);
            cmd.Parameters.AddWithValue("@ArtId", Request.QueryString["ArtId"]);

            SqlDataReader dtrArt = cmd.ExecuteReader();

            if (dtrArt.HasRows)
            {
                while (dtrArt.Read())
                {
                    //msg = "Product Name = " + dtrProd["name"].ToString() + "<br/> Price" + dtrProd["price"].ToString();
                    dArtDetailsImage.ImageUrl = dtrArt["ArtImage"].ToString();
                    dArtName.Text = dtrArt["ArtName"].ToString().ToUpper();
                    dArtPrice.Text = "RM " + String.Format("{0:0.00}", dtrArt["Price"]);
                    dAboutArt.Text = dtrArt["ArtDescription"].ToString();
                    dArtStock.Text = dtrArt["Quantity"].ToString();

                    string profileImage = dtrArt["ProfileImg"].ToString();
                    if (!String.IsNullOrEmpty(profileImage))
                        dArtistImage.ImageUrl = profileImage;
                    else
                        dArtistImage.ImageUrl = "/img/userPic/user_default.png";

                    dArtistName.Text = dtrArt["Name"].ToString();

                    string bio = dtrArt["Bio"].ToString();
                    if (!String.IsNullOrEmpty(bio))
                        dBio.Text = bio;
                    else
                        dBio.Text = "The artist didn't leave any bio...";


                }
            }
            else
            {
                //msg = "No Record(s) Found!";
            }

            con.Close();
        }

        protected void loveBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = sender as ImageButton;


            //Response.Redirect("/WishList.aspx");

            Int32 userID = 0;

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

                string sql = "INSERT into Wishlist (ArtId, UserId, DateAdded) values('" + Request.QueryString["ArtId"] + "', '" + userID + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";


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
            Int32 userID = 0;
            Int32 cartID = 0;
            int qty = 0;
            int qtySelected = Convert.ToInt32(detailsQtyControl.Text);
            decimal unitPrice = 0;
            decimal subtotal = qtySelected * unitPrice;

            // try
            // {
            //Get current user id
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string query = "SELECT UserId FROM [dbo].[User] WHERE Role='Customer' AND LoginStatus='Active'";
                using (SqlCommand cmdUser = new SqlCommand(query, con))
                {
                    userID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                }

                SqlCommand cmd1 = new SqlCommand("SELECT Price, Quantity from [Artist] Where ArtId = @ArtId", con);
                cmd1.Parameters.AddWithValue("@ArtId", Request.QueryString["ArtId"]);

                SqlDataReader dtrArt = cmd1.ExecuteReader();
                if (dtrArt.HasRows)
                {
                    while (dtrArt.Read())
                    {
                        qty = (int)dtrArt["Quantity"];
                        unitPrice = ((decimal)dtrArt["Price"]);
                    }

                }
                con.Close();

                //Check whether valid input and enough quantity
                if (qtySelected == 0)
                {
                    Response.Write("<script>alert('The quantity cannot be 0, please enter your quantity.')</script>");
                }
                else if (qtySelected > qty)
                {
                    Response.Write("<script>alert('Sorry, not enough stock, please enter your quantity.')</script>");
                }
                else
                {
                    con.Open();
                    string queryCheckCart = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + userID + "'AND status = 'cart'";

                    using (SqlCommand cmdCheckCart = new SqlCommand(queryCheckCart, con))
                    {
                        cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
                    }
                    con.Close();

                    if (cartID == 0)
                    {
                        //insert to create a new cart
                        String status = "cart";
                        string sql = "INSERT into Cart (UserId, status) values('" + userID + "', '" + status + "')";

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

                    //insert order details based on cartid

                    string sqlInsertOrder = "INSERT into OrderDetails (CartId, ArtId, qtySelected, Subtotal) values('" + cartID + "', '" + Request.QueryString["ArtId"] + "', '" + 1 + "', '" + unitPrice + "')";

                    SqlCommand cmdInsertOrder = new SqlCommand();
                    con.Open();
                    cmdInsertOrder.Connection = con;
                    cmdInsertOrder.CommandType = CommandType.Text;
                    cmdInsertOrder.CommandText = sqlInsertOrder;


                    cmdInsertOrder.ExecuteNonQuery();
                    con.Close();

                    Response.Write("<script>alert('Congratulation, Art Added into Cart Successfully')</script>");
                }
                /*
                subtotal = qtySelected * unitPrice;

                string sql = "INSERT into Cart (UserId, ArtId, qtySelected, Subtotal) values('" + userID + "', '" + Request.QueryString["ArtId"] + "', '" + qtySelected + "', '" + subtotal + "')";

                SqlConnection conn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;


                cmd.ExecuteNonQuery();
                conn.Close();

                Response.Write("<script>alert('Congratulation, Art Added into Cart Successfully')</script>");
            }*/
            }
            //  }
            //   catch
            //   {
            //       Response.Write("<script>alert('Sorry, Fail to Add the Art into Cart')</script>");
            //   }


        }

        protected void detailsCancelBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/ArtWorks.aspx");
        }

        protected void dPlusControl_Click(object sender, ImageClickEventArgs e)
        {
            int qty = Convert.ToInt32(detailsQtyControl.Text);
            qty += 1;
            detailsQtyControl.Text = qty.ToString();

        }

        protected void dMinusControl_Click(object sender, ImageClickEventArgs e)
        {
            int qty = Convert.ToInt32(detailsQtyControl.Text);
            if (qty != 0)
                qty -= 1;
            detailsQtyControl.Text = qty.ToString();
        }
    }
}