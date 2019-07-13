using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web.Security;
using Store.Data;

namespace Project2_v._2._0.Controllers
{
    public class UsersController : Controller
    {
        private MyDataEntities db = new MyDataEntities();
        SqlSecurityManager manager = new SqlSecurityManager();

        //USER REGISTRATION
        //-----------------------------------------------------------------------------------------

        //Register
        public ActionResult Register()
        {
            return View();
        }
        
        //Register
        //This method is in charg eof Registering users by using the SqlSecurityManager. If the registration is successful then a success message is
        // displayed.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Register(Store.Data.User U)
        {
            if(ModelState.IsValid)
            {
                int result = manager.RegisterUser(U);

                if(result == 1)
                {
                    ModelState.Clear();
                    U = null;
                    ViewBag.Message = "User Successfully Registered";
                }
                else
                {
                    ModelState.AddModelError("UserName", "This Username already exists");
                }
            }
            return View(U);
        }

        //-----------------------------------------------------------------------------------------


        //USER LOGIN    
        //-----------------------------------------------------------------------------------------

        //Login
        public ActionResult Login()
        {
            return View();
        }

        //Login
        //This method is in charge of User login by utilizing the SqlSecurityManager to authenticate the user. The method will also use Forms Authentification
        // to give the current user access to [Authorize] blocks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Store.Data.User user)
        {
            using (MyDataEntities db = new MyDataEntities())
                {
                    //Check to see that the UserName matches a User and that the Password matches that User
                    if(manager.AuthenticateUser(user.UserName, user.Password))
                    {
                        //AUTHORIZATION USING COOKIES
                        //--------------------------------------------------------------
                        int timeout = 100;
                        var ticket = new FormsAuthenticationTicket(1, user.UserID.ToString(), DateTime.Now, DateTime.Now.AddMinutes(20), 
                            false, user.UserName.ToString(), FormsAuthentication.FormsCookiePath);
                        string encrypt = FormsAuthentication.Encrypt(ticket);

                        FormsAuthentication.SetAuthCookie(user.UserName, false);

                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                    //--------------------------------------------------------------

                    //Makes sure that the UserNames match and create a variable to hold the user in question
                    var usr = db.Users.Where(U => U.UserName == user.UserName).FirstOrDefault();
                        Session["UserID"] = usr.UserID.ToString();
                        Session["UserName"] = usr.UserName.ToString();

                        if (usr.IsAdmin == true)
                        {
                            Session["IsAdmin"] = 1;
                        }
                        else
                        {
                            Session["IsAdmin"] = 0;
                        }

                        //Create a list of the user's ShoppingCartProducts in order to find the total quantity of items
                        int temp = Convert.ToInt32(Session["UserID"].ToString());
                        var productList = db.ShoppingCartProducts.Where(a => a.ShoppingCartID == temp);
                        int quan = 0;
                        foreach (var item in productList)
                        {
                            quan += item.Quantity;
                        }
                        //Set the Session variable Quantity to the found value.
                        Session["Quantity"] = quan;

                        //Redirect the now logged in user back to the Homepage
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or Password is incorrect");
                    }
            }
            return View();
        }

        //Logout
        //This method is in charge of Logging out a currently Logged in user
        [Authorize]
        public ActionResult Logout()
        {
            //If the Session does not equal null then delete all of the authorization given to the user to access [Authorize] blocks and 
            // clear the Session
            if(Session != null)
            {
                //Deleting Authentification Cookies
                FormsAuthentication.SignOut();
                //Clearing Session variables
                Session.Clear();
            }
            //Then redirect the now Logged out user back to the Homepage
            return Redirect("~/Home/Index");
        }

        //-----------------------------------------------------------------------------------------

        //Index
        //This method provides a listing of all users in the DB
        [Authorize]
        public ActionResult Index()
        {
            using (MyDataEntities db = new MyDataEntities())
            {
                return View();
            }
        }

        //Profile
        //The precursor to the method DisplayProfile(), this method will grab the current user's ID and pass it to the DisplayProfile method.
        [Authorize]
        public new ActionResult Profile()
        {
            int temp = Convert.ToInt32(Session["UserID"].ToString());
            return RedirectToAction("DisplayProfile", "Users", new { userID = temp });
        }

        //DisplayProfile
        //This method will take in the user ID and will display user data to the screen.
        [Authorize]
        public new ViewResult DisplayProfile(int userID)
        {
            //Find the specific user based on the userID
            Store.Data.User user = db.Users.Find(userID);
            //If the user is not null then display the view
            if (user != null)
            {
                return View(user);
            }
            //Otherwise redirect to the noProfile view.
            return noProfile();
        }

        //noProfile
        //This method is what is displayed when no profile is found for the user.
        public ViewResult noProfile()
        {
            return View("noProfile");
        }
    }
}
