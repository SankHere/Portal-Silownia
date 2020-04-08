using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Order_Pass
    {
        public int ID { get; set; }

        //public int Quantity { get; set; }
        //public double Price { get; set; }

        public int PassID { get; set; }

        public virtual Pass Pass { get; set; }

        public int OrderID { get; set; }

        public virtual Order Order { get; set; }
       
    }
}