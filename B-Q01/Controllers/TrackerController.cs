using B_Q01.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace B_Q01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackerController : ControllerBase
    {
        private readonly ILiteDbDeparturesService departuresService;
        public TrackerController(ILiteDbDeparturesService departuresService) 
        {
            this.departuresService = departuresService;
        }

        [HttpPost]
        public async Task<string> TrackStationTopic(string stopName, int stopId)
        {
            var stop = departuresService.AddTrackedStop(stopName, stopId);

            return stop.KafkaTopic;
        }
    }
}
