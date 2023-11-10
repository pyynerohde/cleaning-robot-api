using System.Collections.Generic;

namespace TibberCleaningRobotApi.Models
{
    public class CleaningRequest
    {
        public Point Start { get; set; }
        public List<Command> Commands { get; set; }

        public CleaningRequest()
        {
            Commands = new List<Command>();
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Command
    {
        public string Direction { get; set; }
        public int Steps { get; set; }
    }
}
