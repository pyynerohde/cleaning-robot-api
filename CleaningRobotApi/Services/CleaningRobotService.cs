using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TibberCleaningRobotApi.Models;
using TibberCleaningRobotApi.Data;
using Microsoft.Extensions.Logging;
using tibber_robot_api.Services;

namespace TibberCleaningRobotApi.Services
{
    public class CleaningRobotService : ICleaningRobotService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CleaningRobotService> _logger;

        public CleaningRobotService(AppDbContext dbContext, ILogger<CleaningRobotService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CleaningResponse> ExecuteCleaningRequest(CleaningRequest request)
        {
            var stopwatch = Stopwatch.StartNew(); // Start timing the operation

            var uniquePositions = new HashSet<Coordinate>(new CoordinateComparer());
            var startCoordinate = new Coordinate(request.Start.X, request.Start.Y);
            uniquePositions.Add(startCoordinate);

            var x = request.Start.X;
            var y = request.Start.Y;
      
            foreach (var command in request.Commands)
            {
                ProcessCommand(command, ref x, ref y, ref uniquePositions);
            }
            _logger.LogInformation("done");
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

        private void ProcessCommand(Command command, ref int x, ref int y, ref HashSet<Coordinate> uniquePositions)
        {
            var (dx, dy) = GetMovementVector(command.Direction);
 
            for (long i = 0; i < command.Steps; i++)
            {
                x += dx;
                y += dy;
                uniquePositions.Add(new Coordinate(x, y));
            }
        }

        private (int dx, int dy) GetMovementVector(string direction)
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
