using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using Store.Data;

namespace Project2_v._2._0.Controllers
{

    public class ShoppingCartController : Controller
    {
        private MyDataEntities db = new MyDataEntities();

//---------------------------------------------------------------------------------------------------------------------------------------------------
    //AJAX METHODS

        //Updates the Total and Quantity Session variables
        [Authorize]
        public void UpdateSession()
        {
            //temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //returns a list of rows that belong to the user's cart and don't include the ShoppingCartProduct that is getting deleted
                //EVERYTHING ELSE in the cart except for the item being deleted
            var productList = db.ShoppingCartProducts.Where(x => x.ShoppingCartID == temporary);

            //UPDATING THE QUANTITY AND TOTAL COUNTS BASED ON THE CURRENT USER'S SHOPPING CART
            int quan = 0; decimal total = 0;
            foreach (var item in productList)
            {
                quan += item.Quantity;
                total += (decimal)(item.Product.Price * item.Quantity);
            }

            Session["Quantity"] = quan;
            Session["Total"] = total.ToString("0.00");
        }

        //----------------------------------------------------

        [HttpPost]
        [Authorize]
        //Update the total after an item deletion
        public ActionResult UpdateTotal(int id)
        {
            //Makes sure everything is up-to-date
            UpdateSession();

            //Now that Total is up-to-date, pass it to the Ajax function
            string newTotal = Session["Total"].ToString();
            //Pass this value for total
            return Json(new { UpTotal = newTotal});
        }

        //----------------------------------------------------

        [HttpPost]
        [Authorize]
        //Update the quantity after an item deletion
        public ActionResult UpdateQuantity(int id)
        {
            if (Session.Count == 0)
            {
                return Redirect("~/Users/Login");
            }

            //Makes sure everything is up-to-date
            UpdateSession();

            //temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //returns the row with the to-be-deleted ShoppingCartID
            var check = db.ShoppingCartProducts.Where(x => x.ShoppingCartID == temporary).Where(x => x.ShoppingCartProductID == id);

            //Grab the quantity associated with the ShoppingCartProductID
            int ProductQuantity = 0;
            foreach(var item in check)
            {
                ProductQuantity = item.Quantity;
            }
            //Take the overall quantity minus the quantity that is to be deleted
            ProductQuantity = Convert.ToInt32(Session["Quantity"].ToString()) - ProductQuantity;
            string passThis = "" + ProductQuantity;

            //Pass this new value for quantity
            return Json(new { UpQuantity = passThis });
        }

        //----------------------------------------------------

        [HttpPost]
        //Update the quantity after an item deletion
        public ActionResult UpdateItemQuantity(int newQuantity, int itemID)
        {
            //Makes sure everything is up-to-date
            UpdateSession();

            //temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //returns the row with the to-be-deleted ShoppingCartID
            var check = db.ShoppingCartProducts.Where(x => x.ShoppingCartID == temporary).Where(x => x.ShoppingCartProductID == itemID);

            //Grab the quantity associated with the ShoppingCartProductID
            decimal newSub = 0;
            int oldQuantity = 0;
            foreach (var item in check)
            {
                if (newQuantity > 0) { item.Quantity = newQuantity; newSub = Convert.ToDecimal(newQuantity * item.Product.Price); }
                else { oldQuantity = item.Quantity; newSub = Convert.ToDecimal(oldQuantity * item.Product.Price); }
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

            return Json(new { UpdateQuantity = newQuantity, newSubTotal = passThis, oldQuan = oldQuantity});
        }

        //----------------------------------------------------

        [HttpPost]
        //Update the quantity after an item deletion
        public ActionResult UpdateRunningTotal(int newQuantity, int id)
        {
            //Makes sure everything is up-to-date
            UpdateSession();

            //temporary variable holding the integer representing the User's ID (and therefore the CartID)
            int temporary = Convert.ToInt32(Session["UserID"].ToString());
            //returns everything except the to-be-deleted ShoppingCartID
            var check = db.ShoppingCartProducts.Where(x => x.ShoppingCartID == temporary).Where(x => x.ShoppingCartProductID != id);

            //Grab the quantity associated with the rest of the shopping cart
            int runningQuantity = 0;
            foreach (var item in check)
            {
                runningQuantity += item.Quantity;
            }

            runningQuantity += newQuantity;
            string passThis = "" + runningQuantity;

            //Pass this new value for quantity
            return Json(new { UpQuantity = passThis });
        }

        //----------------------------------------------------

        //Method designed to update the Shopping Cart when an item is removed from the list
        [HttpPost]
        [Authorize]
        public ActionResult Remove( int id )
        { 
            //Delete all instances of the ShoppingCartProductID with this method
            Store.Data.ShoppingCartProduct S = new Store.Data.ShoppingCartProduct { ShoppingCartProductID = id };
            db.ShoppingCartProducts.Attach(S);
            db.ShoppingCartProducts.Remove(S);

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
            return Json( new { newID= id } );
        }
//---------------------------------------------------------------------------------------------------------------------------------------------------

        //Index
        //This method returns a listing of all ShoppingCarts
        public ActionResult Index()
        {
            return View();
        }

        //Cart
        //The precursor to ShowCart, this method records and passes the current UserID to ShowCart while also updating the quantity and money total
        // by using UpdateSession()
        [Authorize]
        public ActionResult Cart()
        {
            int temp = Convert.ToInt32(Session["UserID"].ToString());
            UpdateSession();
            return RedirectToAction("ShowCart", "ShoppingCart", new { userID = temp });
        }

        //ShowCart
        //This method gives the user a current view of their shopping cart
        [Authorize]
        public ActionResult ShowCart(int userID)
        {
            //returns the ShoppingCart assigned to the user
            ShoppingCart shopCart = db.ShoppingCarts.Where(x => x.ShoppingCartID == userID).FirstOrDefault();
            //Using that shopping cart, return a list of products that exist within that shopping cart
            var productList = db.ShoppingCartProducts.Where(a => a.ShoppingCartID == userID);

            //If a Shopping Cart does not exist for the selected user then take them back to the homepage
            if (shopCart == null)
            {
                return Redirect("~/Home/Index");
            }

            //Return the view of our product list
            return View(productList);
        }

        //OrderNow
        //This method will check to see if the User has a Shopping Cart and if they don't then it will create one for them. Then It will check if
        // the current product exist in the Shopping Cart. If it does then it will increment the Quantity by 1, if not then it will add the product
        // to the ShoppingCartProducts 
        [Authorize]
        public ActionResult OrderNow(int? id)
        {
            int userID = 0;
            if(Session != null)
            {
                userID = Convert.ToInt32(Session["UserID"].ToString());
            }

            //Checks if any cart exists
            if (db.ShoppingCarts.FirstOrDefault(t => t.ShoppingCartID == userID) == null)
            {
                //If not, create a new one with parameters
                Store.Data.ShoppingCart S = new Store.Data.ShoppingCart();
                S.ShoppingCartID = userID;
                S.UserID = userID;
                S.CreatedBy = "dbo";
                DateTime date1 = DateTime.Now;
                S.DateCreated = date1;
                S.ModifiedBy = "dbo";
                S.DateModified = date1;

                //Add the new ShoppingCart to the DB
                db.ShoppingCarts.Add(S);

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
            }
            //check if product exists in the user's cart
            if (db.ShoppingCartProducts.FirstOrDefault(t => t.ShoppingCartID == userID && t.ProductID == id) == null)
            {
                //If the product does not exist, then add it to the shopping cart
                Store.Data.ShoppingCartProduct Shop = new Store.Data.ShoppingCartProduct();
                Shop.ShoppingCartID = userID;
                Shop.ProductID = (int)id;
                Shop.Quantity = 1;
                Shop.CreatedBy = "dbo";
                DateTime date1 = DateTime.Now;
                Shop.DateCreated = date1;
                Shop.ModifiedBy = "dbo";
                Shop.DateModified = date1;

                //Add the new ShoppingCartProduct to the DB
                db.ShoppingCartProducts.Add(Shop);
            }
            //If the Product already exists in the Shopping Cart, simply increase the quantity
            else
            {
                db.ShoppingCartProducts.FirstOrDefault(t => t.ProductID == id && t.ShoppingCartID == userID).Quantity++;
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
            //Redirect to the ShoppingCart view
            return Redirect("~/ShoppingCart/Cart");
        }

        //This method is in charge of the deletion of ShoppingCarts
        public ActionResult Delete(int id)
        {
            //Delete all instances of the ShoppingCartProductID with this method
            Store.Data.ShoppingCart S = new Store.Data.ShoppingCart { ShoppingCartID = id };
            db.ShoppingCarts.Attach(S);
            db.ShoppingCarts.Remove(S);

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

        //-----------------------------------------------------------------------------------------------------------------
        //Methods not used in program but reworked to enable testing

        //OrderNow
        //This method will check to see if the User has a Shopping Cart and if they don't then it will create one for them. Then It will check if
        // the current product exist in the Shopping Cart. If it does then it will increment the Quantity by 1, if not then it will add the product
        // to the ShoppingCartProducts 
        [Authorize]
        public ActionResult TestOrderNow(int? id, int userID)
        {
            //Checks if any cart exists
            if (db.ShoppingCarts.FirstOrDefault(t => t.ShoppingCartID == userID) == null)
            {
                //If not, create a new one with parameters
                Store.Data.ShoppingCart S = new Store.Data.ShoppingCart();
                S.ShoppingCartID = userID;
                S.UserID = userID;
                S.CreatedBy = "dbo";
                DateTime date1 = DateTime.Now;
                S.DateCreated = date1;
                S.ModifiedBy = "dbo";
                S.DateModified = date1;

                //Add the new ShoppingCart to the DB
                db.ShoppingCarts.Add(S);

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
            }
            //check if product exists in the user's cart
            if (db.ShoppingCartProducts.FirstOrDefault(t => t.ShoppingCartID == userID && t.ProductID == id) == null)
            {
                //If the product does not exist, then add it to the shopping cart
                Store.Data.ShoppingCartProduct Shop = new Store.Data.ShoppingCartProduct();
                Shop.ShoppingCartID = userID;
                Shop.ProductID = (int)id;
                Shop.Quantity = 1;
                Shop.CreatedBy = "dbo";
                DateTime date1 = DateTime.Now;
                Shop.DateCreated = date1;
                Shop.ModifiedBy = "dbo";
                Shop.DateModified = date1;

                //Add the new ShoppingCartProduct to the DB
                db.ShoppingCartProducts.Add(Shop);
            }
            //If the Product already exists in the Shopping Cart, simply increase the quantity
            else
            {
                db.ShoppingCartProducts.FirstOrDefault(t => t.ProductID == id && t.ShoppingCartID == userID).Quantity++;
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
            //Redirect to the ShoppingCart view
            return Redirect("~/ShoppingCart/Cart");
        }

    }
}