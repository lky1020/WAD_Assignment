using System;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //Initialize webpage
            if(IsPostBack == false)
            {
                DataSet menuItem = GetMenuItems();

                //Homepage
                bindMenuItem(menuItem, siteMenu);

                //Not Homepage
                bindMenuItem(menuItem, headerSiteMenu);
            }
            
        }

        private DataSet GetMenuItems()
        {
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
    }
}