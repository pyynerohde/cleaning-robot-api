using System.Collections.Generic;

namespace TibberCleaningRobotApi.Models
{
    public class CleaningRequest
    {
        public Point StartingPoint { get; set; }
        public List<Command> Commands { get; set; }

        public CleaningRequest()
        {
            Commands = new List<Command>();
        }
}
}
