using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Data;
using PagedList;
using System.Net;

namespace Project2_v._2._0.Controllers
{
    public class OrderController : Controller
    {
        MyDataEntities db = new MyDataEntities();

        //Index
        //Precursor to ShowIndex, this method will simply record the current Session's UserID, and then pass the page number and UserID
        // to ShowIndex
        public ActionResult Index(int? page)
        {
            int temp = Convert.ToInt32(Session["UserID"].ToString());
            return RedirectToAction("ShowIndex", "Order", new { page = page, userID = temp });
        }

        //ShowIndex
        //ShowIndex will use the UserID pull all of that User's address options to be turned into a DropDown list later in the program. 
        //ShowIndex will also use the page # given to it to display the list of Orders.
        public ActionResult ShowIndex(int? page, int userID)
        {
            ViewBag.DropDownValues = new SelectList(new[] { db.Orders.Where(x => x.UserID == userID).ToList() });
            return View(db.Orders.ToList().ToPagedList( page ?? 1, 50));
        }

        //enterAddress
        //This method will return a partial view that allows th euser to enter a new address into their options
        public ActionResult enterAddress()
        {
            var a = db.States.ToList();
            //Hold the state abbrev list to used and turned into a DropDown List
            ViewData["States"] = new SelectList(a, "StateID", "StateAbbrev");

            return PartialView();
        }

        //CreateAddress
        //Precursor to CreateAddressWithUser. This method will grab the current UserID and will pass the address object and UserID to the next step.
        public ActionResult CreateAddress(Store.Data.Address newAddress)
        {
            int temp = Convert.ToInt32(Session["UserID"].ToString());
            return RedirectToAction("CreateAddressWithUser", "Order", new { newAdress = newAddress, userID = temp });
        }

        // CreateAddressWithUser
        //This method will do all of the work in creating a new address option for the current user.
        [HttpPost]
        [Authorize]
        public ActionResult CreateAddressWithUser(Store.Data.Address newAddress, int userID)
        {
            if (ModelState.IsValid)
            {
                //Populate address object with necessary values
                newAddress.UserID = userID;
                newAddress.IsBilling = true;
                newAddress.IsShipping = true;
                newAddress.DateCreated = DateTime.Now;
                newAddress.CreatedBy = "dbo";
                newAddress.DateModified = DateTime.Now;
                newAddress.ModifiedBy = "dbo";

                //Add the new address object to the DB and then save the DB
                db.Addresses.Add(newAddress);
                db.SaveChanges();
            }
            //Redirect to the method: Address within this controller
            return RedirectToAction("Address", "Order");
        }

        //Address
        //This method provides the Address choice options when a user places their order.
        public ActionResult Address()
        {
            //Grab the curent UserID
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //use the UserID to grab all of their available address options
            var addressList = db.Addresses.Where(u => u.UserID == temporary).ToList();
            //Put this list into a ViewBag to be used in DropDown list in the View
            ViewBag.AddressList = new SelectList(addressList, "AddressID", "Address1");
            return View();
        }

        //ConfirmOrder
        //This method displays the address and shopping cart information to the user, with a button at the button to confirm their order.
        public ActionResult ConfirmOrder(Address A)
        {
            //Take the address chosen in the Address method and grab the ID
            int addressListID = A.AddressID;
            //Use the ID to get the one address object in the DB
            var addressPass = db.Addresses.Where(u => u.AddressID == addressListID).ToList();
            //Mark which address is used for later use
            Session["UsedAddress"] = addressListID;

            return View(addressPass);
        }

        //PartialShopCart
        //A partial view designed to display the current state of the Shopping Cart to the user so that they can view their items before confirming the
        // order
        public ActionResult PartialShopCart()
        {
            //Grab the current USer's ID
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //Create a list of ShoppingCartProducts based on the User's cart
            var shop = db.ShoppingCartProducts.Where(b => b.ShoppingCartID == temporary).ToList();

            return PartialView(shop);
        }

        //ConfirmationMessage
        //Precursor to ShowConfirmationMessage. This method will grab the Session variables: UserID, UsedAddress, and Total and then redirect to
        // ShowConfirmationMessage passing the three new variables into the redirect.
        public ActionResult ConfirmationMessage()
        {
            UpdateSession();

            //Temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            int tempAddress = Convert.ToInt32(Session["UsedAddress"].ToString());
            decimal tempTotal = Convert.ToDecimal(Session["Total"].ToString());

            //Redirect
            return RedirectToAction("ShowConfirmationMessage", "Order", new {
                userID = temporary, usedAddress = tempAddress, orderTotal = tempTotal });
        }

        //ShowConfirmationMessage
        //This method does the work to convert the current user's shopping cart and address into an Order object with OrderProducts
        public ActionResult ShowConfirmationMessage(int userID, int usedAddress, decimal orderTotal)
        {
            //returns the row with the to-be-deleted ShoppingCartID
            var check = db.ShoppingCartProducts.Where(x => x.ShoppingCartID == userID).ToList();

            //Creates a new Order ojbect and populates it with information
            Order order = new Order();
                order.UserID = userID;
                order.AddressID = usedAddress;
                order.OrderDate = DateTime.Now;
                order.Total = orderTotal;
                order.StatusID = 1;
                order.DateCreated = DateTime.Now;
                order.CreatedBy = "dbo";
                order.DateModified = DateTime.Now;
                order.ModifiedBy = "dbo";

            //Add the order to the DB and try saving
            db.Orders.Add(order);
            db.SaveChanges();

            //Now that we have an Order object, we can convert each of our ShoppingCartProducts into OrderProducts
            foreach (var item in check)
            {
                //Create an OrderProduct object and populate it with information gotten from a ShoppingCartProduct
                OrderProduct op = new OrderProduct();
                op.OrderID = order.OrderID;
                op.ProductID = item.ProductID;
                op.Quantity = item.Quantity;
                op.Price = item.Product.Price;
                op.DateCreated = DateTime.Now;
                op.CreatedBy = "dbo";
                op.DateModified = DateTime.Now;
                op.ModifiedBy = "dbo";

                //Add the new products to the DB and try saving
                db.OrderProducts.Add(op);
                db.SaveChanges();

                //Then remove the ShoppingCart product from the DB because it has been order, it is no longer needed in the ShoppingCart. Save changes.
                db.ShoppingCartProducts.Remove(item);
                db.SaveChanges();
            }
            //Return a view that tells the user that their order has been successfully placed.
            return View();
        }

        //Edit
        //This method edits an order based on the OrderID taken in by the method.
        public ActionResult Edit(int id)
        {
            var order = db.Orders.Where(b => b.OrderID == id).FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            Session["CurrentOrder"] = order.OrderID;
            return View(order);
        }

        //plusStatus
        //This method will increment the current status by 1 value
        public ActionResult plusStatus(int id, int orderNum)
        {
            var check = db.Orders.Where(x => x.OrderID == orderNum);

            foreach(var item in check)
            {
                item.StatusID++;
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

            //create a string with the new Status Description to pass back to the View and update the old Status Description
            check = db.Orders.Where(x => x.OrderID == orderNum).Where(x => x.StatusID == (id+1));
            string passThis = "";
            foreach(var item in check)
            {
                passThis = item.Status.StatusDescription;
            }

            return Json(new { newStatus = passThis, StateID = (id+1) });
        }

        //Delete
        //This method is in charge of deleting Order objects
        public ActionResult Delete(int id)
        {
            //Delete all instances of the OrderID
            Store.Data.Order O = new Store.Data.Order { OrderID = id };
            db.Orders.Attach(O);
            db.Orders.Remove(O);

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
            //Pass the ShoppingCartID back to the Ajax function
            return Json(new { newID = id });
        }

        //DeleteAddress
        //This method is in charge of deleting addresses based on the AddressId that the method takes in.
        public ActionResult DeleteAddress(int id)
        {
            //Delete all instances of the AddressID
            Store.Data.Address A = new Store.Data.Address { AddressID = id };
            db.Addresses.Attach(A);
            db.Addresses.Remove(A);

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
            //Pass the ShoppingCartID back to the Ajax function
            return Json(new { newID = id });
        }


        //METHODS FOR AJAX, TAKEN FROM SHOPPING CART CONTROLLER
        //------------------------------------------------------------------------------------------------------------------------

        //Updates the Total and Quantity Session variables
        public void UpdateSession()
        {
            int temp = Convert.ToInt32(Session["CurrentOrder"]);
            var order = db.OrderProducts.Where(a => a.OrderID == temp).ToList();

            //UPDATING THE QUANTITY AND TOTAL COUNTS BASED ON THE CURRENT USER'S SHOPPING CART
            int quan = 0; decimal total = 0;
            foreach (var item in order)
            {
                quan += (int)item.Quantity;
                total += (decimal)(item.Product.Price * item.Quantity);
            }
            System.Diagnostics.Debug.WriteLine(quan);
            System.Diagnostics.Debug.WriteLine(total);

            Session["OrderQuantity"] = quan;
            Session["OrderTotal"] = total.ToString("0.00");
        }


        [HttpPost]
        //Update the total after an item deletion
        public ActionResult UpdateTotal(int id)
        {
            //Makes sure everything is up-to-date
            UpdateSession();

            //Now that Total is up-to-date, pass it to the Ajax function
            string newTotal = Session["OrderTotal"].ToString();
            System.Diagnostics.Debug.WriteLine(newTotal);
            //Pass this value for total
            int temp = Convert.ToInt32(Session["CurrentOrder"]);

            System.Diagnostics.Debug.WriteLine(temp);

            var order = db.Orders.Where(b => b.OrderID == temp).FirstOrDefault();
            order.Total = Convert.ToDecimal(newTotal);

            db.SaveChanges();

            return Json(new { UpTotal = newTotal });
        }

        [HttpPost]
        //Update the quantity after an item deletion
        public ActionResult UpdateQuantity(int id)
        {
            //Makes sure everything is up-to-date
            UpdateSession();

            //temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //returns the row with the to-be-deleted ShoppingCartID
            var check = db.OrderProducts.Where(x => x.Order.UserID == temporary).Where(x => x.OrderProductID == id);

            //Grab the quantity associated with the ShoppingCartProductID
            int ProductQuantity = 0;
            foreach (var item in check)
            {
                ProductQuantity = (int)item.Quantity;
            }
            //Take the overall quantity minus the quantity that is to be deleted
            ProductQuantity = Convert.ToInt32(Session["OrderQuantity"].ToString()) - ProductQuantity;
            string passThis = "" + ProductQuantity;

            //Pass this new value for quantity
            return Json(new { UpQuantity = passThis });
        }

        [HttpPost]
        //Update the quantity after an item deletion
        public ActionResult UpdateItemQuantity(int newQuantity, int itemID)
        {
            //Makes sure everything is up-to-date
            UpdateSession();

            //temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["CurrentOrder"].ToString());
            //returns the row with the to-be-deleted ShoppingCartID
            var check = db.OrderProducts.Where(x => x.OrderID == temporary).Where(x => x.OrderProductID == itemID);

            //Grab the quantity associated with the ShoppingCartProductID
            decimal newSub = 0;
            int oldQuantity = 0;
            foreach (var item in check)
            {
                if (newQuantity > 0)
                {
                    item.Quantity = newQuantity;
                    newSub = Convert.ToDecimal(newQuantity * item.Product.Price);
                    //item.Price = newSub;
                }
                else { oldQuantity = (int)item.Quantity; newSub = Convert.ToDecimal(oldQuantity * item.Product.Price); }
            }

            string passThis = newSub.ToString("0.00");

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

            return Json(new { UpdateQuantity = newQuantity, newSubTotal = passThis, oldQuan = oldQuantity });
        }

        [HttpPost]
        //Update the quantity after an item deletion
        public ActionResult UpdateRunningTotal(int newQuantity, int id)
        {
            //Makes sure everything is up-to-date
            UpdateSession();

            //temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //returns everything except the to-be-deleted ShoppingCartID
            var check = db.OrderProducts.Where(x => x.Order.UserID == temporary).Where(x => x.OrderProductID != id);

            //Grab the quantity associated with the rest of the shopping cart
            int runningQuantity = 0;
            foreach (var item in check)
            {
                runningQuantity += (int)item.Quantity;
            }

            runningQuantity += newQuantity;
            string passThis = "" + runningQuantity;

            //Pass this new value for quantity
            return Json(new { UpQuantity = passThis });
        }


        //Method designed to update the Shopping Cart when an item is removed from the list
        [HttpPost]
        [Authorize]
        public ActionResult Remove(int id)
        {
            //Delete all instances of the ShoppingCartProductID with this method
            Store.Data.OrderProduct P = new Store.Data.OrderProduct { OrderProductID = id };
            db.OrderProducts.Attach(P);
            db.OrderProducts.Remove(P);

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
            //Pass the ShoppingCartID back to the Ajax function
            return Json(new { newID = id });
        }
    }

}