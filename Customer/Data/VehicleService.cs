using Customer.Data.Models;
using Customer.Services;

namespace Customer.Data
{
    public class VehicleService: IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(HttpClient httpClient, ILogger<VehicleService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<VehicleModel>> GetVehiclesByCustomerId(int customerId)
        {
            try
            {
                
                var response = await _httpClient.GetAsync($"customervehicles/{customerId}");

                response.EnsureSuccessStatusCode();

                var vehicles = await response.Content.ReadFromJsonAsync<List<VehicleModel>>();

                return vehicles ?? new List<VehicleModel>();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"HTTP error occurred: {httpEx.Message}");
                throw new Exception($"HTTP error occurred. {httpEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw new Exception($"An error occurred. {ex.Message}");
            }
        }
    }
}