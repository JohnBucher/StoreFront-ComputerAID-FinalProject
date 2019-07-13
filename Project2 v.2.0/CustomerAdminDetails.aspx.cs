using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Store.Data;

namespace Project2_v._2._0
{
    public partial class CustomerAdminDetails : System.Web.UI.Page
    {
        SqlSecurityManager manager = new SqlSecurityManager();
        MyDataEntities db = new MyDataEntities();

        protected void Page_Load(object sender, EventArgs a)
        {

        }
        protected void CADDetails_ItemUpdated(object sender, DetailsViewUpdatedEventArgs u)
        {
            //Update the DB to preserve similarity
            manager.SaveUser();

            //Rebind the GridView and DataView to reflect changes to items
            CADGrid.DataBind();
            CADDetails.DataBind();
        }
        protected void CADGrid_ItemDeleting(object sender, GridViewDeleteEventArgs u)
        {
            //Rebind the GridView and DataView to reflect changes to items
            CADGrid.DataBind();
            CADDetails.DataBind();
            //When item has been deleted, make the details and address grid invisible
            CADDetails.Visible = false;
            AddressGrid.Visible = false;
        }
    }
}