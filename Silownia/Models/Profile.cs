using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DayOfPass { get; set; }
        public string City { get; set; }
        public string NumberPhone { get; set; }
       
        public virtual ICollection<Activites_Profile> Activites_Profile { get; set; }
        public virtual ICollection<Order> Order { get; set; }

        public virtual ICollection<Dish_Profile> Dish_Profile { get; set; }

    }
}