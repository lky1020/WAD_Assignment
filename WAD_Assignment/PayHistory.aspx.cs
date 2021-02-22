﻿using System;
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
    public partial class PayHistory : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ArtWorkDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                paymentHistory_refreshdata();

                for(int i = 0; i < gvPayHistory.Rows.Count; i++)
                {
                    int payID = 1000000 + int.Parse((gvPayHistory.Rows[i].FindControl("his_orderID1") as TextBox).Text.Trim());

                    gvPayHistory.Rows[i].Cells[1].Text = "P"+payID.ToString();
                    
                }
                

            }
            else
            {

            }
        }


        public void paymentHistory_refreshdata()
        {

            Int32 userID = 0;

            //detect current user id
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                string query1 = "Select UserId FROM [dbo].[User] WHERE Role = 'Customer' AND LoginStatus = 'Active' ";
                using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                {
                    userID = ((Int32?)cmd1.ExecuteScalar()) ?? 0;
                }
                conn.Close();

            }

            //pass data into grid
            SqlConnection con = new SqlConnection(cs);
            con.Open();


            String query = "Select p.paymentId, a.ArtName, a.Price, p.qty, p.total, p.datePaid from [Payment] p INNER JOIN [Artist] a on p.ArtId = a.artId Where p.userId = @id ORDER BY p.paymentId DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", userID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                gvPayHistory.DataSource = dt;
                gvPayHistory.DataBind();
                historyEmpty.Visible = false;
            }
            else
            {
                historyEmpty.Visible = true;
               }
            con.Close();


        }
    }
}