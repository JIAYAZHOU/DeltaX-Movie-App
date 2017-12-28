using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Movie.Models;

namespace Movie.Controllers
{
    public class FavouritesController : Controller
    {
        private MovieEntities db = new MovieEntities();
        

        // GET: Favourites
        public ActionResult Index()
        {
            var favourites = db.Favourites.Include(f => f.Account);
            return View(favourites.ToList());
        }

        // GET: Favourites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favourite favourite = db.Favourites.Find(id);
            if (favourite == null)
            {
                return HttpNotFound();
            }
            return View(favourite);
        }

        // GET: Favourites/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Accounts, "Id", "EmailID");
            return View();
        }

        // POST: Favourites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,MovieID")] */Favourite favourite)
        {
            if (Session["Username"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Favourites.Add(favourite);
                    try
                    {
                        db.SaveChanges();
                        return Json(new { success = true}, JsonRequestBehavior.AllowGet);


                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);

                        
                    }
                   
                  
                }

                ViewBag.Id = new SelectList(db.Accounts, "Id", "EmailID", favourite.Id);
                return View(favourite);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        // GET: Favourites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favourite favourite = db.Favourites.Find(id);
            if (favourite == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Accounts, "Id", "EmailID", favourite.Id);
            return View(favourite);
        }

        // POST: Favourites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MovieID")] Favourite favourite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favourite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Accounts, "Id", "EmailID", favourite.Id);
            return View(favourite);
        }

        // GET: Favourites/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            
            Models.Favourite favourite = new Models.Favourite();
            favourite = db.Favourites.Find(id);
            
            //    foreach(var item in db.Favourites)
            //{
            //        var it = item.MovieID;              
            //        favourite = item;
                
            //}
            if (favourite != null)
            {
                db.Favourites.Remove(favourite);
                db.SaveChanges();
            }

            if (favourite == null)
            {
                return HttpNotFound();
            }
            return View(favourite);
        }

        // POST: Favourites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favourite favourite = db.Favourites.Find(id);
            db.Favourites.Remove(favourite);
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
    }
}
