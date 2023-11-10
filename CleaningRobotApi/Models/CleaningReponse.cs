using System; 

namespace TibberCleaningRobotApi.Models
{
    public class CleaningResponse
    {
// It felt uneccessary to have Id, Timestamp, Command, and Duration in the response. No need.
//        public int Id { get; set; }
//        public DateTime Timestamp { get; set; }
//        public int Commands { get; set; }
        public int UniquePlacesCleaned { get; set; }
//        public double Duration { get; set; }
    }
}
