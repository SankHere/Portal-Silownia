using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Activites_Profile
    {

        public int ID { get; set; }

        public int ActivitesID { get; set; }

        public virtual Activites Activites { get; set; }

        public int ProfileID { get; set; }

        public virtual Profile Profile{ get; set; }
    }
}