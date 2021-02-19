using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class ForgotPassword : System.Web.UI.Page
    {

        private string resetPin;
        private string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack == true)
            {
                //Execute after get the user new password
                if (!hiddenResetPasswordValue.Value.Equals(""))
                {
                    //Get user's name from db
                    if(GetUsername() == true)
                    {
                        if (UpdateUserPasswordServer() == true)
                        {
                            //Reset the resetPin
                            UpdateResetPin();

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Update Password", "alert('Password Update Success, Proceed to Login Page');", true);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Direct to Login.aspx", "window.location.replace('https://localhost:44336/Login.aspx');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Update Password", "alert('Password Update Unsuccess, Proceed to Homepage');", true);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Direct to Login.aspx", "window.location.replace('https://localhost:44336/Homepage.aspx');", true);
                        }
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if(validateForgotPasswordEmail() == true)
            {
                //Request PIN First
                if (txtResetPin.Text.Equals(""))
                {
                    //Check is exist email or not
                    if(checkExistingEmail() == true)
                    {
                        //Get the user reset pin
                        GetUserResetPin();

                        //Use team email to send reset pin to user
                        using (MailMessage mail = new MailMessage())
                        {

                            mail.From = new MailAddress("quadCoreTest@gmail.com");
                            mail.To.Add("limkahyee16@gmail.com");
                            mail.Subject = "Your Reset Pin";
                            mail.Body = "Hi " + username + ", this is your reset pin for your password: <b>" + resetPin + "</b><br/> Enter it to Reset Your Password.";
                            mail.IsBodyHtml = true;
                            mail.BodyEncoding = Encoding.UTF8;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new System.Net.NetworkCredential("quadCoreTest@gmail.com", "quad_core");
                                smtp.EnableSsl = true;

                                try
                                {
                                    smtp.Send(mail);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Reset Pin", "alert('Reset PIN Sent')", true);
                                    btnReset.Text = "Enter New Password";
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
                //PIN Entered
                else
                {
                    //Call the get PIN function first, to prevent error when user did not click request pin btn
                    GetUserResetPin();

                    //Validate the PIN
                    if (txtResetPin.Text.Equals(resetPin))
                    {
                        
                        if(btnReset.Text.Equals("Enter New Password"))
                        {
                            //Will execute when the the user want to enter new password
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Reset Password", "var newPassword = window.prompt('Please Enter Your New Password: '); storeResetPassword(newPassword);", true);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Confirm Reset Password", "alert('Click Below Reset Password Button to Confirm');", true);
                            btnReset.Text = "Reset Password";
                        }
                        
                    }
                    else
                    {
                        //To prevent alert after user enter new password
                        if (hiddenResetPasswordValue.Value.Equals(""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Reset Pin", "alert('PIN Incorrect')", true);
                        }
                    }
                }
            }
            else
            {
                if (txtEmail.Text.Equals(""))
                {
                    lblEmail.Text = "Please Enter Your Email!";
                }
            }

        }

        private Boolean validateForgotPasswordEmail()
        {
            //Email
            if (!txtEmail.Text.Equals(""))
            {
                if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    lblEmail.Text = "Invalid Email Format! <br/>";
                    return false;
                }
                else
                {
                    lblEmail.Text = "";
                    return true;
                }
            }

            return false;
        }

        private Boolean GetUserResetPin()
        {
            string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da;
            
            da = new SqlDataAdapter("SELECT Name, ResetPin FROM [dbo].[User] WHERE " + "Email = '" + txtEmail.Text + "' ", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count >= 1)
            {
                username = dt.Rows[0]["Name"].ToString();
                resetPin = dt.Rows[0]["ResetPin"].ToString();
                return true;
            }

            return false;
        }

        private Boolean GetUsername()
        {
            string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da;

            da = new SqlDataAdapter("SELECT Name FROM [dbo].[User] WHERE " + "Email = '" + txtEmail.Text + "' ", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count >= 1)
            {
                username = dt.Rows[0]["Name"].ToString();
                return true;
            }

            return false;
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
                    lblEmail.Text = "";
                    return true;
                }
                else
                {
                    lblEmail.Text = "Email Not Exist!";
                }

                return false;
            }

            //Email invalid
            return false;
        }

        private Boolean UpdateUserPasswordServer()
        {
            string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("UPDATE[dbo].[User] " +
                                            "SET[Password] = '" + hiddenResetPasswordValue.Value + "' " +
                                            "WHERE [Name] = '" + username + "' ", con);
            cmd.CommandType = CommandType.Text;

            con.Open();
            int k = cmd.ExecuteNonQuery();
            con.Close();

            if (k != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean UpdateResetPin()
        {
            //Generate 6 digit random number for the new resetPin
            Random generator = new Random();
            string resetPin = generator.Next(0, 1000000).ToString("D6");

            string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("UPDATE[dbo].[User] " +
                                            "SET[ResetPin] = '" + resetPin + "' " +
                                            "WHERE [Name] = '" + username + "' ", con);
            cmd.CommandType = CommandType.Text;

            con.Open();
            int k = cmd.ExecuteNonQuery();
            con.Close();

            if (k != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}