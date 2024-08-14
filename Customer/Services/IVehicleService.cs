using Customer.Data.Models;

namespace Customer.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleModel>> GetVehiclesByCustomerIdAsync(int customerId);
    }
}
