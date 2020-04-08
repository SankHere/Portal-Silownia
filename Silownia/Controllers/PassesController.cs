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
using PagedList;

namespace Silownia.Controllers
{
    [Authorize]
    public class PassesController : Controller
    {
        private GymContext db = new GymContext();
        
        // GET: Passes
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.TimeSortParm = sortOrder == "Time" ? "time_desc" : "Time"; ;
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price"; ;
            var pass = from s in db.Pass select s;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                pass = pass.Where(s => s.Name.Contains(searchString));
                    
            }

            switch (sortOrder)
            {
                case "Name":
                    pass = pass.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    pass = pass.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    pass = pass.OrderByDescending(s => s.Price);
                    break;
                case "Time":
                    pass = pass.OrderBy(s => s.Time);
                    break;
                case "time_desc":
                    pass = pass.OrderByDescending(s => s.Time);
                    break;
                default:
                    pass = pass.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(pass.ToPagedList(pageNumber, pageSize));
        }

        // GET: Passes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pass pass = db.Pass.Find(id);
            if (pass == null)
            {
                return HttpNotFound();
            }
            return View(pass);
        }

        // GET: Passes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Passes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Name,Time,Price,Photo")] Pass pass)
        {

            HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
            if(file != null && file.ContentLength > 0)
            {
                pass.Photo = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Photo/") + pass.Photo);
            }

            if (ModelState.IsValid)
            {
                db.Pass.Add(pass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pass);
        }

        // GET: Passes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pass pass = db.Pass.Find(id);
            string zdjecie = pass.Photo;
            if (pass == null)
            {
                return HttpNotFound();
            }
            return View(pass);
        }

        // POST: Passes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,Name,Time,Price,Photo")] Pass pass)
        {
           
            HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
            
            if (file != null && file.ContentLength > 0)
            {
                pass.Photo = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Photo/") + pass.Photo);
            }

            if (ModelState.IsValid)
            {
                db.Entry(pass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pass);
        }

        // GET: Passes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pass pass = db.Pass.Find(id);
            if (pass == null)
            {
                return HttpNotFound();
            }
            return View(pass);
        }

        // POST: Passes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Pass pass = db.Pass.Find(id);
            db.Pass.Remove(pass);
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
