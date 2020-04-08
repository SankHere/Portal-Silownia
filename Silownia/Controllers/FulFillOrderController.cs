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
using Rotativa;

namespace Silownia.Controllers
{
    public class FulFillOrderController : Controller
    {
        private GymContext db = new GymContext();
        
        // GET: FulFillOrder
        public ActionResult Index()
        {
             
            var orders = db.Order.Select(n => n);
            List<Order> listOfOrders = new List<Order>();
            foreach (var item in orders)
            {
                listOfOrders.Add(item);
            }

            return View(listOfOrders);
        }
        // Get: zmien status zamówienia - akceptuj -------------------------do poprawy
        public ActionResult Accept(int? id)
        {   
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //wyszkunaie odpowiedniego statusu       
            var status = db.Status.Find(1);
            //wyszukanie odpowieniego zamówienia
            var order = db.Order.Find(id);
            //wyszukanie order_pass dla naszego zamówienia 
            var order_Pass = db.Order_Pass.Where(n => n.Order.ID == id);

            List<Order_Pass> listOfOrder_Pass = new List<Order_Pass>();
            foreach (var item in order_Pass)
            {
                listOfOrder_Pass.Add(item);
            }

            int daysPass = order.Profile.DayOfPass;
            
            foreach (var item in listOfOrder_Pass)
            {
                daysPass = daysPass + item.Pass.Time;
            }

            order.Profile.DayOfPass = daysPass;

            order.Status = status;

            db.SaveChanges();

            return RedirectToAction("Index", "FulFillOrder");
        }
        // Get: zmien status zamówienia - oczekuj
        public ActionResult Wait(int id)
        {
            var status = db.Status.Find(2);

            var order = db.Order.Find(id);

            order.Status = status;
            db.SaveChanges();

            return RedirectToAction("Index", "FulFillOrder");
        }
        // Get: zmien status zamówienia - odrzuć 
        public ActionResult Discard(int id)
        {
            var status = db.Status.Find(3);

            var order = db.Order.Find(id);

            order.Status = status;
            db.SaveChanges();

            return RedirectToAction("Index", "FulFillOrder");
        }
        //Get szczególy
        public ActionResult Details(int id)
        {
            var order = db.Order.Find(id);

            var order_Pass = db.Order_Pass.Where(n => n.OrderID == order.ID);

            List<Order_Pass> order_Passes = new List<Order_Pass>();

            foreach (var item in order_Pass)
            {
                order_Passes.Add(item);
            }

            ViewBag.NameUser = order.Profile.UserName;
            ViewBag.DaysOfPass = order.Profile.DayOfPass;
            ViewBag.Status = order.Status.Name;
            ViewBag.OrderId = order.ID;

            return View("Details", order_Passes);
        }

        //Uzytkownika zamówienia
        public ActionResult UserOrder()
        {
            //wyszukanie zalogowanego użytkownika
            var prof = db.Profiles.Select(n => n).Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            //wyszukanie wszystkich zamowieni
            var orders = db.Order.Where(n => n.Profile.ID == profile.ID);

            List<Order> listOfOrders = new List<Order>();
            foreach (var item in orders)
            {
                listOfOrders.Add(item);
            }

            
            return View(listOfOrders);
        }
        //widok dla pdf-a 
        public ActionResult Pdf(int id)
        {
            var order = db.Order.Find(id);

            var order_Pass = db.Order_Pass.Where(n => n.OrderID == order.ID);

            List<Order_Pass> order_Passes = new List<Order_Pass>();

            foreach (var item in order_Pass)
            {
                order_Passes.Add(item);
            }

            ViewBag.NameUser = order.Profile.UserName;
            ViewBag.DaysOfPass = order.Profile.DayOfPass;
            ViewBag.Status = order.Status.Name;
            ViewBag.OrderId = order.ID;

            return View("Pdf", order_Passes);
        }
        //Drukowanie pdf-a
        public ActionResult DoPDF(int id)
        {
           var result = new ActionAsPdf("Pdf", new { id = id });
            return result;
        }
    }
}