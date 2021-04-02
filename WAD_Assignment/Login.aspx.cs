using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace WAD_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        //DB
        private string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // If already got account exist, deactive it && delete the form authentication cookie
            if (Session["username"] != null)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlCommand cmd = new SqlCommand("sp_ProfileDeactive", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("name", Session["username"].ToString());

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    //Reset Authentication (Cookie)
                    FormsAuthentication.SignOut();

                    Response.Write("<script>alert('You been Direct to Login Page!');</script>");
                }
                catch (Exception)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
                }
            }

        }
    }
}