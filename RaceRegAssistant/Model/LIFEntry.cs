using System;
using System.Collections.Generic;

namespace RaceRegAssistant.Model
{
    /// <summary>
    /// LIFEntry class containg the data points in which an entry in a LIF file contains. 
    /// See: http://www.finishlynx.com/2017/01/file-formats-meet-manager/
    /// Some entry points are NOT supported.
    /// </summary>
    public class LIFEntry
    {
        public int Place { get; set; }
        public int RacerId { get; set; }
        public int Lane { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Affiliation { get; set; }
        public DateTime Time { get; set; }
        public string License { get; set; }
        public DateTime DeltaTime { get; set; }
        public DateTime ReacTime { get; set; }
        public List<Split> splits { get; set; }
        //Time Trial Start Time
        //User 1
        //User 2
        //User 3
    }
}
