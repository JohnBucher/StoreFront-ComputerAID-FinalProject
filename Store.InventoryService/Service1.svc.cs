using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Store.Data;

namespace Store.InventoryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private MyDataEntities db = new MyDataEntities();
        InventoryRepository ir = new InventoryRepository();

        //SearchProducts
        //This method will return a list of items who's ProductName and/or Description contain the search string text.
        public List<ProductItem> SearchProducts(string value)
        {
            List<ProductItem> listProductItems = new List<ProductItem>();
            //Call the SearchProducts method and return the result into a list of Products
            List<Product> list = ir.SearchProducts(value);

            //Transfer all Product objects into ProductItem objects
            foreach (var item in list)
            {
                ProductItem input = new ProductItem();
                    input.ProductID = item.ProductID;
                    input.ProductName = item.ProductName;
                    input.Description = item.Description;
                    input.Quantity = (int)item.Quantity;
                    input.Price = (decimal)item.Price;
                    input.ImageFile = item.ImageFile;

                //Add the current ProductItem to the ProductItem list
                listProductItems.Add(input);
            }
            //Return the list of ProductItems
            return listProductItems;
        }

        //GetProductDetails
        //This method will return a single Product based on ProductID that is taken in by the method
        //Despite the method returning a LIST of ProductItems, the list will ALWAYS only contain one ProductItem
        public List<ProductItem> GetProductDetails(int id)
        {
            List<ProductItem> listProductItems = new List<ProductItem>();
            //Call the SearchProducts method and return the result into a Product object
            Product item = ir.GetProduct(id);

            //Transfer the Product object into a ProductItem object
            ProductItem input = new ProductItem();
                input.ProductID = item.ProductID;
                input.ProductName = item.ProductName;
                input.Description = item.Description;
                input.Quantity = (int)item.Quantity;
                input.Price = (decimal)item.Price;
                input.ImageFile = item.ImageFile;

            //Add the current ProductItem to the ProductItem list
            listProductItems.Add(input);

            //Return the list of ProductItems
            return listProductItems;
        }

    }
}
