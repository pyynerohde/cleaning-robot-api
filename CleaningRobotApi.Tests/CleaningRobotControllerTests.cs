using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TibberCleaningRobotApi.Controllers;
using TibberCleaningRobotApi.Models;
using TibberCleaningRobotApi.Services;
using Xunit;
using Microsoft.Extensions.Logging;
using System;

namespace TibberCleaningRobotApi.Tests
{
    public class CleaningRobotControllerTests
    {
        private readonly Mock<ICleaningRobotService> _mockCleaningRobotService;
        private readonly Mock<ILogger<CleaningRobotController>> _mockLogger;
        private readonly CleaningRobotController _controller;

        public CleaningRobotControllerTests()
        {
            _mockCleaningRobotService = new Mock<ICleaningRobotService>();
            _mockLogger = new Mock<ILogger<CleaningRobotController>>();
            _controller = new CleaningRobotController(_mockCleaningRobotService.Object, _mockLogger.Object);
        }

        [Fact]
        public void Get_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task EnterPath_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var cleaningRequest = new CleaningRequest();
            var cleaningResponse = new CleaningResponse();
            _mockCleaningRobotService.Setup(service => service.ExecuteCleaningRequest(cleaningRequest))
                .ReturnsAsync(cleaningResponse);

            // Act
            var result = await _controller.EnterPath(cleaningRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cleaningResponse, okResult.Value);
        }

        [Fact]
        public async Task EnterPath_WithNullRequest_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.EnterPath(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task EnterPath_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var cleaningRequest = new CleaningRequest();
            _mockCleaningRobotService.Setup(service => service.ExecuteCleaningRequest(cleaningRequest))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.EnterPath(cleaningRequest);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
