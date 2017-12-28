using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Movie.Models;
using Microsoft.VisualBasic;


namespace Movie.Controllers
{

    public class AccountsController : Controller
    {
        static int admin;
        private MovieEntities db = new MovieEntities();
        private MovieEntities2 db2 = new MovieEntities2();

        // GET: Accounts
        public ActionResult Index()
         {
           // if(Session["Admin"]!=null)
            return View(db.Accounts.ToList());
          
                
        }
        public ActionResult IsAdmin()
        {
            return View();
        }

        [HttpPost]
        public JsonResult IsAdmin(string passs)
        {
            // var pass = Request["adminpass"].ToString();
            var pass = passs;
            Session["Admin"] = null;
            if (pass=="PASSWORD")
            {
                //Session["Admin"] = 0;
                admin = 1;
            }
            else
            {
                // Session["Admin"] = 1;
                admin = 0;
                return Json(new { verify = false });
            }

            return Json(new { verify = true });
        }



       




        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Account account = db.Accounts.Find(id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                return View(account);
            }
               else
            {
                    return RedirectToAction("Login", "Accounts");
                }

            
        }

        // GET: Accounts/Create
        public ActionResult Register()
        {
            admin = 0;
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account account)
        {
            
            account.IsAdmin = admin;
           // account.Password = Encrypt(account.Password);


            if (ModelState.IsValid)
            {              

                db.Accounts.Add(account);


                try {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                     if (ex is System.Data.SqlClient.SqlException || ex is System.Data.Entity.Infrastructure.DbUpdateException)
                     {
                    ViewBag.ErrorMsg = "The Email/Username already Exists in our database, kindly use another one";
                    return View();
                    }

                   //throw;
                }
                return RedirectToAction("Login","Accounts");
            }

            return View(account);

        }

        


        public string Encrypt(string password)
        {

            //USE APPROPRIATE ENCRYPTION
            int len = password.Length;
            char[] arr = password.ToCharArray();
            char[] raa = new char[50];

            for (int i = 0, j = len - 1; i < len && j >= 0; i++, j--)
            {
                raa[i] = arr[j];
            }
            password = new string(raa);
            password = password.TrimEnd('\0');
            return password;
        }
        public string Decrypt(string password)
        {
            //USE APPROPRIATE DECRYPTION
            int len = password.Length;
            char[] arr = password.ToCharArray();
            char[] raa = new char[50];

            for (int i = 0, j = len - 1; i < len && j >= 0; i++, j--)
            {
                raa[i] = arr[j];
            }
            password = new string(raa);
            password = password.TrimEnd('\0');
            return password;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //   public ActionResult Login([Bind(Include = "ID,Username,Password")] Register register)
        public ActionResult Login(Account account)
        {
            var uname = account.Username;
            var pass = account.Password;
           // pass = Encrypt(pass);
            var r = db.Accounts.ToList();

            foreach (var item in r)
            {
                if (uname.Equals(item.Username))
                {
                    if (pass.Equals(item.Password))
                    {
                     
                        Session["Username"] = item.Username;
                        Session["ID"] = item.Id;
                        Session["Admin"] = item.IsAdmin;
                        return RedirectToAction("Index","Films");
                    }

                }

            }
            ViewBag.Invalid = "Invalid Username or password";
            return View();

        }


        public ActionResult Logout()
        {

            Session["Username"] = null;
            Session["ID"] = null;
            Session["Admin"] = null;
            return RedirectToAction("Index","Home");
        }

        public ActionResult MyProfile()
        {
            if (Session["Username"] != null)
            {
                var id = Session["ID"];
                Account account = db.Accounts.Find(id);
                ViewBag.Email = account.EmailID;
                ViewBag.Username = account.Username;

                var len = Decrypt(account.Password).Length;
                var pw = "";
                for (var i = 1; i <= len; i++)
                    pw = pw + "*";
                ViewBag.Password = pw;
                ViewBag.Favs = account.Favourites.ToList();


                ///////////////////////////////////////////////////////
                string[] names = new string[account.Favourites.Count +1];
                int k = -1;
                foreach (var item1 in account.Favourites)
                {
                    foreach (var item in db2.Films)
                    {
                        if (item1.MovieID == item.MovieID)
                        {
                            names[++k] = item.Name;
                            break;
                        }
                    }
                }
                ViewBag.Movies = names.ToList();
           ////////////////////////////////////////////////////////     
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }




        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Account account = db.Accounts.Find(id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                return View(account);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }

        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmailID,Username,Password,ConfirmPassword")] Account account)
        {
            if (Session["Username"] != null)
            {

                if (ModelState.IsValid)
                {
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyProfile");
                }
                return View(account);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }

        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Username"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Account account = db.Accounts.Find(id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Accounts.Remove(account);
                    db.SaveChanges();
                    Session["Username"] = null;
                    Session["ID"] = null;
                    Session["Admin"] = null;
                    return RedirectToAction("Index","Home");
                }
              
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }

        }

        // POST: Accounts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{

        //    if (Session["Username"] != null)
        //    {

        //        Account account = db.Accounts.Find(id);
        //        db.Accounts.Remove(account);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Accounts");
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
