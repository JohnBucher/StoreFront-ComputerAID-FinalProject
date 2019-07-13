using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Store.Data;
//using StoreFront.InventoryService;

namespace Project2_v._2._0
{
    public partial class ServiceTest : System.Web.UI.Page
    {
        public MyDataEntities da = new MyDataEntities();

        protected void Button1_Click(object sender, EventArgs e)
        {
            //On button click
            //If the TextBox containing the search string is NOT empty
            if (TextBox1.Text != string.Empty)
            {
                //Create the WebService
                ServiceInventoryReference.Service1Client service = new ServiceInventoryReference.Service1Client();

                //Bind the DataSource to the result of the method
                GridView1.DataSource = service.SearchProducts(TextBox1.Text);
                //Rebind the GridView to reflect the changes
                GridView1.DataBind();
            }
            else
            {
                //If the search string is empty then set the GridView to appear empty
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //On button click
            //If the TextBox containing the search string is NOT empty
            if (TextBox2.Text != string.Empty)
            {
                //Create the WebService
                ServiceInventoryReference.Service1Client service = new ServiceInventoryReference.Service1Client();
                //Retrieve the item (Product) that the seach string was referring to
                var item = service.GetProductDetails(Convert.ToInt32(TextBox2.Text));

                //Check to make sure that the Product is not null
                if(item != null)
                {
                    //If the item exists then bind the DataSource to the item
                    GridView2.DataSource = item.ToList();
                    //Rebind the GridView to reflect the changes
                    GridView2.DataBind();
                }
                else
                {
                    //If the item doe snot exist then set the GridView to appear empty
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                }
            }
            else
            {
                //If the search string is empty then set the GridView to appear empty
                GridView2.DataSource = null;
                GridView2.DataBind();
            }

        }
    }
}