using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using TibberCleaningRobotApi.Data;
using TibberCleaningRobotApi.Models;
using TibberCleaningRobotApi.Services;
using Xunit;
using Microsoft.Extensions.Logging;

namespace TibberCleaningRobotApi.Tests
{
    public class CleaningRobotServiceTests
    {
        private readonly Mock<ILogger<CleaningRobotService>> _mockLogger;
        private readonly CleaningRobotService _service;
        private readonly AppDbContext _dbContext;

        public CleaningRobotServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDbForTesting")
                .Options;
            _dbContext = new AppDbContext(options);

            _mockLogger = new Mock<ILogger<CleaningRobotService>>();
            _service = new CleaningRobotService(_dbContext, _mockLogger.Object);
        }

        [Fact]
        public async Task ExecuteCleaningRequest_WithPositiveStartCoordinates_ReturnsCorrectResponse()
        {
            // Arrange
            var request = new CleaningRequest
            {
                Start = new Point { X = 10, Y = 22 },
                Commands = new System.Collections.Generic.List<Command>
                {
                    new Command { Direction = "east", Steps = 2 },
                    new Command { Direction = "north", Steps = 1 }
                }
            };

            // Act
            var response = await _service.ExecuteCleaningRequest(request);

            // Assert
            Assert.Equal(4, response.UniquePlacesCleaned); // 4 unique places should be cleaned
        }

        [Fact]
        public async Task ExecuteCleaningRequest_WithOneNegativeStartCoordinate_ReturnsCorrectResponse()
        {
            // Arrange
            var request = new CleaningRequest
            {
                Start = new Point { X = -10, Y = 22 },
                Commands = new System.Collections.Generic.List<Command>
                {
                    new Command { Direction = "east", Steps = 2 },
                    new Command { Direction = "north", Steps = 1 }
                }
            };

            // Act
            var response = await _service.ExecuteCleaningRequest(request);

            // Assert
            Assert.Equal(4, response.UniquePlacesCleaned); // 4 unique places should be cleaned
        }

        [Fact]
        public async Task ExecuteCleaningRequest_WithTwoNegativeStartCoordinates_ReturnsCorrectResponse()
        {
            // Arrange
            var request = new CleaningRequest
            {
                Start = new Point { X = -10, Y = -22 },
                Commands = new System.Collections.Generic.List<Command>
                {
                    new Command { Direction = "east", Steps = 2 },
                    new Command { Direction = "north", Steps = 1 }
                }
            };

            // Act
            var response = await _service.ExecuteCleaningRequest(request);

            // Assert
            Assert.Equal(4, response.UniquePlacesCleaned); // 4 unique places should be cleaned
        }

        [Fact]
        public async Task ExecuteCleaningRequest_WithOverlappingPaths_ReturnsCorrectResponse()
        {
            // Arrange
            var request = new CleaningRequest
            {
                Start = new Point { X = 0, Y = 0 },
                Commands = new System.Collections.Generic.List<Command>
                {
                    new Command { Direction = "east", Steps = 5 },
                    new Command { Direction = "west", Steps = 5 }
                }
            };

            // Act
            var response = await _service.ExecuteCleaningRequest(request);

            // Assert
            Assert.Equal(6, response.UniquePlacesCleaned); // 6 unique places should be cleaned
        }
    }
}