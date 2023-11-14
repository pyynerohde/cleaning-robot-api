using Microsoft.AspNetCore.Mvc;
using TibberCleaningRobotApi.Models;
using TibberCleaningRobotApi.Services;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace TibberCleaningRobotApi.Controllers
{
    [ApiController]
    [Route("tibber-developer-test")]
    public class CleaningRobotController : ControllerBase
    {
        private readonly ICleaningRobotService _cleaningRobotService;
        private readonly ILogger<CleaningRobotController> _logger;

        // Business logic is implemented in ICleaningRobotService
        public CleaningRobotController(ICleaningRobotService cleaningRobotService, ILogger<CleaningRobotController> logger)
        {
            _cleaningRobotService = cleaningRobotService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to the Tibber Cleaning Robot API");
        }

        [HttpPost]
        [Route("enter-path")]
        public async Task<IActionResult> EnterPath([FromBody] CleaningRequest cleaningRequest)
        {
            // No need for input validation but it's good practice to check for nulls
            _logger.LogInformation("Cleaning request received");
            if (cleaningRequest == null)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var response = await _cleaningRobotService.ExecuteCleaningRequest(cleaningRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, ex.Message);
            }
        }
    }
}
