using Microsoft.AspNetCore.Mvc;
using Vehicles.Data.Models;
using Vehicles.Data.Repository;

namespace Vehicles.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        // updated one's 
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
                //var vehicles = VehicleRepository.Vehicles.Where(v => v.CustomerId == customerId).ToList();
                var vehicles = _vehicleRepository.GetVehiclesByCustomerId(customerId); // updated
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
