using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

            bindList();
        }

        protected void btnContactSubmit_Click(object sender, EventArgs e)
        {
            if (validateContact())
            {
                using (MailMessage mail = new MailMessage())
                {
                    //Hardcode first
                    //mail.From = new MailAddress(lblContactEmail.Text);
                    mail.From = new MailAddress("quadCoreTest@gmail.com");
                    mail.To.Add(WAD.userEmail);
                    mail.To.Add("quadCoreTest@gmail.com");
                    mail.Subject = "Customer's Comment";
                    mail.Body = txtContactComment.Text + "<br/> From: " + WAD.username + " (" + WAD.userEmail + ")";
                    mail.IsBodyHtml = true;
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

        private void bindList()
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            //sorting feature
            SqlDataAdapter da = new SqlDataAdapter("Select TOP 6 * from Artist", con);

            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);

            con.Close();

            //paging feature
            DataListPaging(dt);
        }

        private void DataListPaging(DataTable dt)
        {
            //PagedDataSource setting
            PagedDataSource PD = new PagedDataSource();

            PD.DataSource = dt.DefaultView;
            ArtWorkDataList.DataSource = PD;
            ArtWorkDataList.DataBind();

        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            if(WAD.userRole != null)
            {
                //Direct to gallery
                if (WAD.userRole.Equals("Artist"))
                {
                    Response.Write("<script>window.location = 'ArtistGallery.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>window.location = 'ArtWorks.aspx';</script>");
                }
            }
            else
            {
                Response.Write("<script>window.location = 'ArtWorks.aspx';</script>");
            }

        }
    }
}