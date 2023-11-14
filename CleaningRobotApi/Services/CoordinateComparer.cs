using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TibberCleaningRobotApi.Models;

namespace tibber_robot_api.Services
{
    public class CoordinateComparer : IEqualityComparer<Coordinate>
    {
        public bool Equals(Coordinate x, Coordinate y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(Coordinate obj)
        {
            return obj.Y.GetHashCode() ^ obj.X.GetHashCode();
        }
    }
}
