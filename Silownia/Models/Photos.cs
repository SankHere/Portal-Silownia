using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Photos
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public int TrainingRoomID { get; set; }
        public virtual TrainingRoom TrainingRoom { get; set; }
    }
}