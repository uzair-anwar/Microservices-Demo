using Customer.Data.DTO;
using Customer.Data.Models;
using Customer.Data.Repository;
using Customer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Controller
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    { 

        private readonly IVehicleService _vehicleService;

        public CustomerController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<CustomerModel>> GetCustomers()
        {
            return Ok(CustomerRepository.Customers);
        }

        [HttpPost]
        public ActionResult<CustomerModel> PostCustomer(CustomerModel customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest("Customer data is null.");
                }

                if (!CustomerRepository.Customers.Any())
                {
                    customer.Id = 1;
                }
                else
                {
                    customer.Id = CustomerRepository.Customers.Max(c => c.Id) + 1;
                }

                CustomerRepository.Customers.Add(customer);
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the customer. Error is: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult PutCustomer([FromQuery] int id, [FromBody] CustomerModel customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest("Customer data is null.");
                }

                var existingCustomer = CustomerRepository.Customers.FirstOrDefault(c => c.Id == id);
                if (existingCustomer == null)
                {
                    return NotFound($"Customer with ID {id} not found.");
                }

                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;

                return Ok(existingCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the customer. Error is: {ex.Message}");
            }
        }

        [HttpGet("byId")]
        public async Task<ActionResult<CustomerVehiclesDTO>> GetCustomer([FromQuery] int id)
        {
            try
            {
                var customer = CustomerRepository.Customers.FirstOrDefault(c => c.Id == id);
                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found.");
                }

                var vehicles = await _vehicleService.GetVehiclesByCustomerId(id);

                var customerWithVehicles = new CustomerVehiclesDTO
                {
                    Customer = customer,
                    Vehicles = vehicles.ToList()
                };

                return Ok(customerWithVehicles);
            }
            catch (HttpRequestException httpEx)
            {
                return StatusCode(503, $"HTTP error occurred while fetching vehicles: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
