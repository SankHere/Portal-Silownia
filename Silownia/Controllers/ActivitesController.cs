using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.DAL;
using Silownia.Models;

namespace Silownia.Controllers
{
    [Authorize]
    public class ActivitesController : Controller
    {
        private GymContext db = new GymContext();

        // GET: Activites
        public ActionResult Index()
        {
            var activites = db.Activites.Include(a => a.TrainingRoom);
            return View(activites.ToList());
        }

        // GET: Activites/Details/5
        public ActionResult Details(int? id)
        {
            var isSave = true;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Activites activites = db.Activites.Find(id);

            if (activites == null)
            {
                return HttpNotFound();
            }

            //pobranie aktualnie zalogowanego użytkownika
            var prof = db.Profiles.Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            //wyszukiwanie czy jest już uzytkownik zapisany na zajecia
            var activitesProfile = db.Activites_Profile.Select(m => m).Where(m => m.ActivitesID == id && m.Profile.ID == profile.ID);

            Activites_Profile activitesProf = null;

            foreach (var item in activitesProfile)
            {
                activitesProf = item;
            }

            if (activitesProf == null)
            {
                isSave = false;
            }

            ViewBag.IsSave = isSave;
            ViewBag.Slot = activites.NumberPlaces;

            return View(activites);
        }
       
        //Get Activites/SaveActivity/3
        //zapisywanie sie na zajęcia
        public ActionResult SaveActivity(int id)
        {
            //wyszukanie aktywnsci na która chce sie zapisac
            var acti = db.Activites.Where(n => n.ID == id);

            Activites activites = null;

            foreach (var item in acti)
            {
                activites = item;
            }
           
            //pobranie aktualnie zalogowanego użytkownika
            var prof = db.Profiles.Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;
                
            foreach (var item in prof)
            {
                profile = item;
            }
            activites.NumberPlaces = activites.NumberPlaces - 1;
            Activites_Profile activiteProfile = new Activites_Profile { Profile = profile, ActivitesID = id };
            db.Activites_Profile.Add(activiteProfile);
            db.SaveChanges();
            
            return RedirectToAction("Index", "Activites");
        }
        // Get Activites/UnSaveActivity/3
        public ActionResult UnSaveActivity(int id)
        {
            //wyszukanie aktywnsci na która chce sie wypisać
            var acti = db.Activites.Where(n => n.ID == id);

            Activites activites = null;

            foreach (var item in acti)
            {
                activites = item;
            }

            //pobranie aktualnie zalogowanego użytkownika
            var prof = db.Profiles.Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            //Wyszukanie zajecia i usuniecie go z bazy dla danego użytkownika
            var activitesProfile = db.Activites_Profile.Select(m => m).Where(m => m.Activites.ID == id && m.Profile.ID == profile.ID);

            Activites_Profile activitesProf = null;

            foreach (var item in activitesProfile)
            {               
                    activitesProf = item;             
            }

            activites.NumberPlaces = activites.NumberPlaces + 1;
            db.Activites_Profile.Remove(activitesProf);
            db.SaveChanges();

            return RedirectToAction("Index", "Activites");
        }

        // Get: Activites/MyActivites
        public ActionResult MyActivites()
        {
            //pobranie aktualnie zalogowanego użytkownika
            var prof = db.Profiles.Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            //Wyszukanie zajęć dla zalogowanego użytkownika 
            var activitesProfile = db.Activites_Profile.Select(m => m).Where(m => m.Profile.ID == profile.ID);

            List<Activites_Profile> actProf = new List<Activites_Profile>();

            foreach (var item in activitesProfile)
            {
                actProf.Add(item);
            }

            return View(actProf);
        }

        // GET: Activites/Create
        [Authorize(Roles = "Coach")]
        public ActionResult Create()
        {
            ViewBag.TrainingRoomID = new SelectList(db.TrainingRoom, "ID", "Name");
            return View();
        }
        // POST: Activites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coach")]
        public ActionResult Create([Bind(Include = "ID,Name,NumberPlaces,TrainingRoomID,Description,Day,Godzina")] Activites activites)
        {
            if (ModelState.IsValid)
            {
              
                var prof = db.Profiles.Where(n => n.UserName == User.Identity.Name);

                Profile profile = null;

                foreach (var item in prof)
                {
                    profile = item;
                }

                Activites_Profile ac_prof = new Activites_Profile { Profile = profile, Activites = activites };
                db.Activites_Profile.Add(ac_prof);

                db.Activites.Add(activites);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //chyba nie potrzebne 
            ViewBag.TrainingRoomID = new SelectList(db.TrainingRoom, "ID", "Name", activites.TrainingRoomID);
            return View(activites);
        }

        // GET: Activites/Edit/5
        [Authorize(Roles = "Coach")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activites activites = db.Activites.Find(id);
            if (activites == null)
            {
                return HttpNotFound();
            }
            ViewBag.TrainingRoomID = new SelectList(db.TrainingRoom, "ID", "Name", activites.TrainingRoomID);
            return View(activites);
        }

        // POST: Activites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,NumberPlaces,TrainingRoomID,Description,Day,Godzina")] Activites activites)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activites).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TrainingRoomID = new SelectList(db.TrainingRoom, "ID", "Name", activites.TrainingRoomID);
            return View(activites);
        }

        // GET: Activites/Delete/5
        [Authorize(Roles = "Coach")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activites activites = db.Activites.Find(id);
            if (activites == null)
            {
                return HttpNotFound();
            }
            //Wyszukanie zajecia i usuniecie go z bazy Activites_Profiles
            var activitesProfile = db.Activites_Profile.Where(m => m.Activites.ID == activites.ID);

            List<Activites_Profile> actProf = new List<Activites_Profile>();
           
            foreach (var item in activitesProfile)
            {
                actProf.Add(item);
            }
            
            foreach (var item in actProf)
            {
                db.Activites_Profile.Remove(item);
            }
            db.Activites.Remove(activites);

            db.SaveChanges();
            return RedirectToAction("MyActivites");
        }


        //Get: Activites/DetailsUser/1
        public ActionResult DetailsUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //pobranie aktualnie zalogowanego użytkownika
            var prof = db.Profiles.Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            // Wyszukanie zajecia z tym id i bez treniera
            var activitesProfile = db.Activites_Profile.Where(m => m.Activites.ID == id && m.Profile.ID != profile.ID);

            List<Activites_Profile> actProf = new List<Activites_Profile>();

            foreach (var item in activitesProfile)
            {
                actProf.Add(item);
            }

            return View(actProf);
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
