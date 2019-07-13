using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project2_v._2._0
{
    public partial class CustomersAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void CADetails_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {

        }

        protected void CADetails_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //when an item is inserted, rebind the GridView to reflect the changes
            CAGrid.DataBind();
        }

        protected void CAGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}