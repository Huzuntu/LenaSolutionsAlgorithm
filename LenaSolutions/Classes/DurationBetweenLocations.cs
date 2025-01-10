using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LenaSolutions.Classes
{

    public class DurationBetweenLocations
    {
        public string From { get; set; }
        public string To { get; set; }
        public int DurationMinutes { get; set; }


        static public List<DurationBetweenLocations> InitializeDurations()
        {
            return new List<DurationBetweenLocations>
            {
                new DurationBetweenLocations { From = "A", To = "B", DurationMinutes = 15 },
                new DurationBetweenLocations { From = "A", To = "C", DurationMinutes = 20 },
                new DurationBetweenLocations { From = "A", To = "D", DurationMinutes = 10 },
                new DurationBetweenLocations { From = "B", To = "C", DurationMinutes = 5 },
                new DurationBetweenLocations { From = "B", To = "D", DurationMinutes = 25 },
                new DurationBetweenLocations { From = "C", To = "D", DurationMinutes = 25 }
            };
        }
    }
}