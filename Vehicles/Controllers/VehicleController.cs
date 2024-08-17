using Microsoft.AspNetCore.Mvc;
using Vehicles.Data.Models;
using Vehicles.Data.Repository;

namespace Vehicles.Controller
{
    [Route("api/vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet("customervehicles/{customerId}")]
        public ActionResult<IEnumerable<Vehicle>> GetVehiclesByCustomerId(int customerId)
        {
            try
            {
                var vehicles = _vehicleRepository.GetVehiclesByCustomerId(customerId);
                if (!vehicles.Any())
                {
                    return NotFound($"No vehicles found for customer with ID {customerId}.");
                }
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving vehicles: {ex.Message}");
            }
        }

    }
}
