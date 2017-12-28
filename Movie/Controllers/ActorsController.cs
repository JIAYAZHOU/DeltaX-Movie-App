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
    public class ActorsController : Controller
    {
        private MovieEntities2 db = new MovieEntities2();

        // GET: Actors
        public ActionResult Index()
        {

            if (Session["Username"] != null)
            {
                return View(db.Actors.ToList());
            }
            else
                return RedirectToAction("Login", "Accounts");
            
        }
        //public ActionResult old()
        //{
        //    ViewBag.Aist = db.Actors.ToList();
        //    ViewBag.Actors = new MultiSelectList(db.Actors, "ActorID", "Name");
        //    return PartialView("old");
        //}




        // GET: actors/aCreate
        public PartialViewResult ACreate()
        {
            ViewBag.Actors = db.Actors.ToList();
            ViewBag.Aist = db.Actors.ToList();
            return PartialView();
        }

       //post
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public PartialViewResult ACreatee(Models.Actor ActorData)
        {
            if (ModelState.IsValid)
            {
                db.Actors.Add(ActorData);
                db.SaveChanges();
                ViewBag.Aist = db.Actors.ToList();
                //   ViewBag.Actors = new MultiSelectList(db.Actors, "ActorID", "Name");
                ViewBag.Actors = db.Actors.ToList();
                // return PartialView("PList", db.Producers.ToList());
                return PartialView("ACreate");
            }

            return PartialView();
        }





        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Actor actor = db.Actors.Find(id);
                if (actor == null)
                {
                    return HttpNotFound();
                }
                return View(actor);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            if (Session["Admin"] != null)
            {
                if ((int)Session["Admin"] == 1)
                {
                    return View();
                }
                else
                    return RedirectToAction("Login", "Accounts");
            }
            else
                return RedirectToAction("Login", "Accounts");
        }
        public ActionResult AAcreate()
        {
            return View();
        }


        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActorID,Name,Sex,DOB,Bio")] Actor actor)
        {
            if (Session["Admin"] != null)
            {
                if ((int)Session["Admin"] == 1)
                {
                    if (ModelState.IsValid)
                    {
                        db.Actors.Add(actor);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Accounts");
                    }

                }
            }
            
           
                return RedirectToAction("Login", "Accounts");
            
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] != null)
            {
                if ((int)Session["Admin"] == 1)
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Actor actor = db.Actors.Find(id);
                    if (actor == null)
                    {
                        return HttpNotFound();
                    }
                    return View(actor);
                }
                else
                    return RedirectToAction("Login", "Accounts");
            }
            else
                return RedirectToAction("Login", "Accounts");

        }

        

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActorID,Name,Sex,DOB,Bio")] Actor actor)
        {
            if (Session["Admin"] != null)
            {

                if ((int)Session["Admin"] == 1)
                {

                    if (ModelState.IsValid)
                    {
                        db.Entry(actor).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(actor);

                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] != null)
            {


                if ((int)Session["Admin"] == 1)
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Actor actor = db.Actors.Find(id);


                    if (actor == null)
                    {
                        return HttpNotFound();
                    }

                    db.Actors.Remove(actor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }

            //  return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Admin"]!=null)
            {

                if ((int)Session["Admin"] == 1)
                {

                    Actor actor = db.Actors.Find(id);
                    db.Actors.Remove(actor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }

            }

            else
            {
                return RedirectToAction("Login", "Accounts");
            }


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
