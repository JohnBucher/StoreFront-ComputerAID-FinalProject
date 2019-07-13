using Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project2_v._2._0.Controllers
{
    public class HomeController : Controller
    {
        private MyDataEntities db = new MyDataEntities();

        // GET: Home
        public ActionResult Index()
        {
            //Make a list of the THREE items to be displayed on the Home Page
            return View(db.Products.Where(x =>  x.ProductName.Contains("1984") ||
                                                x.ProductName.Contains("Harry") ||
                                                x.ProductName.Contains("Animal")).ToList());
        }
    }
}