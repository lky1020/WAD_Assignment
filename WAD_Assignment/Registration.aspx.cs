using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            string password = Request.QueryString["password"];
            string successRegister = Request.QueryString["successRegister"];
            string postBackUrl = Request.QueryString["postBackUrl"];

            if(IsPostBack == true)
            {
                //Validate the register form
                validateRegister();
                checkExistingUser();
                checkExistingEmail();

                //keep password when still not register
                if (validateRegister() == false)
                {
                    txtUsername.Attributes["value"] = txtUsername.Text;
                    txtEmail.Attributes["value"] = txtEmail.Text;
                    txtPassword.Attributes["value"] = txtPassword.Text;

                    //Gender
                    if (rdMale.Checked)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeGender", "selectMale();", true);
                    }else if (rdFemale.Checked)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeGender", "selectFemale();", true);
                    }
                    
                    //Role
                    if (rdArtist.Checked)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeRole", "selectArtist();", true);
                    }
                    else if (rdCustomer.Checked)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeRole", "selectCustomer();", true);
                    }
                }
            }

            if (successRegister != null)
            {
                if (successRegister.Equals("true"))
                {
                    btnLogin.PostBackUrl = postBackUrl + "?username=" + username + "&password=" + password;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Register Status", "alert('Successfully Registered')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Register Status", "alert('UnSuccessfully Registered')", true);
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            if (validateRegister() && checkExistingUser() == false && checkExistingEmail() == false)
            {
                insertRegisterData();

                //Reset to default
                txtUsername.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";

                //Go back to login.aspx
                //Response.Redirect("~/login.aspx?username=" + txtUsername.Text + "&password=" + txtPassword.Text);

            }
            else
            {
                if (txtUsername.Text.Equals(""))
                {
                    lblUsername_Validation.Text = "Please Enter Your Name!";
                }

                if (txtEmail.Text.Equals(""))
                {
                    lblEmail_Validation.Text = "Please Enter Your Email!";
                }

                if (txtPassword.Text.Equals(""))
                {
                    lblPassword_Validation.Text = "Please Enter Your Password!";
                }

                if (!rdMale.Checked && !rdFemale.Checked)
                {
                    lblGender_Validation.Text = "Please Select Your Gender <br/>";
                }

                if (!rdArtist.Checked && !rdCustomer.Checked)
                {
                    lblRole_Validation.Text = "Please Select Your Role  <br/>";
                }
            }
        }

        private void insertRegisterData()
        {
            string gender;

            if (rdMale.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            string role;

            if (rdArtist.Checked)
            {
                role = "Artist";
            }
            else
            {
                role = "Customer";
            }

            Random generator = new Random();
            string resetPin = generator.Next(0, 1000000).ToString("D6");

            string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("sp_registerUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("name", txtUsername.Text);
            cmd.Parameters.AddWithValue("password", txtPassword.Text);
            cmd.Parameters.AddWithValue("email", txtEmail.Text);
            cmd.Parameters.AddWithValue("gender", gender);
            cmd.Parameters.AddWithValue("profilePic", "../img/userPic/user_default.png");   //initialize the profile pic
            cmd.Parameters.AddWithValue("resetPin", resetPin);
            cmd.Parameters.AddWithValue("role", role);

            con.Open();
            int k = cmd.ExecuteNonQuery();
            con.Close();
            if (k != 0)
            {
                string str = "~/login.aspx";
                Response.Redirect("~/registration.aspx?username=" + txtUsername.Text + "&password=" + txtPassword.Text + "&successRegister=true" + "&postBackUrl=" + str);
            }
            else
            {
                Response.Redirect("~/registration.aspx?username=" + txtUsername.Text + "&password=" + txtPassword.Text + "&successRegister=false");
            }

        }

        private Boolean checkExistingUser()
        {
            if(!txtUsername.Text.Equals(""))
            {
                string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                SqlDataAdapter da = new SqlDataAdapter("SELECT Name FROM [dbo].[User] WHERE Name = '" + txtUsername.Text + "' ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    lblUsername_Validation.Text = "Username already exist!";
                    return true;
                }
                else
                {
                    lblUsername_Validation.Text = "";
                }

                return false;
            }

            //return true means it is invalid
            return true;
        }

        private Boolean checkExistingEmail()
        {
            if (!txtEmail.Text.Equals(""))
            {
                string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                SqlDataAdapter da = new SqlDataAdapter("SELECT Email FROM [dbo].[User] WHERE Email = '" + txtEmail.Text + "' ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    lblEmail_Validation.Text = "Email already exist!";
                    return true;
                }
                else
                {
                    lblEmail_Validation.Text = "";
                }

                return false;
            }

            //return true means it is invalid
            return true;
        }

        private Boolean validateRegister()
        {
            Boolean registerValidation = false;

            //Username
            if (!txtUsername.Text.Equals(""))
            {
                registerValidation = true;
            }
            else
            {
                registerValidation = false;
            }

            //Email
            if (!txtEmail.Text.Equals(""))
            {
                if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    lblEmail_Validation.Text = "Invalid Email Format! <br/>";
                    registerValidation = false;
                }
                else
                {
                    lblEmail_Validation.Text = "";
                    registerValidation = true;
                }
            }

            //Password
            if (!txtPassword.Text.Equals(""))
            {
                registerValidation = true;
            }
            else
            {
                registerValidation = false;
            }

            return registerValidation;
        }

    }
}