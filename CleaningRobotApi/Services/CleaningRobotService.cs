//  Implement the interface; use a HashSet to track unique positions.

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

            var uniquePositions = new HashSet<Position>();
            var currentPosition = new Position { X = request.Start.X, Y = request.Start.Y };
            uniquePositions.Add(currentPosition); // Add the starting position

            foreach (var command in request.Commands)
            {
                ProcessCommand(command, currentPosition, uniquePositions);
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

        private void ProcessCommand(Command command, Position currentPosition, HashSet<Position> uniquePositions)
        {
            // Determine the movement vector based on the direction
            var (dx, dy) = GetMovementVector(command.Direction);

            // Move the robot and track the positions
            for (int i = 0; i < command.Steps; i++)
            {
                currentPosition.X += dx;
                currentPosition.Y += dy;
                uniquePositions.Add(new Position(currentPosition.X, currentPosition.Y));
            }
        }

        private (int dx, int dy) GetMovementVector(string direction)
        {
            // Convert direction to lower case to ensure case-insensitive comparison
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

