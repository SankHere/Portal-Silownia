using Microsoft.AspNet.SignalR;
using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silownia.Controllers
{
    public class DietController : Controller
    {
        private GymContext db = new GymContext();
        // GET: Diet
        public ActionResult Index()
        {
            //wyszukanie zalogowanego użytkownika
            var prof = db.Profiles.Select(n => n).Where(n => n.UserName == User.Identity.Name);

            float calories = 0;
            float protein = 0;
            float carbohydrates = 0;

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            var diet = db.Dish_Profile.Where(a => a.Profile.ID == profile.ID);
            List<Dish_Profile> dishProfile = new List<Dish_Profile>();

            if (diet != null)
            {         
                foreach (var item in diet)
                {
                    dishProfile.Add(item);
                }

                foreach(var item in dishProfile)
                {
                    calories = calories + item.Dish.Calories;
                    protein = protein + item.Dish.Protein;
                    carbohydrates = carbohydrates + item.Dish.Carbohydrates;
                }
            }

            ViewBag.Calories = calories;
            ViewBag.Protein = protein;
            ViewBag.Carbohudrates = carbohydrates;

            return View(dishProfile);
        }


        // GET: Diet/CreateDiet
        public ActionResult CreateDiet()
        {
            return View();
        }

        //POST Diet/GenerateDiet
        [HttpPost]
        public ActionResult GenerateDiet()
        {
            //wyszukanie zalogowanego użytkownika
            var prof = db.Profiles.Select(n => n).Where(n => n.UserName == User.Identity.Name);

            Profile profile = null;

            foreach (var item in prof)
            {
                profile = item;
            }

            //usuwanie starej diety
            var diet = db.Dish_Profile.Where(a => a.Profile.ID == profile.ID);

            if (diet != null)
            {
                foreach (var item in diet)
                {
                    db.Dish_Profile.Remove(item);
                }
                db.SaveChanges();
            }

            //pobranie danych przychodzących formularza
            int growth = int.Parse(Request.Form.Get("growth"));
            int weight = int.Parse(Request.Form.Get("weight"));
            int countDish = int.Parse(Request.Form.Get("dish"));
            int age = int.Parse(Request.Form.Get("age"));
            var gender = Request.Form.Get("gender");
            var dietFor = Request.Form.Get("diet");

            //algorytm na generowanie diety
            float basicCalories = 1250;
            float totalCalories = 0;

            float breakfast = 0;
            float secondBreakfast = 0;
            float dinner = 0;
            float afternoonSnack = 0;
            float supper = 0;

            float limit = 100;
            List<Dish> dishs = new List<Dish>();

            //ustalanie podstawowych kalorii
            if (gender == "M")
            {
                basicCalories = basicCalories + 100;
            } else if (gender == "K")
            {
                basicCalories = basicCalories - 200;
            } else if (age < 17 && age > 60)
            {
                basicCalories = basicCalories - 350;
            } else if (age >= 17 && age <= 60) {
                basicCalories = basicCalories + 200;
            } else if(dietFor == "Masa")
            {
                basicCalories = basicCalories + 250;
            }else if(dietFor == "Odchudzanie")
            {
                basicCalories = basicCalories - 250;
            }

            if(basicCalories < 500)
            {
                basicCalories = 650;
            }

            //całkowite wymagne kalorie
            totalCalories = (basicCalories + weight + age) * 2;

            //podział kalorii na ilość posiłków i wybrór posiłków
            float oneTime = totalCalories / 3;

            if (countDish == 5)
            {
                breakfast = oneTime - 300; 
                supper = oneTime - 400;
                dinner = oneTime + 600;
                secondBreakfast = (totalCalories - breakfast - supper - dinner) / 2;
                afternoonSnack = (totalCalories - breakfast - supper - dinner) / 2;

                dishs.Add(ChoseDish(TimeOfEat.Śniadanie, breakfast, limit));
                dishs.Add(ChoseDish(TimeOfEat.DrugieŚniadanie, secondBreakfast, limit));
                dishs.Add(ChoseDish(TimeOfEat.Obiad, dinner, limit));
                dishs.Add(ChoseDish(TimeOfEat.Podwieczorek, afternoonSnack, limit));
                dishs.Add(ChoseDish(TimeOfEat.Kolacja, supper, limit));
            }
            else if (countDish == 4){
                breakfast = oneTime - 300;
                supper = oneTime - 400;
                dinner = oneTime + 600;
                secondBreakfast = totalCalories - breakfast - supper - dinner;

                dishs.Add(ChoseDish(TimeOfEat.Śniadanie, breakfast, limit));
                dishs.Add(ChoseDish(TimeOfEat.DrugieŚniadanie, secondBreakfast, limit));
                dishs.Add(ChoseDish(TimeOfEat.Obiad, dinner, limit));
                dishs.Add(ChoseDish(TimeOfEat.Kolacja, supper, limit));
            }
            else if (countDish == 3) {
                breakfast = oneTime - 300;
                supper = oneTime - 400;
                dinner = oneTime + 700;

                dishs.Add(ChoseDish(TimeOfEat.Śniadanie, breakfast, limit));
                dishs.Add(ChoseDish(TimeOfEat.Obiad, dinner, limit));
                dishs.Add(ChoseDish(TimeOfEat.Kolacja, supper, limit));
            }
            else if (countDish == 2)
            {
                breakfast = totalCalories / 2;
                dinner = totalCalories / 2;

                dishs.Add(ChoseDish(TimeOfEat.Śniadanie, breakfast, limit));
                dishs.Add(ChoseDish(TimeOfEat.Obiad, dinner, limit));
            }
            else if (countDish == 1)
            {
                dinner = totalCalories;

                dishs.Add(ChoseDish(TimeOfEat.Obiad, dinner, limit));
            }

            List<Dish_Profile> dishProfile = new List<Dish_Profile>();

            foreach (var item in dishs)
            {
                dishProfile.Add(new Dish_Profile { Dish = item, Profile = profile});
            }           
            dishProfile.ForEach(a => db.Dish_Profile.Add(a));
            db.SaveChanges();
            
            return RedirectToAction("Index", "Diet");
        }


        public Dish ChoseDish(TimeOfEat timeOfEat, float calories, float limit)
        {
            Dish selectedDish = null;
            List<Dish> listOfDish = new List<Dish>();
            //pobranie wszystkich dan z naszego przedziału
            var listDish = db.Dish.Select(n => n).Where(n => n.EatingTime == timeOfEat && (n.Calories - calories <= limit && n.Calories - calories >= -limit));

            foreach (var item in listDish)
            {
                listOfDish.Add(item);
            }

            if(listOfDish.Count == 0)
            {
                selectedDish = ChoseDish(timeOfEat, calories, limit + 100);
                return selectedDish;
            }
            //wybór dania do zwrócenia
            Random rnd = new Random();
           
            int randomDish = rnd.Next(0, (listOfDish.Count));

            selectedDish = listOfDish[randomDish];

            return selectedDish;
        }
    }
}