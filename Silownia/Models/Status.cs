using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}