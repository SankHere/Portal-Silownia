using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Pass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public float Price { get; set; }
        public string Photo { get; set; }
        public virtual ICollection<Order_Pass> Order_Pass{ get; set; }
        //public virtual Basket Basket { get; set; }

    }
}