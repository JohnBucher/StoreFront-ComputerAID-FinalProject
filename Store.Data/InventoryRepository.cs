using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data
{
    public class InventoryRepository
    {
        public MyDataEntities dc = new MyDataEntities();

        //Returns a list of class “Product”, that have name or description containing the attached search text
        public List<Product> SearchProducts(string search)
        {
            var productList = dc.Products.Where(x => x.ProductName.Contains(search) || x.Description.Contains(search)).ToList();

            return productList;
        }

        //Returns an instance of the custom class “Product”, which will have all the fields in the “Product” class in entity framework
        public Product GetProduct(int id)
        {
            Product p = (dc.Products.Where(x => x.ProductID == id)).FirstOrDefault();

            return p;
        }

    }
}
