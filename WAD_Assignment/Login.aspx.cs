using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        //DB
        string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;

        private string loginName;
        private string profilePicPath;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack == false)
            {
                //Auto insert username and password that get from param
                string username = Request.QueryString["username"];
                string password = Request.QueryString["password"];

                txtEmail_Username.Text = username;

                txtPassword.TextMode = TextBoxMode.SingleLine;
                txtPassword.Text = password;
                txtPassword.Attributes["type"] = "password";

            }else
            {
                //Validate the login form
                validateLogin();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string loginMethod;

            if (validateLogin())
            {
                //Check db
                if(checkExistingUser() == true || checkExistingEmail() == true)
                {
                    if(checkExistingUser() == true)
                    {
                        //Username
                        loginMethod = "Name";
                    }
                    else
                    {
                        //Email
                        loginMethod = "Email";
                    }

                    //Check password
                    if(checkPassword(loginMethod) == true)
                    {
                        //Get profile pic path
                        if(getProfilePicPath(loginMethod) == true)
                        {
                            //Activate the profile navigation + user account
                            ActivateProfileNavigation();

                            //Return to homepage
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                            "alert('Login Success');window.location ='Homepage.aspx';", true);
                        }
                    }
                    else
                    {
                        lblPassword.Text = "Invalid Password";
                    }
                }
                else
                {
                    //Invalid Username
                    if (checkExistingUser() == false)
                    {
                        lblEmail_Username.Text = "Invalid Username";
                    }
                    else
                    {
                        lblEmail_Username.Text = "Invalid Email";
                    }
                }
            }
            else
            {
                if (txtEmail_Username.Text.Equals(""))
                {
                    lblEmail_Username.Text = "Please Enter Your Username or Email!";
                }

                if (txtPassword.Text.Equals(""))
                {
                    lblPassword.Text = "Please Enter Your Password!";
                }

            }
        }

        private Boolean checkExistingUser()
        {
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da = new SqlDataAdapter("SELECT Name FROM [dbo].[User] WHERE Name = '" + txtEmail_Username.Text + "' ", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count >= 1)
            {
                return true;
            }

            return false;

        }

        private Boolean checkExistingEmail()
        {
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da = new SqlDataAdapter("SELECT Email FROM [dbo].[User] WHERE Email = '" + txtEmail_Username.Text + "' ", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count >= 1)
            {
                return true;
            }

            return false;
        }

        private Boolean checkPassword(string loginMethod)
        {
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da;
            if (loginMethod.Equals("Name")){
                da = new SqlDataAdapter("SELECT Name, Email, Password FROM [dbo].[User] WHERE " + "Name = '" + txtEmail_Username.Text +"' " + "AND Password = '" + txtPassword.Text + "' ", con);
            }
            else
            {
                da = new SqlDataAdapter("SELECT Name, Email, Password FROM [dbo].[User] WHERE " + "Email = '" + txtEmail_Username.Text + "' " + "AND Password = '" + txtPassword.Text + "' ", con);
            }
            
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count >= 1)
            {
                loginName = dt.Rows[0]["Name"].ToString();
                return true;
            }

            return false;
        }

        private Boolean getProfilePicPath(string loginMethod)
        {
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da;
            if (loginMethod.Equals("Name"))
            {
                da = new SqlDataAdapter("SELECT ProfileImg FROM [dbo].[User] WHERE " + "Name = '" + txtEmail_Username.Text + "' " + "AND Password = '" + txtPassword.Text + "' ", con);
            }
            else
            {
                da = new SqlDataAdapter("SELECT ProfileImg FROM [dbo].[User] WHERE " + "Email = '" + txtEmail_Username.Text + "' " + "AND Password = '" + txtPassword.Text + "' ", con);
            }

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count >= 1)
            {
                profilePicPath = dt.Rows[0]["ProfileImg"].ToString();
                return true;
            }

            return false;
        }

        private Boolean validateLogin()
        {
            Boolean loginValidation = false;

            //Username
            if (!txtEmail_Username.Text.Equals(""))
            {
                loginValidation = true;
            }
            else
            {
                loginValidation = false;
            }

            //Password
            if (!txtPassword.Text.Equals(""))
            {
                loginValidation = true;
            }
            else
            {
                loginValidation = false;
            }

            return loginValidation;
        }

        private void ActivateProfileNavigation()
        {
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("sp_ProfileActive", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("name", loginName);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void DeactivateProfileNavigation()
        {
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("sp_ProfileDeactive", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("name", loginName);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}