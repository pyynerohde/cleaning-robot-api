using System.Threading.Tasks;
using TibberCleaningRobotApi.Models;

namespace TibberCleaningRobotApi.Services
{
    public interface ICleaningRobotService
    {
        Task<CleaningResponse> ExecuteCleaningRequest(CleaningRequest request);
    }
}
