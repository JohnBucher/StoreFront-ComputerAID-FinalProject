using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data
{
    public class OrderRepository
    {
        private MyDataEntities db = new MyDataEntities();

        //Returns an instance of the “Order” object, which is the row in the table with the corresponding id
        public Order GetOrderMethod(int id)
        {
            Order o = new Order();
            o = db.Orders.Where(x => x.OrderID == id).FirstOrDefault();

            return o;
        }

    }
}
