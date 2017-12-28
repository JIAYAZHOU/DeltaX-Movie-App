using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Mvc;
using Movie.Models;
using System.Data.Entity.Validation;

namespace Movie.Controllers
{
    public class FilmsController : Controller
    {
        
        private MovieEntities2 db = new MovieEntities2();

        // GET: Films
        public ActionResult Index()
        {
            var films = db.Films.Include(f => f.Producer);
            if(Session["Username"]!=null)
            {
                return View(films.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
           
            
        }

        // GET: Films/Details/5
        public ActionResult Details(int? id)
        {


            if (Session["Username"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Film film = db.Films.Find(id);
                if (film == null)
                {
                    return HttpNotFound();
                }
                if (film.Poster != null)
                {
                    ViewBag.ImageToShow = Convert.ToBase64String(film.Poster);
                    
                   // ViewBag.ImageToShow = new string(film.Poster);
                }
                else if (film.Poster == null)
                {
                    ViewBag.ImageToShow = null;
                }
                return View(film);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }




            
        }
        public ActionResult CCreate()
        {


            //   ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "Name");
            //  ViewBag.Actors = new MultiSelectList(db.Actors, "ActorID", "Name");
            // ViewBag.Actors = db.Actors.ToList();
            return View();
        }
        // GET: Films/Create
        public ActionResult Create()
        {

            if (Session["Admin"] != null)
            {
                if ((int)Session["Admin"] == 1)
                {
                    //   ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "Name");
                    //  ViewBag.Actors = new MultiSelectList(db.Actors, "ActorID", "Name");
                    // ViewBag.Actors = db.Actors.ToList();
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

        // POST: Films/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Film film, HttpPostedFileBase post)
        {
            if (((Session["Admin"] != null) && ((int)Session["Admin"] == 1)) || (((Request["Realtime"]) != null) && (Int32.Parse(Request["Realtime"]) == 1)))
            {

                if (ModelState.IsValid)
                {

                    foreach(var item in db.Films.ToList())
                    {
                        if(film.Name.Equals(item.Name))
                        {
                            return RedirectToAction("Index");
                        }
                    }



                    if (Request["Actors"] != null)
                    {
                        string act = Request["Actors"].ToString();
                        string[] Aids = act.Split(',');
                        Models.Actor atemp = new Models.Actor();

                        for (int i = 0; i < Aids.Length; i++)
                        {
                            var aa = (string)(Aids[i]);
                            foreach (var name in db.Actors)
                            {
                                if (aa.Equals(name.Name))
                                {
                                    atemp = name;
                                    break;
                                }
                            }
                            if (atemp.Name != null)
                            {
                                film.Actors.Add(atemp);

                            }
                            else
                            {
                                Models.Actor aatemp = new Models.Actor();
                                aatemp.Name = Aids[i];
                                db.Actors.Add(aatemp);
                                db.SaveChanges();
                                film.Actors.Add(aatemp);

                            }
                        }
                    }









                    if (Request["Producer"] != null)
                    {
                        string prod = Request["Producer"].ToString();

                        Models.Producer ptemp = new Models.Producer();


                        foreach (var name in db.Producers)
                        {
                            if (prod.Equals(name.Name))
                            {
                                ptemp = name;
                                break;
                            }
                        }
                        if (ptemp.Name != null)
                        {
                            film.ProducerID = ptemp.ProducerID;

                        }
                        else
                        {
                            Models.Producer pptemp = new Models.Producer();
                            pptemp.Name = prod;
                            db.Producers.Add(pptemp);
                            db.SaveChanges();
                            film.ProducerID = pptemp.ProducerID;

                        }

                    }











                    if (post != null)
                    {
                        HttpPostedFileBase blah = post;
                        byte[] pic = null;
                        using (var binaryReader = new BinaryReader(blah.InputStream))
                        {
                            pic = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        }
                        film.Poster = pic;
                        //blah.SaveAs("/Uploads/" + blah.FileName);
                        //string po = "/Uploads" + blah.FileName;
                        //film.Poster = po;
                    }


                    if (Request["ActorID"] != null)
                    {
                        string blah = Request["ActorID"].ToString();
                        string[] Aids = blah.Split(',');
                        Models.Actor atemp = new Models.Actor();
                        for (int i = 0; i < Aids.Length; i++)
                        {
                            atemp = db.Actors.Find(Int32.Parse(Aids[i]));
                            film.Actors.Add(atemp);
                        }
                    }


                    db.Films.Add(film);

                    if ((((Request["Realtime"]) != null) && (Int32.Parse(Request["Realtime"]) == 1)))
                    {
                        try
                        {
                            db.SaveChanges();
                            var s = film.MovieID;
                            return Json(new { success = true,id=film.MovieID }, JsonRequestBehavior.AllowGet);

                        }
                        //  catch (DbEntityValidationException ex)
                        catch (Exception ex)
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                            //foreach (var entityValidationErrors in ex.EntityValidationErrors)

                            //{
                            //    foreach (var validationError in entityValidationErrors.ValidationErrors)
                            //    {
                            //        errorList.Add("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            //    }
                            //}
                        }
                    }

                    //db.SaveChanges();
                    //[Bind(Include = "MovieID,Name,Plot,Year_Of_Release,ProducerID,ActorList")]
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "Name", film.ProducerID);
                return View(film);

            }

            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        // GET: Films/Edit/5
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
                    Film film = db.Films.Find(id);
                    if (film == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "Name", film.ProducerID);
                    // ViewBag.Actors = new MultiSelectList(db.Actors, "ActorID", "Name");
                    ViewBag.Actors = db.Actors.ToList();
                    ViewBag.id = id;
                    ViewBag.AList = film.Actors.ToList();
                    var act = new int[999];
                    var i = 0;
                    foreach (var itemm in film.Actors)
                    {
                        act[i] = itemm.ActorID;
                        i = i + 1;
                    }
                    if (film.Poster != null)
                    {
                        ViewBag.ImageToShow = Convert.ToBase64String(film.Poster);
                        //ViewBag.ImageToShow = new string(film.Poster);
                    }
                    else if (film.Poster == null)
                    {
                        ViewBag.ImageToShow = null;
                    }
                    db.SaveChanges();
                    return View(film);
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

        // POST: Films/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Edit([Bind(Include = "MovieID,Name,Plot,Year_Of_Release,ProducerID,Actors")] Film film)
        public ActionResult Edit(Models.Film film, HttpPostedFileBase post)
        {
            if (Session["Admin"] != null)
            {


                if ((int)Session["Admin"] == 1)
                {


                    if (ModelState.IsValid)
                    {
                        //  db.Entry(film).State = EntityState.Modified;
                        //   db.SaveChanges();
                        // return RedirectToAction("Index");
                        //   }                 
                        // db.SaveChanges();
                        if (post != null)
                        {
                            HttpPostedFileBase blah = post;
                            byte[] pic = null;
                            using (var binaryReader = new BinaryReader(blah.InputStream))
                            {
                                pic = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                            }
                            film.Poster = pic;
                            //blah.SaveAs("/Uploads/" + blah.FileName);
                            //string po = "/Uploads" + blah.FileName;
                            //film.Poster = po;

                        }

                        db.Entry(film).State = EntityState.Modified;
                        db.SaveChanges();
                        Film ff = db.Films.Include(f => f.Actors).Single(f => f.MovieID == film.MovieID);
                        //  db.Films.Remove(f);

                        foreach (var actor in ff.Actors.ToList())
                        {
                            ff.Actors.Remove(actor);
                        }

                        if (Request["ActorID"] != null)
                        {
                            string blah = Request["ActorID"].ToString();
                            string[] Aids = blah.Split(',');
                            Models.Actor atemp = new Models.Actor();
                            for (int i = 0; i < Aids.Length; i++)
                            {
                                atemp = db.Actors.Find(Int32.Parse(Aids[i]));
                                ff.Actors.Add(atemp);
                                db.SaveChanges();
                            }
                        }


                        // db.Films.Add(film);
                        //db.SaveChanges();
                        return RedirectToAction("Index");
                    }



                    ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "Name", film.ProducerID);
                    return View(film);
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

        // GET: Films/Delete/5
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
                    Film film = db.Films.Find(id);
                    if (film == null)
                    {
                        return HttpNotFound();
                    }
                    db.Films.Remove(film);
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
            // return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Admin"] != null)
            {

                if ((int)Session["Admin"] == 1)
                {


                    Film film = db.Films.Find(id);
                    db.Films.Remove(film);
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
