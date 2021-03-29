using IWSN_Backend_Server.Model.Sensor;
using IWSN_Backend_Server.Models.Database;
using IWSN_Backend_Server.Models.Settings.Class;
using IWSN_Backend_Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Controllers.ControllerInstances
{
    /// <summary>
    /// Controller class of the REST API (via HTTP communication)
    /// </summary>
    [Route(IWSNControllerRouteSettings.IWSNMainRouteName)]
    [ApiController]
    public class IWSNController : ControllerBase
    {
        private readonly MongoDBService _SensorMeasurementService;
        private readonly int MAX_LATEST_RANGE_ALLOWED = 20;

        public IWSNController()
        {
            // Assign the service to the class variable
            this._SensorMeasurementService = MongoDBService.Instance;
        }

        // ROUTE: .../iwsn/all
        // get all the available users - async
        [Route("all/async")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MongoDBDatagramModel>>> GetAllMeasurements()
        {
            var measurements = await this._SensorMeasurementService.GetAllAsync();
            return measurements.ToList();
        }

        // ROUTE: .../iwsn/latest/all
        // get lastest measurements available based on variable => LATEST_RANGE_ALLOWED defined as private attribute
        [Route("latest/all/async")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MongoDBDatagramModel>>> GetLatestMeasurementsAsync()
        {
            var measurements = await this._SensorMeasurementService.GetAllAsync();
            var count = measurements.Count();

            if (count <= MAX_LATEST_RANGE_ALLOWED)
            {
                return measurements.ToList();
            }
            return measurements.ToList().GetRange(count - MAX_LATEST_RANGE_ALLOWED, MAX_LATEST_RANGE_ALLOWED);
        }

        // ROUTE: .../iwsn/latest/electric/all/async
        // get lastest measurements available based on variable => LATEST_RANGE_ALLOWED defined as private attribute
        [Route("latest/electric/all/async")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<float>>> GetLatestWattageMeasurementsAsync()
        {
            var measurements = await this._SensorMeasurementService.GetAllAsync();
            var count = measurements.Count();

            if (count <= MAX_LATEST_RANGE_ALLOWED)
            {
                return measurements.Select(item => item.Datagram.Telegram.InstantaneousElectricityUsage).ToList();
            }
            return measurements.Select(item => item.Datagram.Telegram.InstantaneousElectricityUsage)
                .ToList()
                .GetRange(count - MAX_LATEST_RANGE_ALLOWED, MAX_LATEST_RANGE_ALLOWED);
        }

        // ROUTE: .../iwsn/latest/single
        // get lastest measurement available based on variable => LATEST_RANGE_ALLOWED defined as private attribute
        [Route("latest/single/async")]
        [HttpGet]
        public async Task<ActionResult<MongoDBDatagramModel>> GetLatestSingleMeasurementAsync()
        {
            var measurements = await this._SensorMeasurementService.GetAllAsync();
            var count = measurements.Count();

            if (count > 0)
            {
                return measurements.ToList().LastOrDefault();
            }
            return NoContent();
        }

        [Route("latest/single/sync")]
        [HttpGet]
        public ActionResult<MongoDBDatagramModel> GetLatestSingleMeasurement()
        {
            var measurements = this._SensorMeasurementService.GetAll();
            var count = measurements.Count();

            if (count > 0)
            {
                return measurements.ToList().LastOrDefault();
            }
            return NoContent();
        }

        // ROUTE: .../iwsn/latest/solar
        // get all the available users - async
        [Route("lastest/solar/async")]
        [HttpGet]
        public async Task<ActionResult<SObject>> GetLatestSolarMeasurement()
        {
            var measurements = await this._SensorMeasurementService.GetAllAsync();
            return measurements.ToList().Last().Datagram.SolarPanel;
        }
    }
}