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
using System.IO;
using System.Diagnostics;

namespace Silownia.Controllers
{
    
    [Authorize]
    public class BasketController : Controller
    {
        private GymContext db = new GymContext();
        public static List<Pass> listInBasket = new List<Pass>();

        // GET: Basket strona koszyka
        public ViewResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                return View(listInBasket);
            }
            Pass pass = db.Pass.Find(id);

            foreach(var item in listInBasket)
            {
                if(item.ID == pass.ID)
                {
                    return View(listInBasket);
                }
            }
            listInBasket.Add(pass);
            return View(listInBasket);
        }

        //Get: Delete {id} usuwanie z koszyka
        public ViewResult Delete(int? id)
        {
            foreach (var item in listInBasket)
            {
                if(item.ID == id)
                {
                    listInBasket.Remove(item);
                    break;
                }
            }
            
            
           return View("Index", listInBasket);
        }

        //Get: DoOrder składanie zamówień
        public ViewResult DoOrder()
        {
            if (listInBasket.Count == 0)
            {
                return View("Index", listInBasket);
            }
            float priceAll = 0;
            foreach(var item in listInBasket)
            {
                priceAll = priceAll + item.Price;
            }

            //wyszukiwanie zalogowanego użytkownika

            var prof = db.Profiles.Select(n => n).Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            //wyszukanie odpowiedniego statusu

            var stat = db.Status.Select(n => n).Where(n => n.Name == "Oczekuje");

            Status status = null;

            foreach (var item in stat)
            {
                status = item;
            }

            //tworzenie zamowienia
            Order order = new Order { Price = priceAll, Data_Order = DateTime.Now, Profile = profile, Status = status };

            db.Order.Add(order);
            db.SaveChanges();

            //tworzenie informacji jakie produktu należą do zamówienia
            Pass pass = null;
            foreach (var item in listInBasket)
            {
                var passQ = db.Pass.Select(n => n).Where(n => n.ID == item.ID);

                foreach (var itemQ in passQ)
                {
                    pass = itemQ;
                }

                Order_Pass order_pass = new Order_Pass {Pass = pass, Order = order};
                db.Order_Pass.Add(order_pass);
                db.SaveChanges();
            }

            listInBasket.Clear();

            var apikey = "YiaYlpjdIK64lvGaKQMPW52EyK27y4vVuGEg5AOI";

            var address = "https://api.smsapi.pl/sms.do?access_token=";

            //var id1 = zamowienia.Profile_ID1;

            var numer = "48" + profile.NumberPhone;
            //var email = profil.UserName;
            var message = "System przyjal twoje zamowienie, dziekujemy!";

            var url = address + apikey + "&to=" + numer + "&message=" + message + "&sender=" + "&fast=1&format=json";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            Debug.WriteLine(results);
            sr.Close();

            return View("Index", listInBasket);
        }
    }
}