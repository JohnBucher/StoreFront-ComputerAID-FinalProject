using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Data;
using System.Web.Mvc;
using System.Web;
using System.Security.Cryptography;

namespace Project2_v._2._0
{
    public class SqlSecurityManager : Controller
    {
        private MyDataEntities db = new MyDataEntities();

        public bool AuthenticateUser(string username, string password)
        {
            //Makes sure that the UserNames match and create a variable to hold the user in question
            var usr = db.Users.Where(U => U.UserName == username).FirstOrDefault();
            //If a user has matched and their password matches the user password, then set the session to this user.
            if (usr != null && ValidatePassword(password, usr.Password))
            {
                //If the Session is not null then set the USerName session variable
                if(Session != null)
                {
                    Session["UserName"] = username;
                }
                return true;
            }
            return false;
        }

        public bool IsAdmin(string username)
        {
            //Return a list of Users where th User's username is the input username AND they are an Admin
            List<User> list = db.Users.Where(x => x.UserName == username).Where(x => x.IsAdmin == true).ToList();
            //If a user exists that matches this criteria then return true
            if (list.Count != 0)
            {
                return true;
            }
            //otherwise return false
            return false;
        }

        public User LoadUser(string username)
        {
            //Return the User object with the corresponding username
            return db.Users.Where(x => x.UserName == username).FirstOrDefault();
        }

        public void SaveUser()
        {
            //Enact a save on the DB to ensure parity between the data we work with and the state of the DB
            db.SaveChanges();
        }

        public int RegisterUser(Store.Data.User U)
        {
            //Passback is our return value. A value of 1 indicates a successful registered user while a value of -1 indicates an unsuccessful register 
            int passBack = 0;
            using (MyDataEntities db = new MyDataEntities())
            // Check if UserName already exists
            if (db.Users.FirstOrDefault(t => t.UserName.ToLower() == U.UserName.ToLower()) == null)
            {
                //Populate the User with necessary information while also hashing the Password
                U.Password = CreateTheHash(U.Password);
                U.ConfirmPassword = U.Password;
                U.IsAdmin = false;
                U.CreatedBy = "dbo";
                DateTime date1 = DateTime.Now;
                U.DateCreated = date1;
                U.ModifiedBy = "dbo";
                U.DateModified = date1;

                //Add the User to the DB
                db.Users.Add(U);

                //Try saving the DB
                try
                {
                        SaveUser();
                }
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
                passBack = 1;
            }//End IF
            else
            {
                passBack = -1;
            }//END ELSE

            return passBack; 
        }

        private const int SALT_BYTES = 24;
        private const int HASH_BYTES = 24;
        private const int PBKDF2_ITERATIONS = 1000;

        private const int ITERATION_INDEX = 0;
        private const int SALT_INDEX = 1;
        private const int PBKDF2_INDEX = 2;

        public string CreateTheHash(string passwordToHash)
        {
            //Generate the random salt
            RNGCryptoServiceProvider RNGcsp = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTES];
            RNGcsp.GetBytes(salt);

            //Hash the password and encode the parameters
            byte[] hash = PBKDF2(passwordToHash, salt, PBKDF2_ITERATIONS, HASH_BYTES);
            return PBKDF2_ITERATIONS + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password</returns>
        public byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }

        /// <summary>
        /// Validates a password against the stored, hashed value.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="goodHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public bool ValidatePassword(string password, string goodHash)
        {
            //Extract the parameters from the hash
            char[] delimiter = { ':' };
            string[] split = goodHash.Split(delimiter);
            int iterations = Int32.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return slowEquals(hash, testHash);
        }

        public bool slowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        public int Delete(string username)
        {
            //Return the user who's UserName matches the input username string
            User U = db.Users.Where(x => x.UserName == username).FirstOrDefault();
            //result is our return value. A value of 1 indicates a successful removal while a value of -1 indicates an unsuccessful removal
            int result = 0;
            //If a user exists who's UserName matches the input username string
            if (U != null)
            {
                //Then remove the User
                db.Users.Attach(U);
                db.Users.Remove(U);
                result = 1;
            }
            else
            {
                result = -1;
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
            //Return the result to indicate the outcome of the method
            return result;
        }
    }
}