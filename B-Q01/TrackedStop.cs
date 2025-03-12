using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Q01
{
    public class TrackedStop
    {
        public int StopId { get; set; }
        public string StopName { get; set; }
        public string KafkaTopic
        {
            get
            {
                return $"Departures-{StopName}";
            }
        }
    }
}
