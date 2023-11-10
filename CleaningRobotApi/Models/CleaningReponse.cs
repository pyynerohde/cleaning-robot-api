using System; 

namespace TibberCleaningRobotApi.Models
{
    public class CleaningResponse
    {
        // Is it uneccessary to have Id, Timestamp, Command, and Duration? 
        // We don't return anything else than UniquePlacesCleaned in the response atm
//        public int Id { get; set; }
//        public DateTime Timestamp { get; set; }
//        public int Commands { get; set; }
        public int UniquePlacesCleaned { get; set; }
//        public double Duration { get; set; }
    }
}
