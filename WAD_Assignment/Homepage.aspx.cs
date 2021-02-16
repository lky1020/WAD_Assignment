using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class Homepage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack == true)
            {
                //Validate the contact form
                validateContact();
            }
        }

        protected void btnContactSubmit_Click(object sender, EventArgs e)
        {
            if(validateContact())
            {
                using (MailMessage mail = new MailMessage())
                {
                    //Hardcode first
                    //mail.From = new MailAddress(lblContactEmail.Text);
                    mail.From = new MailAddress("quadCoreTest@gmail.com");
                    mail.To.Add("limkahyee16@gmail.com");
                    mail.Subject = "Customer's Comment";
                    mail.Body = txtContactComment.Text;
                    mail.BodyEncoding = Encoding.UTF8;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("quadCoreTest@gmail.com", "quad_core");
                        smtp.EnableSsl = true;

                        try
                        {
                            smtp.Send(mail);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Email Status", "alert('Email Sent')", true);
                            txtContactName.Text = "";
                            txtContactEmail.Text = "";
                            txtContactComment.Text = "";
                            btnContactSubmit.Text = "Submit";
                            btnContactSubmit.Style.Add("cursor", "pointer");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            else
            {
                if (txtContactName.Text.Equals(""))
                {
                    lblContactName.Text = "Please Enter Your Name!";
                }

                if (txtContactEmail.Text.Equals(""))
                {
                    lblContactEmail.Text = "Please Enter Your Email!";
                }

                if (txtContactComment.Text.Equals(""))
                {
                    lblContactComment.Text = "Please Enter Your Message!";
                }
                    
            }
        }

        private Boolean validateContact()
        {
            Boolean contactValidation = false;

            if (!txtContactName.Text.Equals(""))
            {
                contactValidation = true;
            }
            else
            {
                contactValidation = false;
            }

            if (!txtContactEmail.Text.Equals(""))
            {
                if (!Regex.IsMatch(txtContactEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    lblContactEmail.Text = "Invalid Email Format!";
                    contactValidation = false;
                }
                else
                {
                    lblContactEmail.Text = "";
                    contactValidation = true;
                }
            }
           

            if (!txtContactComment.Text.Equals(""))
            {
                lblContactComment.Text = "";
                contactValidation = true;
            }
            else
            {
                contactValidation = false;
            }

            return contactValidation;
        }
    }
}