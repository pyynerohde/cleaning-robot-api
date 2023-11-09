// Define a POST method EnterPath that accepts a JSON body with the structure provided in the problem description.

using Microsoft.AspNetCore.Mvc;
using TibberCleaningRobotApi.Models;
using TibberCleaningRobotApi.Services;
using System.Threading.Tasks;

namespace TibberCleaningRobotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CleaningRobotController : ControllerBase
    {
        private readonly ICleaningRobotService _cleaningRobotService;

        // Business logic is implemented in ICleaningRobotService
        public CleaningRobotController(ICleaningRobotService cleaningRobotService)
        {
            _cleaningRobotService = cleaningRobotService;
        }


        [HttpGet]
        public IAction

        [HttpPost]
        [Route("/tibber-developer-test/enter-path")]
        public async Task<IActionResult> EnterPath([FromBody] CleaningRequest cleaningRequest)
        {
            // No need for input validation but it's good practice to check for nulls
            if (cleaningRequest == null)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var response = await _cleaningRobotService.ExecuteCleaningRequest(cleaningRequest);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, ex.Message);
            }
        }
    }
}
