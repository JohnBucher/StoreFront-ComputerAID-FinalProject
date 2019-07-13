using Store.Data;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project2_v._2._0.Controllers;
using System.Net;
using System.Web.Mvc;
using Project2_v._2._0;
using System.Collections.Generic;
using StoreFront.ShippingApi.Controllers;

namespace Store.Data.Tests
{
    [TestClass()]
    public class DataTest
    {
        //------------------------------------------------------------------------------------------------------------
        //STORED PROCEDURES

        [TestMethod()]
        //TEST:  SPADDUSER_TEST
                //Test that adding a user to the database is successful
        public void spAddUser_Test()
        {
            //ARRANGE
                string testUserName = "testUser"; string testEmailAddress = "testUser@gmail.com";
                MyDataEntities db = new MyDataEntities();
            //ACT
                ObjectResult<Store.Data.spAddUser_Result> result =  db.spAddUser(testUserName, testEmailAddress);
                //Use the testUserName to get the User object that we just made
                var item = db.Users.Where(x => x.UserName == testUserName).FirstOrDefault();

                string returnUserName = item.UserName;
                string returnEmailAddress = item.EmailAddress;
            //ASSERT
            //Compare the UserName and Password we started with and ensure that the User Object has the same properties.
            Assert.AreEqual(testUserName, returnUserName);
                Assert.AreEqual(testEmailAddress, returnEmailAddress);
        }

        [TestMethod()]
        //TEST:  SPGETUSER_TEST
                //Test that a specific user can be retrieved from the database
        public void spGetUser_Test()
        {
            //ARRANGE
                string testUserName = "testUser";
                MyDataEntities db = new MyDataEntities();
                //Grab a specific user based on their UserName to ensure that we can retreieve them from the DB
                var array = db.Users.Where(x => x.UserName == testUserName).ToList();
                ObjectResult<Store.Data.spGetUser_Result> result;
            //ACT
                int initialUserID = 0; int afterUserID = 0;
                foreach (var item in array)
                {
                    //Grab our User's ID
                    initialUserID = item.UserID;
                    //Then call our stored procedure
                    result = db.spGetUser(item.UserID);
                    foreach(var input in result)
                    {
                        //Store the returned User ID to compare later
                        afterUserID = input.UserID;
                    }
                }
            //ASSERT
                //Check to see that our resulting User is the same as the User we were intending to grab
                Assert.AreEqual(initialUserID, afterUserID);
        }

        [TestMethod()]
        //TEST:  SPUPDATEUSER_TEST
                //Test that a user's details can be updated successfully
        public void spUpdateUser_Test()
        {
            //ARRANGE
                string testUserName = "testUser"; string testEmailAddress = "changeTestUser@gmail.com";
                MyDataEntities db = new MyDataEntities();
                //Grab the user we want to manipulate based on the testUserName parameter
                var array = db.Users.Where(x => x.UserName == testUserName).ToList();
                //We already grabbed the user with our previous UserName, now we can change it to what we want to test
                testUserName = "changeTestUser";
            //ACT
                int successfulUpdateID = 0;
                foreach (var item in array)
                {
                    //Update + check the passed back ID
                    successfulUpdateID = db.spUpdateUser(item.UserID, testUserName, testEmailAddress);
                }
            //ASSERT
            //If the entry is successfully updated, the program will return a value of -1. 0 if not successful
                Assert.AreEqual(-1, successfulUpdateID);
        }
        
        [TestMethod()]
        //TEST:  SPDELETEUSER_TEST
                //Test that a user can be successfully deleted from the database
        public void spDeleteUser_Test()
        {
            //ARRANGE
                string testUserName = "changeTestUser";
                MyDataEntities db = new MyDataEntities();
                //Grab the user we want to manipulate based on the testUserName parameter
                var array = db.Users.Where(x => x.UserName == testUserName).ToList();
            //ACT
                int deletedID = 0;
                foreach (var item in array)
                {
                    //Deletion check the passed back ID
                    deletedID = db.spDeleteUser(item.UserID);
                }
            //ASSERT
            //If the entry is successfully deleted, the program will return a value of -1. 0 if not successful
                Assert.AreEqual(-1, deletedID);
        }


        //------------------------------------------------------------------------------------------------------------
        //USERS CONTROLLER METHODS
        
        [TestMethod()]
        //TEST:  USERS_REGISTER_TEST
                //Test the functionality of the User Registration method using test data.
        public void Users_Register_Test()
        {
            //ARRANGE
                UsersController usersController = new UsersController();
                //Create a test user with some test parameters
                Store.Data.User newUser = new User();
                    newUser.UserName = "testRegisterUser";
                    newUser.Password = "testing11";
                    newUser.ConfirmPassword = "testing11";
                    newUser.EmailAddress = "testRegisterUser@gmail.com";
            //ACT
                ViewResult result = usersController.Register(newUser) as ViewResult;
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));

                Assert.AreEqual("", result.ViewName);
        }
        

        [TestMethod()]
        //TEST:  USERS_PROFILE_TEST
                //Test the functionality of the User Profile method using test data.
        public void Users_Profile_Test()
        {
            //ARRANGE
                UsersController usersController = new UsersController();
                MyDataEntities db = new MyDataEntities();
                //Grab the test user that we just created
                var array = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                //use the following to grab the UserID (this method of grabbing the user object and then finding the UserID is to avoid hardcoding
                // a specific ID or object and instead use the newly created one in each test)
                int testUserID = 0;
                foreach (var item in array)
                {
                    testUserID = item.UserID;
                }
            //ACT
                ViewResult result = usersController.DisplayProfile(testUserID) as ViewResult;
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                Assert.AreEqual("", result.ViewName);

                User testUser = new User();
                testUser = db.Users.Where(x => x.UserName == "testRegisterUser").FirstOrDefault();
                //Assert.AreEqual(testUser, result.Model);
        }

        [TestMethod()]
        //TEST:  USERS_PROFILE_FAILURETEST
                //Test the functionality of the User Profile method when using unintended test data.
                //This will result in the same as the noProfile method, because the method attempts to give a view of the user's profile,
                // but if no user is found then it will redirect to the noProfile view.
        public void Users_Profile_FAILURETest()
        {
            //ARRANGE
                UsersController usersController = new UsersController();
                MyDataEntities db = new MyDataEntities();
            
            //ACT
                ViewResult result = usersController.DisplayProfile(6) as ViewResult;
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));

                Assert.AreEqual("noProfile", result.ViewName);
        }

        [TestMethod()]
        //TEST:  USERS_NOPROFILE_TEST
                //Test the functionality of the User noProfile method using test data.
        public void Users_noProfile_Test()
        {
            //ARRANGE
                UsersController usersController = new UsersController();
                MyDataEntities db = new MyDataEntities();

            //ACT
                ViewResult result = usersController.noProfile() as ViewResult;
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));

                Assert.AreEqual("noProfile", result.ViewName);
        }

        [TestMethod()]
        //TEST:  USERS_INDEX_TEST
                //Test the functionality of the User Index method using test data.
        public void Users_Index_Test()
        {
            //ARRANGE
                UsersController usersController = new UsersController();
            //ACT
                ActionResult result = usersController.Index();
            //ASSERT
                Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        //TEST:  USERS_LOGOUT_TEST
                //Test the functionality of the User Logout method using test data.
        public void Users_Logout_Test()
        {
            //ARRANGE
                UsersController usersController = new UsersController();
                MyDataEntities db = new MyDataEntities();

            //ACT
                RedirectResult result = usersController.Logout() as RedirectResult;
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(RedirectResult));

                Assert.IsTrue(result.Url.Equals("~/Home/Index"));
        }


        //------------------------------------------------------------------------------------------------------------
        //PRODUCTS CONTROLLER METHODS

        [TestMethod()]
        //TEST:  PRODUCTS_CREATE_TEST
                //Test the functionality of the Products Create method using test data.
        public void Products_Create_Test()
        {
            //ARRANGE
                ProductsController productsController = new ProductsController();
                //Create a test product with some test parameters
                Store.Data.Product product = new Product();
                    product.ProductName = "testProduct";
                    product.Description = "A test product";
                    product.IsPublished = true;
                    product.Quantity = 1;
                    product.Price = 10;
                    product.ImageFile = "~/ProductImages/1984.jpg";
                    product.DateCreated = DateTime.Now;
                    product.CreatedBy = "dbo";
                    product.DateModified = DateTime.Now;
                    product.ModifiedBy = "dbo";
            //ACT
                ActionResult result = productsController.Create(product);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
        
        [TestMethod()]
        //TEST:  PRODUCTS_DETAILS_TEST
                //Test the functionality of the Product Details method using test data.
        public void Products_Details_Test()
        {
            //ARRANGE
                ProductsController productsController = new ProductsController();
                MyDataEntities db = new MyDataEntities();
                //Grab the previously created test product and find its ProductID
                var array = db.Products.Where(x => x.ProductName == "testProduct").ToList();
                int testProductID = 0;
                foreach (var item in array)
                {
                    testProductID = item.ProductID;
                }
            //ACT
                ActionResult result = productsController.Details(testProductID);
            //ASSERT
                Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        //TEST:  PRODUCTS_INDEX_TEST
                //Test the functionality of the Product Index method using test data.
        public void Products_Index_Test()
        {
            //ARRANGE
                ProductsController productsController = new ProductsController();
            //ACT
                ActionResult result = productsController.Index("testProduct", 1);
            //ASSERT
                Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        //------------------------------------------------------------------------------------------------------------
        //SHOPPING CART CONTROLLER METHODS

        [TestMethod()]
        //TEST:  SHOPPINGCART_ORDERNOW_TEST
                //Test the functionality of the ShoppingCart OrderNow method using test data.
        public void ShoppingCart_OrderNow_Test()
        {
            //ARRANGE
                ShoppingCartController shoppingCartController = new ShoppingCartController();
                MyDataEntities db = new MyDataEntities();

                //Grab the test User and test Product objects and find both of their IDs
                var productArray = db.Products.Where(x => x.ProductName == "testProduct").ToList();
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();

                int testProductID = 0;
                foreach (var item in productArray)
                {
                    testProductID = item.ProductID;
                }

                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

            //ACT
                ActionResult result = shoppingCartController.TestOrderNow(testProductID, testUserID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(RedirectResult));
        }

        [TestMethod()]
        //TEST:  SHOPPINGCART_SHOWCART_TEST
                //Test the functionality of the ShoppingCart ShowCart method using test data.
        public void ShoppingCart_ShowCart_Test()
        {
            //ARRANGE
                ShoppingCartController shoppingCartController = new ShoppingCartController();
                MyDataEntities db = new MyDataEntities();
                //Grab the previously created test user and find their ID
                var initialArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in initialArray)
                {
                    testUserID = item.UserID;
                }  
            //ACT
                ActionResult result = shoppingCartController.ShowCart(testUserID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        //TEST:  SHOPPINGCART_SHOWCART_FAILURETEST
                //Test the functionality of the ShoppingCart ShowCart method when using unintended test data.
                //This will give the method a UserID of a User that does not have their own Shopping Cart. The method will simply redirect
                // the process back to the homepage rather than returning the Shopping Cart View.
        public void ShoppingCart_ShowCart_FAILURETest()
        {
            ShoppingCartController shoppingCartController = new ShoppingCartController();
            MyDataEntities db = new MyDataEntities();

            //ARRANGE
                var initialArray = db.Users.Where(x => x.UserName == "No one").ToList();
                int testUserID = 0;
                foreach (var item in initialArray)
                {
                    testUserID = item.UserID;
                }
            //ACT
                RedirectResult result = shoppingCartController.ShowCart(testUserID) as RedirectResult;
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(RedirectResult));

                Assert.IsTrue(result.Url.Equals("~/Home/Index"));
        }

        [TestMethod()]
        //TEST:  SHOPPINGCART_INDEX_TEST
                //Test the functionality of the ShoppingCart Index method using test data.
        public void ShoppingCart_Index_Test()
        {
            //ARRANGE
                ShoppingCartController shoppingCartController = new ShoppingCartController();
            //ACT
                ActionResult result = shoppingCartController.Index();
            //ASSERT
                Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        //------------------------------------------------------------------------------------------------------------
        //ORDER CONTROLLER METHODS

        [TestMethod()]
        //TEST:  ORDER_CREATEADDRESS_TEST
                //Test the functionality of the Order CreateAddress method using test data.
        public void Order_CreateAddress_Test()
        {
            //ARRANGE
                OrderController orderController = new OrderController();
                MyDataEntities db = new MyDataEntities();
                //Grab the previously created test user and find their ID
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }
                //Create a test address using test parameters
                Store.Data.Address newAddress = new Address();
                    newAddress.Address1 = "test";
                    newAddress.City = "test";
                    newAddress.UserID = testUserID;
                    newAddress.StateID = 25;
            //ACT
                ActionResult result = orderController.CreateAddressWithUser(newAddress, testUserID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod()]
        //TEST:  ORDER_CONFIRMATIONMESSAGE_TEST
                //Test the functionality of the Order ConfirmationMessage method using test data.
        public void Order_ConfirmationMessage_Test()
        {
            //ARRANGE
                OrderController orderController = new OrderController();
                MyDataEntities db = new MyDataEntities();
                //Grab the previously created test user and find their ID
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

                //Using the UserID return the list of Shopping Cart products and calculate the running total
                var check = db.ShoppingCartProducts.Where(x => x.ShoppingCartID == testUserID).ToList();
                decimal runningTotal = 0;
                foreach (var item in check)
                {
                    runningTotal += (decimal)(item.Quantity * item.Product.Price);
                }
            //ACT
                ActionResult result = orderController.ShowConfirmationMessage(testUserID, 1, runningTotal);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        //TEST:  ORDER_INDEX_TEST
                //Test the functionality of the Order Index method using test data.
        public void Order_Index_Test()
        {
            //ARRANGE
                OrderController orderController = new OrderController();
                MyDataEntities db = new MyDataEntities();
                //Grab the previously created test user and find their ID
                var array = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in array)
                {
                    testUserID = item.UserID;
                }
            //ACT
                ActionResult result = orderController.ShowIndex(1, testUserID);
            //ASSERT
                Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        //TEST:  ORDER_PLUSSTATUS_TEST
                //Test the functionality of the Order plusStatus method using test data.
        public void Order_plusStatus_Test()
        {
            //ARRANGE
                OrderController orderController = new OrderController();
                MyDataEntities db = new MyDataEntities();
                //Grab the previously created test user and find their ID
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

                //Using the UserID, create a list of Orders owned by the user (Should be 1 order) whose status will then be incremented
                var check = db.Orders.Where(x => x.UserID == testUserID).ToList();
                int testOrderID = 0; int testStatusID = 0;
                foreach (var item in check)
                {
                    testOrderID = item.OrderID;
                    testStatusID = (int)item.StatusID;
                }
            //ACT
                ActionResult result = orderController.plusStatus(testStatusID, testOrderID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod()]
        //TEST:  ORDER_REMOVE_TEST
                //Test the functionality of the Order Remove method using test data.
        public void Order_Remove_Test()
        {
            //ARRANGE
                OrderController orderController = new OrderController();
                MyDataEntities db = new MyDataEntities();
                //Grab the previously created test user and test product and find their IDs
                var productArray = db.Products.Where(x => x.ProductName == "testProduct").ToList();
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();

                int testProductID = 0;
                foreach (var item in productArray)
                {
                    testProductID = item.ProductID;
                }
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

                //Using the UserID create a list of orders owned by the user (Should cotnain 1 order) and record the orderID
                var orderArray = db.Orders.Where(x => x.UserID == testUserID).ToList();
                int testOrderID = 0;
                foreach (var item in orderArray)
                {
                    testOrderID = item.OrderID;
                }

                //Using the OrderID and ProductID get the list of OrderProducts (Should contain 1 item) and record its OrderProductID to be
                // used int he remove method
                var orderProductArray = db.OrderProducts.Where(x => x.OrderID == testOrderID)
                                            .Where(x => x.ProductID == testProductID).ToList();
                int testOrderProductID = 0;
                foreach (var item in orderProductArray)
                {
                    testOrderProductID = item.OrderProductID;
                }
            //ACT
                ActionResult result = orderController.Remove(testOrderProductID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        //------------------------------------------------------------------------------------------------------------
        //SqlSecurityManager METHODS

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_REGISTERUSER_TEST
                //Test the functionality of the SqlSecurityManager RegisterUser method using test data.
        public void SqlSecurityManager_RegisterUser_Test()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                //Create a test user with test parameters
                Store.Data.User newUser = new User();
                    newUser.UserName = "testSqlUser";
                    newUser.Password = "testing11";
                    newUser.ConfirmPassword = "testing11";
                    newUser.EmailAddress = "testSqlUser@gmail.com";
            //ACT
                int result = manager.RegisterUser(newUser);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.AreEqual(result, 1);

        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_REGISTERUSER_FAILURETEST
                //Test the functionality of the SqlSecurityManager RegisterUser method when using unintended test data.
                //Previous method created this User, therefore the UserName should be reserved, and this method will not be able to use it
        public void SqlSecurityManager_RegisterUser_FAILURETest()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                //Create a test user with test parameters
                Store.Data.User newUser = new User();
                    newUser.UserName = "testSqlUser";
                    newUser.Password = "testing11";
                    newUser.ConfirmPassword = "testing11";
                    newUser.EmailAddress = "testSqlUser@gmail.com";
            //ACT
                int result = manager.RegisterUser(newUser);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.AreEqual(result, -1);

        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_AUTHENTICATEUSER_TEST
                //Test the functionality of the SqlSecurityManager AuthenticateUser method using test data.
        public void SqlSecurityManager_AuthenticateUser_Test()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                //These parameters refer to the test user that we created in SqlSecurityManager_RegisterUser
                string username = "testSqlUser";
                string password = "testing11";
                bool result;
            //ACT
                result = manager.AuthenticateUser(username, password);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsTrue(result);
        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_AUTHENTICATEUSER_FAILURETEST
                //Test the functionality of the SqlSecurityManager AuthenticateUser method when using unintended test data.
                //Uses a wrong password to show that if the username and/or password are wrong then AuthenticateUser() will return false
        public void SqlSecurityManager_AuthenticateUser_FAILURETest()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                //Login parameters
                string username = "testSqlUser";
                //Wrong Password
                string password = "testing12";
                bool result;
            //ACT
                result = manager.AuthenticateUser(username, password);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsFalse(result);
        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_ISADMIN_TEST
                //Test the functionality of the SqlSecurityManager IsAdmin method using test data.
        public void SqlSecurityManager_IsAdmin_Test()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                //The only Admin username in the DB
                string username = "JohnBucher";
                bool result;
            //ACT
                result = manager.IsAdmin(username);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsTrue(result);
        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_ISADMIN_FAILURETEST
                //Test the functionality of the SqlSecurityManager IsAdmin method when using unintended test data.
                //Uses our newly created user who is not an admin to show that IsAdmin will return false when given a non-admin.
        public void SqlSecurityManager_IsAdmin_FAILURETest()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                //These parameters refer to the test user that we created in SqlSecurityManager_RegisterUser
                string testUsername = "testSqlUser";
                bool result;
            //ACT
                result = manager.IsAdmin(testUsername);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsFalse(result);
        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_LOADUSER_TEST
                //Test the functionality of the SqlSecurityManager LoadUser method using test data.
        public void SqlSecurityManager_LoadUser_Test()
        {
            SqlSecurityManager manager = new SqlSecurityManager();
            MyDataEntities db = new MyDataEntities();

            //ARRANGE
                //Grab the test user that we created in SqlSecurityManager_RegisterUser and find their UserID
                var userArray = db.Users.Where(x => x.UserName == "testSqlUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

                //These parameters refer to the test user that we created in SqlSecurityManager_RegisterUser
                User test;
                string username = "testSqlUser";
            //ACT
                test = manager.LoadUser(username);
            //ASSERT
                Assert.AreEqual(testUserID, test.UserID);
        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_LOADUSER_FAILURETEST
                //Test the functionality of the SqlSecurityManager LoadUser method when using unintended test data.
                //Loading a username that is not owned by any User will simply return a null User.
        public void SqlSecurityManager_LoadUser_FAILURETest()
        {
            SqlSecurityManager manager = new SqlSecurityManager();
            MyDataEntities db = new MyDataEntities();

            //ARRANGE
                User test;
                string username = "No one";
            //ACT
                test = manager.LoadUser(username);
            //ASSERT
                Assert.AreEqual(0, test.UserID);
                Assert.IsNull(test.UserName);
                Assert.IsNull(test.Password);
        }

        [TestMethod()]
        //TEST:  SQLSECURITYMANAGER_DELETEUSER_TEST
                //Test the functionality of the SqlSecurityManager DeleteUser method using test data.
        public void SqlSecurityManager_DeleteUser_Test()
        {
            SqlSecurityManager manager = new SqlSecurityManager();
            MyDataEntities db = new MyDataEntities();

            //ARRANGE
                //Grab the test user that we created in SqlSecurityManager_RegisterUser and find their UserID to be used in the Delete method
                var userArray = db.Users.Where(x => x.UserName == "testSqlUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }
            //ACT
                int result = manager.Delete("testSqlUser");
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(int));
                Assert.AreEqual(result, 1);
        }

        //------------------------------------------------------------------------------------------------------------
        //WebAPI METHODS

        [TestMethod()]
        //TEST:  WEBAPI_GETORDERS_TEST
                //Test the functionality of the WebAPI GetOrders method using test data.
        public void WebAPI_GetOrders_Test()
        {
            MyDataEntities db = new MyDataEntities();
            OrderShippingController os = new OrderShippingController();

            //ARRANGE
                db.SaveChanges();
                List<Order> list = new List<Order>();
                //This time value is equal to 1 minute and the purpose is to subtract the current time by 1 minutes and get all the orders created
                //within the last minute (we will only find our test order)
                TimeSpan time = new TimeSpan(0, 1, 0);
                DateTime startDate = DateTime.Now.Subtract(time);
                DateTime endDate = DateTime.Now;
            //ACT
                list = os.GetOrders(startDate, endDate);
            //ASSERT
                Assert.IsNotNull(list);
                Assert.IsInstanceOfType(list, typeof(List<Order>));
                Assert.IsTrue(list.Count() == 1);
        }

        [TestMethod()]
        //TEST:  WEBAPI_GETORDERS_FAILURETEST
                //Test the functionality of the WebAPI GetOrders method when using unintended test data.
                //The input dates are reversed and the result of invoking GetOrders simply gives back an empty array
        public void WebAPI_GetOrders_FAILURETest()
        {
            MyDataEntities db = new MyDataEntities();
            OrderShippingController os = new OrderShippingController();

            //ARRANGE
                List<Order> list = new List<Order>();
                //This time value is equal to 1 minute and the purpose is to subtract the current time by 1 minutes and get all the orders created
                //within the last minute (we will only find our test order)
                TimeSpan time = new TimeSpan(0, 1, 0);

                //START DATE IS AFTER END DATE!! THIS IS THE REASON FOR THE FAILURE
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.Subtract(time);
            //ACT
                list = os.GetOrders(startDate, endDate);
            //ASSERT
                Assert.IsNotNull(list);
                Assert.IsInstanceOfType(list, typeof(List<Order>));
                Assert.IsTrue(list.Count() == 0);
        }

        [TestMethod()]
        //TEST:  WEBAPI_MARKORDERSHIPPED_TEST
                //Test the functionality of the WebAPI MarkOrderShipped method using test data.
        public void WebAPI_MarkOrderShipped_Test()
        {
            MyDataEntities db = new MyDataEntities();
            OrderShippingController os = new OrderShippingController();

            //ARRANGE
                //Grab the test user that we created in SqlSecurityManager_RegisterUser and find their UserID
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

                //Use the UserID to find the list of Orders owned by this User (Should cotnain 1 order) and return the OrderID
                var list = db.Orders.Where(x => x.UserID == testUserID).ToList();
                int testOrderID = 0;
                foreach (var item in list)
                {
                    testOrderID = item.OrderID;
                }
            //ACT
                string result = os.MarkOrderShipped(testOrderID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Equals("Order Status set to: Order Shipped"));
        }

        [TestMethod()]
        //TEST:  WEBAPI_MARKORDERSHIPPED_FAILURETEST
                //Test the functionality of the WebAPI MarkOrderShipped method when using unintended test data.
                //When given an integer that does not match an order in the DB (this could be a negative number or simply a number not 
                // associated with an order) The method will simply return a string that says: "Failure: No Order Found"
        public void WebAPI_MarkOrderShipped_FAILURETest()
        {
            MyDataEntities db = new MyDataEntities();
            OrderShippingController os = new OrderShippingController();

            //ARRANGE

            //ACT
                string result = os.MarkOrderShipped(1);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Equals("Failure: No Order Found"));
        }

        //------------------------------------------------------------------------------------------------------------
        //DELETE METHODS

        [TestMethod()]
        //TEST:  SHOPPINGCART_DELETE_TEST
                //Test the functionality of the ShoppingCart Delete method using test data.
        public void ShoppingCart_Delete_Test()
        {
            //ARRANGE
                ShoppingCartController shoppingCartController = new ShoppingCartController();
                MyDataEntities db = new MyDataEntities();
                //Grab the test user that we created in Users_Register and find their UserID. This is used to delete the User's cart
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

            //ACT
                ActionResult result = shoppingCartController.Delete(testUserID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
        
        [TestMethod()]
        //TEST:  ORDER_DELETE_TEST
                //Test the functionality of the Order Delete method using test data.
        public void Order_Delete_Test()
        {
            //ARRANGE
                OrderController orderController = new OrderController();
                MyDataEntities db = new MyDataEntities();
                //Grab the test user that we created in Users_Register and find their UserID
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

                //Using the UserID return the list of Orders owned by the User (Should contain 1 order) and record its OrderID
                var OrderArray = db.Orders.Where(x => x.UserID == testUserID).ToList();
                int testOrderID = 0;
                foreach (var item in OrderArray)
                {
                    testOrderID = item.OrderID;
                }
            //ACT
                ActionResult result = orderController.Delete(testOrderID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
        
        [TestMethod()]
        //TEST:  PRODUCT_DELETE_TEST
                //Test the functionality of the Product Delete method using test data.
        public void Products_Delete_Test()
        {
            //ARRANGE
                ProductsController productsController = new ProductsController();
                MyDataEntities db = new MyDataEntities();
                //Grab the test product that we created in Products_Create and find its ProductID
                var array = db.Products.Where(x => x.ProductName == "testProduct").ToList();
                int testProductID = 0;
                foreach (var item in array)
                {
                    testProductID = item.ProductID;
                }
            //ACT
                ActionResult result = productsController.Delete(testProductID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
        
        [TestMethod()]
        //TEST:  ORDER_DELETEADDRESS_TEST
                //Test the functionality of the Order DeleteAddress method using test data.
        public void Order_DeleteAddress_Test()
        {
            //ARRANGE
                OrderController orderController = new OrderController();
                MyDataEntities db = new MyDataEntities();
                //Grab the test product that we created in Users_Register and find their UserID
                var userArray = db.Users.Where(x => x.UserName == "testRegisterUser").ToList();
                int testUserID = 0;
                foreach (var item in userArray)
                {
                    testUserID = item.UserID;
                }

                //Using the UserID return the list of Addresses useable by the User (Should contain 1 Address) and record its AddressID
                var array = db.Addresses.Where(x => x.UserID == testUserID).ToList();
                int testAddressID = 0;
                foreach (var item in array)
                {
                    testAddressID = item.AddressID;
                }
            //ACT
                ActionResult result = orderController.DeleteAddress(testAddressID);
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
        
        [TestMethod()]
        //TEST:  USER_DELETE_TEST
                //Test the functionality of the User Delete method using test data.
        public void Users_Delete_Test()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                UsersController usersController = new UsersController();
                MyDataEntities db = new MyDataEntities();
            //ACT
                int result = manager.Delete("testRegisterUser");
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(int));
                Assert.AreEqual(result, 1);
        }

        [TestMethod()]
        //TEST:  USER_DELETE_FAILURETEST
                //Test the functionality of the User Delete method when using unintended test data.
                //Because the User has already been deleted by the previous method, this will result in a value of -1 (signifying a 
                // failure to delete a user), rather than the value of 1 that is returned on the successful deletion of a user.
        public void Users_Delete_FAILURETest()
        {
            SqlSecurityManager manager = new SqlSecurityManager();

            //ARRANGE
                UsersController usersController = new UsersController();
                MyDataEntities db = new MyDataEntities();
            //ACT
                int result = manager.Delete("testRegisterUser");
            //ASSERT
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(int));
                Assert.AreEqual(result, -1);
        }

    }
}
