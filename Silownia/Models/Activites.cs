using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Activites
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NumberPlaces { get; set; }

        public int TrainingRoomID { get; set; }
        public string Description { get; set; }
        public Day? Day { get; set; }
        public int Godzina { get; set; }

        public virtual TrainingRoom TrainingRoom { get; set; }

        public virtual ICollection<Activites_Profile> Activites_Profile { get; set; }

    }

    public enum Day
    {
        Poniedziałek, Wtorek, Środa, Czwartek, Piątek, Sobota, Niedziela
    }

    /*public enum Godzina
    {
         Ósmma = 8,
         Dziewiąta = 9,
         Dziesiąta = 10,
         Jedenasta = 11,
         Dwunasta = 12,
         Trzynasta = 13,
         Czternasta = 14,
         Pietnasta = 15,
         Szesnasta = 16,
         Siedemnasta = 17,
         Osiemnasta = 18,
         Dziewietnasta = 19,
         Dwudziesta = 20
    }*/
}