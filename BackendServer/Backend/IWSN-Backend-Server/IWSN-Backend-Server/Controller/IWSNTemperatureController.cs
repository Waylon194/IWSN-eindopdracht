using IWSN_Backend_Server.Model.MongoDB;
using IWSN_Backend_Server.Models.Settings.Class;
using IWSN_Backend_Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Controller
{
    /// <summary>
    /// Controller class of the REST API (via HTTP communication)
    /// </summary>
    [Route(IWSNControllerRouteSettings.IWSNMainRouteName_TMP)]
    [ApiController]
    public class IWSNTemperatureController : ControllerBase
    {
        private readonly MongoDBTemperatureService _TempMeasurementService;

        public IWSNTemperatureController()
        {
            // Assign the service to the class variable
            this._TempMeasurementService = MongoDBTemperatureService.Instance;
        }

        // ROUTE: .../iwsn/latest/single
        // get lastest measurement available based on variable => LATEST_RANGE_ALLOWED defined as private attribute
        [Route("latest/async")]
        [HttpGet]
        public async Task<ActionResult<MongoDBTempModel>> GetLatestSingleMeasurementAsync()
        {
            var measurement = await this._TempMeasurementService.GetLatest();

            if (measurement != null)
            {
                return measurement;
            }
            return NoContent();
        }      
    }
}
