using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silownia.ViewModels;
using Silownia.DAL;
using Silownia.Models;

namespace Silownia.Controllers
{
    public class HomeController : Controller
    {
        private GymContext db = new GymContext();
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Chat()
        {
            //wyszukanie zalogowanego użytkownika
            var prof = db.Profiles.Select(n => n).Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            ViewBag.Name = profile.Name;
            return View();
        }

        public ActionResult About()
        {
           
            IQueryable<TimeDataGroup> data = from pass in db.Pass
                                                   group pass by pass.Time into timeGroup
                                                   select new TimeDataGroup()
                                                   {
                                                       Time = timeGroup.Key,
                                                       PassCount = timeGroup.Count()
                                                   };
            return View(data.ToList());
        
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}