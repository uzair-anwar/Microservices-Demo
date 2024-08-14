using Customer.Data.Models;

namespace Customer.Data.DTO
{
    public class CustomerVehiclesDTO
    {
        public CustomerModel? Customer { get; set; }
        public List<VehicleModel>? Vehicles { get; set; }

    }
}
