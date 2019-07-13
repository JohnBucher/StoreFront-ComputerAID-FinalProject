using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Store.Data;
using System.IO;

namespace Project2_v._2._0
{
    public partial class ProductAdminDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void PADDetails_ItemUpdated(object sender, DetailsViewUpdatedEventArgs u)
        {
            //Rebind the GridView and DataView to reflect changes to items
            PADGrid.DataBind();
            PADDetails.DataBind();
        }
        protected void PADGrid_ItemDeleting(object sender, GridViewDeleteEventArgs u)
        {
            //Rebind the GridView and DataView to reflect changes to items
            PADGrid.DataBind();
            PADDetails.DataBind();
            //When an item has been deleted set the DetailsView to invisible
            PADDetails.Visible = false;
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            Store.Data.MyDataEntities dc = new Store.Data.MyDataEntities();
            if (ProductImageUpload.HasFile)
            {
                try
                {
                    //Creation of the address string that is stored and is used to access the image's location
                    string q = Request.QueryString["ProductID"];
                    string imgName = Path.GetFileName(ProductImageUpload.FileName);
                    ProductImageUpload.SaveAs(Server.MapPath("~/ProductImages/") + imgName);

                    imgName = "~/ProductImages/" + imgName;

                    //Set the Product's ImageFile to the image address string
                    dc.Database.ExecuteSqlCommand("UPDATE Product SET ImageFile = {0} WHERE(ProductID = {1})", imgName, q);

                    //Rebind the GridView and DataView to reflect changes to items
                    PADGrid.DataBind();
                    PADDetails.DataBind();

                }
                catch (Exception ex)
                {
                    //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

    }
}