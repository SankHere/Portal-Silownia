using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Order
    {
        public int ID { get; set; }
       
        public float Price { get; set; }
        public DateTime Data_Order { get; set; }
        //powiazanie z uzytkownikiem
        public int ProfileID { get; set; }
        public virtual Profile Profile { get; set; }

        public int StatusID { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Order_Pass> Order_Pass { get; set; }
    }
}