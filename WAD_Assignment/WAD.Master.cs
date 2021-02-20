﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WAD_Assignment
{
    public partial class WAD : System.Web.UI.MasterPage
    {

        //DB
        private string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;

        //Share the user data to profile.aspx.cs
        public static string username;
        public static string userPicPath;
        public static string userRole;
        public static string userEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Always check active user account
            RetrieveActiveUserAccount();

            //Initialize webpage
            if (IsPostBack == false)
            {
                BindMenu();
            }

        }

        private void BindMenu()
        {
            DataSet menuItem = GetMenuItems();

            //Bind Menu for Homepage
            bindMenuItem(menuItem, siteMenu);

            //Bind Menu for Not Homepage
            bindMenuItem(menuItem, headerSiteMenu);
        }

        private DataSet GetMenuItems()
        {
            //Retrieve the menu data for menu control
            string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da = new SqlDataAdapter("spGetMenuData", con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            // Establish relationship (foreign keys)
            ds.Relations.Add("ChildRows", ds.Tables[0].Columns["Id"], ds.Tables[1].Columns["ParentId"]);

            return ds;
        }

        private void bindMenuItem(DataSet ds, Menu menuID)
        {
            //Bind the menu item to menu control
            foreach (DataRow level1DataRow in ds.Tables[0].Rows)
            {
                MenuItem item = new MenuItem();
                item.Text = level1DataRow["MenuText"].ToString();
                item.NavigateUrl = level1DataRow["NavigateUrl"].ToString();

                DataRow[] level2DataRows = level1DataRow.GetChildRows("ChildRows");
                foreach (DataRow level2DataRow in level2DataRows)
                {
                    MenuItem childItem = new MenuItem();
                    childItem.Text = level2DataRow["MenuText"].ToString();
                    childItem.NavigateUrl = level2DataRow["NavigateUrl"].ToString();
                    item.ChildItems.Add(childItem);
                }

                menuID.Items.Add(item);

            }
        }

        private Boolean RetrieveActiveUserAccount()
        {
            //Retrieve the account that is active
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da = new SqlDataAdapter("SELECT Name, Email, ProfileImg, Role FROM [dbo].[User] WHERE LoginStatus = 'Active'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //Update the sidebar with user account
            if (dt.Rows.Count >= 1)
            {
                username = dt.Rows[0]["Name"].ToString();
                lblLoginName.Text = username;

                userPicPath = dt.Rows[0]["ProfileImg"].ToString();

                //Update Profile Pic
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateProfilePicPath", "document.getElementById('profileImg').src ='" + userPicPath + "';", true);

                //User Role
                userRole = dt.Rows[0]["Role"].ToString();

                //Email - for contact form
                userEmail = dt.Rows[0]["Email"].ToString();

                return true;
            }

            return false;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //Deactive the account
            DeactivateProfileNavigation();

            //Deactive the manage art navigation
            DeactiveManageArtworkNavigation();

            //Reset the lblLoginName
            lblLoginName.Text = "";

            //Update menu (Need to reset siteMenu && headerSiteMenu first)
            siteMenu.Items.Clear();
            headerSiteMenu.Items.Clear();
            BindMenu();

            //Return to homepage
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Homepage",
            "window.location ='Homepage.aspx';alert('Logout Success');", true);

        }

        private void DeactivateProfileNavigation()
        {
            //Deactive the user account once the user logout
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("sp_ProfileDeactive", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("name", lblLoginName.Text);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void DeactiveManageArtworkNavigation()
        {
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("sp_ManageArtDeactive", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}