using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class TrainingRoom
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NumberRoom { get; set; }
        public virtual ICollection<Activites> Activites { get; set; }
        public virtual ICollection<Photos> Photos { get; set; }

    }
}