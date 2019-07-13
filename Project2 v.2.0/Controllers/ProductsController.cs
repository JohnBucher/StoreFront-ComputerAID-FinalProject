using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PagedList;
using PagedList.Mvc;
using Store.Data;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace Project2_v._2._0.Controllers
{
    public class ProductsController : Controller
    {
        MyDataEntities db = new MyDataEntities();

        //IsMember
        //This method checks to see if a user exists with the current Session's UserName and if so return true, otherwise return false.
        public bool IsMember()
        {
            if(db.Users.Where(x => x.UserName == Session["UserName"].ToString()) != null)
            {
                return true;
            }
            return false;
        }

        //Index
        //This method will return a list of products based on a search string while also utilizing paed list controls
        public ActionResult Index(string search, int? page)
        {
                return View(db.Products.Where(x => x.ProductName.Contains(search) || search == null).ToList().ToPagedList( page ?? 1, 50));
        }

        //AddImage
        [Authorize]
        public ActionResult AddImage()
        {
            Store.Data.Product p1 = new Store.Data.Product();
            return View(p1);
        }

        //AddImage
        [HttpPost]
        [Authorize]
        public ActionResult AddImage(string text)
        {
            return View();
        }

        //Details
        //This method provides a detail view of a selected product object.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store.Data.Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ProductName,Description,IsPublished,Quantity,Price,ImageFile,DateCreated,CreatedBy,DateModified,ModifiedBy")] Store.Data.Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //Edit
        [Authorize] 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store.Data.Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ProductName,Description,IsPublished,Quantity,Price,ImageFile,DateCreated,CreatedBy,DateModified,ModifiedBy")] Store.Data.Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //Delete
        //This method is in charge of deleting product objects based on the given ProductID
        public ActionResult Delete(int id)
        {
            //Delete all instances of the ShoppingCartProductID with this method
            Store.Data.Product P = new Store.Data.Product { ProductID = id };
            db.Products.Attach(P);
            db.Products.Remove(P);

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

        /*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Store.Data.Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
