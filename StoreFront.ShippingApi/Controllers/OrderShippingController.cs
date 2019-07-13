using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Data;

namespace StoreFront.ShippingApi.Controllers
{
    public class OrderShippingController : Controller
    {
        private MyDataEntities db = new MyDataEntities();
        OrderRepository or = new OrderRepository();

        // GET api/Order/
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //GetOrders:            /OrderShipping/GetOrders/(startDate, endDate)
        public List<Order> GetOrders(DateTime startDate, DateTime endDate)
        {
            List<Order> orders = db.Orders.Where(x => x.OrderDate > startDate && x.OrderDate < endDate).ToList();
            return orders;
        }


        //MarkOrderShipped:     /OrderShipping/MarkOrderShipped/(id)
        public string MarkOrderShipped(int id)
        {
            //Create an order object to hold our Order based on its ID
            Order initialOrder = new Order();
            //Used to grab the order again after saving in order to make sure that the changes have gone through
            List<Order> testOrder = new List<Order>();
            //o = or.GetOrderMethod(id);

            //Using the ID, put the specific order into our Order object
            initialOrder = db.Orders.Where(x => x.OrderID == id).FirstOrDefault();

            //If our initial order isn't empty (it would be empty if no Order matching the ID was found) then set the Status to 3.
            if(initialOrder != null)
            {
                initialOrder.StatusID = 3;
            }

            //Try saving
            try
            {
                db.SaveChanges();
            }
            //If saving does not work
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            //PassThis will be returned so that we can view our success/failure message
            string passThis = "";
            //If our initialOrder is not null (Meaning we did find an order matching the OrderID) then go about checking the StatusID
            if (initialOrder != null)
            {
                //Grab the order from the DB, we don't want to use our old object because we want to make sure that the change has gone through
                testOrder = db.Orders.Where(x => x.OrderID == id).ToList();
                foreach (var item in testOrder)
                {
                    //Return the status ID of the order so that we can observe its value
                    passThis = "Order Status set to: " + item.Status.StatusDescription;
                }
            }
            //If our initialOrder was null then it means that no Order matched the OrderID and we need to report that error to the user
            else
            {
                passThis = "Failure: No Order Found";
            }

            //Return the success/failure message
            return passThis;
        }
    }
}