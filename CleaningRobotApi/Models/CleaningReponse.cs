// Represents the result of the cleaning operation

using System; 

namespace TibberCleaningRobotApi.Models
{
    public class CleaningResponse
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Commands { get; set; }
        public int UniquePlacesCleaned { get; set; }
        public double Duration { get; set; }
    }
}
