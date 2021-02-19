using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAD_Assignment
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Set up the user detail (Pic + Name)
            InitializeProfile();

        }

        private void InitializeProfile()
        {
            lblProfileName.Text = WAD.username;

            //Update Profile Pic
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateUserPic", "document.getElementById('userPic').src ='" + WAD.userPicPath + "';", true);

            //Update the Profile Pic for change pic
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBrowsePic", "document.getElementById('previewPic').src ='" + WAD.userPicPath + "';", true);

            //Set user bio
            if (RetrieveUserBio() == false)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Bio Error", "alert('Fail to Retrieve Your Bio');", true);
            }
        }

        protected void btnEditBio_Click(object sender, EventArgs e)
        {
            //Change the text of the btnBio
            if (btnEditBio.Text.Equals("Edit Bio"))
            {
                btnEditBio.Text = "Confirm Bio";
                /*ScriptManager.RegisterStartupScript(Page, this.GetType(), "Display Cancel Btn", 
                    "document.getElementById('<%=btnCancelEditBio.ClientID %>').style.display = 'block';", true);*/
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Cancel Btn Background",
                    "changeColorForCancelBtn();", true);
            }
            else
            {
                string bio = txtAreaEditBio.Value.ToString();

                //Update the Bio in db
                UpdateUserBio(bio);

                btnEditBio.Text = "Edit Bio";
                txtAreaUserBio.Value = bio;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBioUI", 
                    "document.getElementById('editBio').style.display = 'none';", true);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "UnDisplay Cancel Btn",
                   "undisplayCancelEditButton();", true);
            }
        }

        protected void btnCancelEditBio_Click(object sender, EventArgs e)
        {
            //Undisplay the txtAreaEditBio
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBioUI",
                    "document.getElementById('editBio').style.display = 'none';", true);
            btnEditBio.Text = "Edit Bio";

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UnDisplay Cancel Btn",
                    "undisplayCancelEditButton();", true);
        }

        protected void btnUploadPic_Click(object sender, EventArgs e)
        {
            if(fileUpProfilePic.PostedFile != null)
            {
                string imgFile = Path.GetFileName(fileUpProfilePic.PostedFile.FileName);
                string imgFileExtension = Path.GetExtension(imgFile);

                if (imgFileExtension.Equals(".jpg"))
                {
                    fileUpProfilePic.SaveAs(Server.MapPath("~") + "/img/userPic/" + lblProfileName.Text + imgFileExtension);

                    string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
                    SqlConnection con = new SqlConnection(cs);
                    SqlCommand cmd = new SqlCommand("sp_UpdateProfilePic", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", lblProfileName.Text);
                    cmd.Parameters.AddWithValue("profileImg", "../img/userPic/" + lblProfileName.Text + imgFileExtension);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Upload Success", "alert('Profile Pic Upload Success');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Upload Fail", "alert('Please Upload .jpg Profile Image Only!');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Upload Fail", "alert('Profile Pic Fail to Upload');", true);
            }

            //Refresh page
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "window.location ='Profile.aspx';", true);
        }

        private void UpdateUserBio(string bio)
        {

            string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("sp_UpdateBio", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("name", lblProfileName.Text);
            cmd.Parameters.AddWithValue("bio", bio);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private Boolean RetrieveUserBio()
        {
            if (txtAreaEditBio.Value.Equals(""))
            {
                string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                SqlDataAdapter da;

                da = new SqlDataAdapter("SELECT Bio FROM [dbo].[User] WHERE " + "Name = '" + lblProfileName.Text + "' ", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    txtAreaUserBio.Value = dt.Rows[0]["Bio"].ToString();
                    txtAreaEditBio.Value = dt.Rows[0]["Bio"].ToString();
                    return true;
                }

                return false;
            }

            //No bio retrieve error
            return true;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Undisplay the txtAreaEditBio
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBioUI",
                    "document.getElementById('editBio').style.display = 'none';", true);
            btnEditBio.Text = "Edit Bio";

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UnDisplay Cancel Btn",
                    "undisplayCancelEditButton();", true);
        }

    }
}