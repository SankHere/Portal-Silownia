using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Dish
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public float Calories { get; set; }
        public float Protein { get; set; }
        public float Carbohydrates { get; set; }
        public TimeOfEat EatingTime { get; set; }

        public virtual ICollection<Dish_Profile> Dish_Profile { get; set; }
    }

    public enum TimeOfEat
    {
        Śniadanie,
        DrugieŚniadanie,
        Obiad,
        Podwieczorek,
        Kolacja
    }
}