using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Silownia.DAL
{
    public class GymInit : DropCreateDatabaseIfModelChanges<GymContext>
    {
    
        protected override void Seed(GymContext context)
        {

            var trainingRoom = new List<TrainingRoom>
            {
                new TrainingRoom { Name = "Sala fitnes" },
                new TrainingRoom { Name = "Sala siłowa" }
            };
            trainingRoom.ForEach(a => context.TrainingRoom.Add(a));
            context.SaveChanges();

            var profile = new List<Profile>
            {
                new Profile { UserName = "user@o2.pl", DayOfPass = 4, Name = "KrzysiekUser", Surname = "Kot", City = "Poznań", NumberPhone = "691160986"},
                new Profile { UserName = "admin@o2.pl", Name = "MarcinAdmin", Surname = "Adminowski", City = "Kraków", NumberPhone = "691160986" },
                new Profile { UserName = "coach@o2.pl", Name = "AdasTrener", Surname = "Bobkowski", City = "Wrocław", NumberPhone = "691160986"}
            };
            profile.ForEach(a => context.Profiles.Add(a));
            context.SaveChanges();

            var activites = new List<Activites>
            {
                new Activites { Name = "Trening fitnes", NumberPlaces = 20, TrainingRoom = trainingRoom[0], Description = "Najlepszy trening dla każdego kto chce stracić troszkę kg.", Day = Day.Poniedziałek, Godzina = 12},
                new Activites { Name = "Trening siłowy", NumberPlaces = 10, TrainingRoom = trainingRoom[1], Description = "Najlepszy trening dla każdego kto chce nabrać troszkę masy", Day = Day.Wtorek, Godzina = 14},
                new Activites { Name = "Wspólne bieganie", NumberPlaces = 10, TrainingRoom = trainingRoom[1], Description = "Najlepszy trening dla każdego kto chce pobiegać w dobrym towarzystwie", Day = Day.Środa, Godzina = 8},
                new Activites { Name = "Trening karate", NumberPlaces = 10, TrainingRoom = trainingRoom[1], Description = "Najlepszy trening dla każdego kto chce nauczyć się karate", Day = Day.Czwartek, Godzina = 18},
                new Activites { Name = "Trening boksu", NumberPlaces = 10, TrainingRoom = trainingRoom[1], Description = "Najlepszy trening dla każdego kto chce nauczyć się boksu", Day = Day.Piątek, Godzina = 16},
                new Activites { Name = "Trening MMA", NumberPlaces = 10, TrainingRoom = trainingRoom[1], Description = "Najlepszy trening dla każdego kto chce nauczyć się MMA", Day = Day.Sobota, Godzina = 10},
                new Activites { Name = "Trening krav maga", NumberPlaces = 10, TrainingRoom = trainingRoom[1], Description = "Najlepszy trening dla każdego kto chce nauczyć się krav maga", Day = Day.Niedziela, Godzina = 18}
            };
            activites.ForEach(a => context.Activites.Add(a));
            context.SaveChanges();

            var activites_Profile = new List<Activites_Profile>
            {
                new Activites_Profile { Activites = activites[0], Profile = profile[0] },
                new Activites_Profile { Activites = activites[1], Profile = profile[0] },
                new Activites_Profile { Activites = activites[0], Profile = profile[2] },
                new Activites_Profile { Activites = activites[1], Profile = profile[2] }
            };
            var pass = new List<Pass>
            {
                new Pass { Name = "Karnet Full", Time =  30, Price = 100, Photo = "karnet1.jpg"},
                new Pass { Name = "Karnet Special", Time =  30, Price = 100, Photo = "karnet3.jpg"},
                new Pass { Name = "Karnet Half", Time =  15, Price = 60, Photo = "karnet4.jpg"},
                new Pass { Name = "hhhhelo Half", Time =  15, Price = 60, Photo = "karnet3.jpg" }
            };
            pass.ForEach(a => context.Pass.Add(a));
            context.SaveChanges();

            var status = new List<Status>
            {
                new Status { Name = "Zaakceptowany" },
                new Status { Name = "Oczekuje" },
                new Status { Name = "Odrzucono" }
            };
            status.ForEach(a => context.Status.Add(a));
            context.SaveChanges();

            var order = new List<Order>
            {
                new Order { Price = 200, Data_Order=new DateTime(2018, 11, 18, 11, 10, 22), Profile = profile[0], Status = status[0]},
                new Order { Price = 150, Data_Order=new DateTime(2018, 12, 26, 13, 44, 15), Profile = profile[0], Status = status[1]},
                new Order { Price = 100, Data_Order=new DateTime(2019, 04, 04, 18, 12, 23), Profile = profile[0], Status = status[2]}

            };
            order.ForEach(a => context.Order.Add(a));
            context.SaveChanges();

            var order_pass = new List<Order_Pass>
            {
                new Order_Pass { Pass = pass[0], Order = order[0]},
                new Order_Pass { Pass = pass[1], Order = order[1]},
                new Order_Pass {  Pass = pass[2], Order = order[2]}
            };
            order_pass.ForEach(a => context.Order_Pass.Add(a));
            context.SaveChanges();

            //base.Seed(context);

           
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));
            roleManager.Create(new IdentityRole("Coach"));

            var user = new ApplicationUser { UserName = "user@o2.pl" };
            string passwordU = "User12.";

            var admin = new ApplicationUser { UserName = "admin@o2.pl" };
            string passwordA = "Admin12.";

            var coach = new ApplicationUser { UserName = "coach@o2.pl" };
            string passwordC = "Coach12.";

            userManager.Create(user, passwordU);
            userManager.AddToRole(user.Id, "User");

            userManager.Create(admin, passwordA);
            userManager.AddToRole(admin.Id, "Admin");

            userManager.Create(coach, passwordC);
            userManager.AddToRole(coach.Id, "Coach");


            var dish = new List<Dish>
            {
                new Dish { Name = "Jajecznica", Calories = 720, Protein = 29, Carbohydrates = 190, EatingTime = TimeOfEat.Śniadanie},
                new Dish { Name = "Płatki owsiane", Calories = 399, Protein = 17, Carbohydrates = 102, EatingTime = TimeOfEat.Śniadanie},
                new Dish { Name = "Rogaliki z nutellą", Calories = 680, Protein = 26, Carbohydrates = 170, EatingTime = TimeOfEat.Śniadanie},
                new Dish { Name = "Płatki kukurydziane", Calories = 540, Protein = 22, Carbohydrates = 164, EatingTime = TimeOfEat.Śniadanie},
                new Dish { Name = "Jabłko", Calories = 52, Protein = 5, Carbohydrates = 22, EatingTime = TimeOfEat.DrugieŚniadanie},
                new Dish { Name = "Banan", Calories = 67, Protein = 11, Carbohydrates = 31, EatingTime = TimeOfEat.DrugieŚniadanie},
                new Dish { Name = "Kurczak z ryżem i sałatką", Calories = 1450, Protein = 56, Carbohydrates = 760, EatingTime = TimeOfEat.Obiad},
                new Dish { Name = "Schabowy z ziemniakami i kapustą", Calories = 1250, Protein = 44, Carbohydrates = 590, EatingTime = TimeOfEat.Obiad},
                new Dish { Name = "Mięso wołowe z ryżem i kapustą", Calories = 1390, Protein = 54, Carbohydrates = 660, EatingTime = TimeOfEat.Obiad},
                new Dish { Name = "Kurczak z kaszą i sosem marchewkowym", Calories = 1445, Protein = 51, Carbohydrates = 620, EatingTime = TimeOfEat.Obiad},
                new Dish { Name = "Gruszka", Calories = 57, Protein = 8, Carbohydrates = 25, EatingTime = TimeOfEat.Podwieczorek},
                new Dish { Name = "Pomarańcza", Calories = 33, Protein = 9, Carbohydrates = 29, EatingTime = TimeOfEat.Podwieczorek},
                new Dish { Name = "Kanapka nutellą", Calories = 511, Protein = 29, Carbohydrates = 277, EatingTime = TimeOfEat.Kolacja},
                new Dish { Name = "Kanapka z szynka i ogórkiem", Calories = 451, Protein = 27, Carbohydrates = 210, EatingTime = TimeOfEat.Kolacja},
                new Dish { Name = "Naleśniki z serem", Calories = 590, Protein = 33, Carbohydrates = 310, EatingTime = TimeOfEat.Kolacja},
                new Dish { Name = "Kanapka z serem", Calories = 598, Protein = 31, Carbohydrates = 290, EatingTime = TimeOfEat.Kolacja}
            };
            dish.ForEach(a => context.Dish.Add(a));
            context.SaveChanges();

            var dish_profile = new List<Dish_Profile>
            {
                new Dish_Profile {Dish = dish[0], Profile = profile[0]},
                new Dish_Profile {Dish = dish[2], Profile = profile[0]},
                new Dish_Profile {Dish = dish[3], Profile = profile[0]},
                new Dish_Profile {Dish = dish[5], Profile = profile[0]},
                new Dish_Profile {Dish = dish[7], Profile = profile[0]},
            };
            dish_profile.ForEach(a => context.Dish_Profile.Add(a));
            context.SaveChanges();
        }
    }
}