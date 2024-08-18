using Customer.Data.Models;

namespace Customer.Services
{
    public interface IVehicleService
    {
        Task<List<VehicleModel>> GetVehiclesByCustomerId(int customerId);
    }
}
