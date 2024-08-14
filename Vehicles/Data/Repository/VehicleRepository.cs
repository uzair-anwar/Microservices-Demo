using Vehicles.Data.Models;

namespace Vehicles.Data.Repository
{
    // updated///
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetVehiclesByCustomerId(int customerId);
    }
    public class VehicleRepository : IVehicleRepository
    {
        //updated//
        public IEnumerable<Vehicle> GetVehiclesByCustomerId(int customerId)
        {
            return Vehicles.Where(v => v.CustomerId == customerId).ToList();
        }

        public List<Vehicle> Vehicles = new List<Vehicle>
        {
        new Vehicle { Id = 1, Name = "Accord", Model = "2018", Company = "Honda", CustomerId = 1 },
        new Vehicle { Id = 2, Name = "Civic", Model = "2019", Company = "Honda", CustomerId = 1 },
        new Vehicle { Id = 3, Name = "Gwagon", Model = "2022", Company = "Mercedes", CustomerId = 2 },
        new Vehicle { Id = 3, Name = "Land Crusior", Model = "2023", Company = "Toyota", CustomerId = 2 },
        new Vehicle { Id = 3, Name = "A5", Model = "2022", Company = "Audi", CustomerId = 3 },
        new Vehicle { Id = 3, Name = "Swift", Model = "2014", Company = "Suzuki", CustomerId = 4 }
        };


    }

}
