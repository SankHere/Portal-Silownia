using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Dish_Profile
    {
        public int ID { get; set; }
        public int DishID { get; set; }

        public virtual Dish Dish { get; set; }

        public int ProfileID { get; set; }

        public virtual Profile Profile { get; set; }
    }
}