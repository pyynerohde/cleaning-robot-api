using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TibberCleaningRobotApi.Models;
using TibberCleaningRobotApi.Data;

namespace TibberCleaningRobotApi.Services
{
    public class CleaningRobotService : ICleaningRobotService
    {
        private readonly AppDbContext _dbContext;

        public CleaningRobotService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CleaningResponse> ExecuteCleaningRequest(CleaningRequest request)
        {
            var stopwatch = Stopwatch.StartNew(); // Start timing the operation

            var uniquePositions = new HashSet<long>();
            var currentHash = ComputePositionHash(request.Start.X, request.Start.Y);
            uniquePositions.Add(currentHash);

            var x = request.Start.X;
            var y = request.Start.Y;

            foreach (var command in request.Commands)
            {
                ProcessCommand(command, ref x, ref y, ref uniquePositions);
            }

            stopwatch.Stop(); // Stop timing the operation

            var result = new CleaningResponse
            {
                UniquePlacesCleaned = uniquePositions.Count,
                // Add other properties to the result as necessary
            };

            // Store the result in the database
            var execution = new Execution
            {
                Timestamp = DateTime.UtcNow,
                Commands = request.Commands.Count,
                Result = uniquePositions.Count,
                Duration = stopwatch.Elapsed.TotalSeconds // Duration in seconds
            };

            _dbContext.Executions.Add(execution);
            await _dbContext.SaveChangesAsync();

            return result;
        }

        private void ProcessCommand(Command command, ref long x, ref long y, ref HashSet<long> uniquePositions)
        {
            // Determine the movement vector based on the direction
            var (dx, dy) = GetMovementVector(command.Direction);

            // Move the robot and track the positions
            for (long i = 0; i < command.Steps; i++)
            {
                x += dx;
                y += dy;
                var positionHash = ComputePositionHash(x, y);
                uniquePositions.Add(positionHash);
            }
        }

        private long ComputePositionHash(long x, long y)
        {
            /*
            The approach to combine x and y into a unique hash code is simple and does not require you to 
            store the positions in a grid. We only use the HashSet to track the unique positions, and this 
            way we don't need to define what "unique" is in the sense of overriding the Equals and GetHashCode
            methods to check for 'value equality' instead of 'reference equality'. 
    
            However, this approach could in theory lead to hash collisions for very large grids. 
            */
            return ((long)x << 32) | (long)y;
        }

        private (long dx, long dy) GetMovementVector(string direction)
        {
            direction = direction.ToLowerInvariant();
            return direction switch
            {
                "north" => (0, 1),
                "east" => (1, 0),
                "south" => (0, -1),
                "west" => (-1, 0),
                _ => throw new ArgumentException("Invalid direction")
            };
        }
    }
}
