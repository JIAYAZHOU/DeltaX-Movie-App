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
    public class ProducersController : Controller
    {
        private MovieEntities2 db = new MovieEntities2();


        public ActionResult PList()
        {
            return View();
        }

        // GET: Producers
        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {

                return View(db.Producers.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        // GET: Producers/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer producer = db.Producers.Find(id);
                if (producer == null)
                {
                    return HttpNotFound();
                }
                return View(producer);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        // GET: Producers/Create
        public ActionResult Create()
        {
            if (Session["Admin"] != null)
            {


                if ((int)Session["Admin"] == 1)
                {

                    return View();
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
        public ActionResult PPcreate()
        {
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProducerID,Name,Sex,DOB,Bio")] Producer producer)
        {
            if (Session["Admin"] != null)
            {

                if ((int)Session["Admin"] == 1)
                {

                    if (ModelState.IsValid)
                    {
                        db.Producers.Add(producer);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(producer);
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


        //public ActionResult old()
        //{
        //    ViewBag.Pist = db.Producers.ToList();
        //    return PartialView("old");
        //}

        // GET: Producers/pCreate
        public PartialViewResult PCreate()
        {

            ViewBag.Pist = db.Producers.ToList();
            return PartialView();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public PartialViewResult PCreatee(Models.Producer ProducerData)
        {
            if (ModelState.IsValid)
            {
                db.Producers.Add(ProducerData);
                db.SaveChanges();
                ViewBag.Pist = db.Producers.ToList();
                // return PartialView("PList", db.Producers.ToList());
                ViewBag.NewGuy = ProducerData.ProducerID;
                return PartialView("PCreate");
            }
      
            return PartialView();
        }


        






        // GET: Producers/Edit/5
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
                    Producer producer = db.Producers.Find(id);
                    if (producer == null)
                    {
                        return HttpNotFound();
                    }
                    return View(producer);
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

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProducerID,Name,Sex,DOB,Bio")] Producer producer)
        {
            if (Session["Admin"] != null)
            {

                if ((int)Session["Admin"] == 1)
                {

                    if (ModelState.IsValid)
                    {
                        db.Entry(producer).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(producer);
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

        // GET: Producers/Delete/5
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
                    Producer producer = db.Producers.Find(id);
                    if (producer == null)
                    {
                        return HttpNotFound();
                    }

                    db.Producers.Remove(producer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    // return View(producer);
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

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["Admin"] != null)
            {

                if ((int)Session["Admin"] == 1)
                {


                    Producer producer = db.Producers.Find(id);
                    db.Producers.Remove(producer);
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
