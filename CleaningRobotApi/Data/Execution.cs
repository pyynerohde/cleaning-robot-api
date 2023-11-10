using System;
using System.ComponentModel.DataAnnotations;

namespace TibberCleaningRobotApi.Data
{
    public class Execution
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Commands { get; set; }
        public int Result { get; set; }
        public double Duration { get; set; }
    }
}
