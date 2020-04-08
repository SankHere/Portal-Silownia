using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Silownia.ViewModels
{
    public class TimeDataGroup
    {
        [DataType(DataType.Date)]
        public int Time { get; set; }

        public int PassCount { get; set; }
    }
}