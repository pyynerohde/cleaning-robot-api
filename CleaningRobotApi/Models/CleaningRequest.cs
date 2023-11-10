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

// Letting them be type long in case there are crazy large numbers of commands, otherwise they could've been int.
    public class Point
    {
        public long X { get; set; }
        public long Y { get; set; }
    }

    public class Command
    {
        public string Direction { get; set; }
        public long Steps { get; set; }
    }
}
