using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Store.Data;

namespace Store.InventoryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        // Return a list of products whose names or desriptions contain the search text.
        [OperationContract]
        List<ProductItem> SearchProducts(string text);
        // Return a product whose ProductID corresponds to the input id.
        [OperationContract]
        List<ProductItem> GetProductDetails(int id);

    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "StoreFront.InventoryService.ContractType".
    [DataContract]
    public class ProductItem
    {
        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string ImageFile { get; set; }
    }
}
