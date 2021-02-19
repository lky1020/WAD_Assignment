using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class Art : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnViewArt_Click(object sender, EventArgs e)
        {
            Response.Redirect("PostArts.aspx");
        }

        protected void btnViewArtSubmit_Click1(object sender, EventArgs e)
        {
            Int32 userID = 0;


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString))
            {

                con.Open();

                string query = "SELECT UserId FROM [dbo].[User] WHERE Role='Artist' AND LoginStatus='Active'";
                using (SqlCommand cmdUser = new SqlCommand(query, con))
                {
                    userID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                }



            }

            string path;

            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(Server.MapPath("~/img/Artist/") + FileUpload1.FileName);

            }

            path = "img/Artist/" + FileUpload1.FileName;

            string sql = "insert into Artist (ArtName, ArtDescription, ArtImage, UserId, Category, Price, Quantity) values('" + txtBoxArtName.Text + "', '"
                + txtBoxArtDesc.Text + "', '" + path + "', '" + userID + "', '" + ddlCatArt.SelectedValue + "', '"
                + txtBoxArtPrice.Text + "', '" + txtBoxArtQuantity.Text + "')";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            int a = 0;

            con.Open();
            a = cmd.ExecuteNonQuery();
            con.Close();

            if (a == 0)
            {
                Response.Write("<script>alert('Sorry, Fail to Update the Art')</script>");
            }
            else
            {
                Response.Write("<script>alert('Congratulation, Art Added Successfully')</script>");
                txtBoxArtDesc.Text = "";
                txtBoxArtName.Text = "";
                txtBoxArtPrice.Text = "";
                txtBoxArtQuantity.Text = "";
            }
        }

        protected void txtBoxArtName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCatArt_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtBoxArtDesc_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtBoxArtPrice_TextChanged(object sender, EventArgs e)
        {

        }
    }
}