using Polly;
using Polly.CircuitBreaker;
using Customer.Data.Models;
using Customer.Services;

namespace Customer.Data
{
    public class VehicleService: IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehicleService> _logger;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public VehicleService(HttpClient httpClient, ILogger<VehicleService> logger)
        {
            _httpClient = httpClient;
            _circuitBreakerPolicy = Policy
          .Handle<HttpRequestException>()
          .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
            _logger = logger;
        }

        public async Task<IEnumerable<VehicleModel>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            try
            {
                return await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync($"customervehicles/{customerId}");

                    response.EnsureSuccessStatusCode();

                    var vehicles = await response.Content.ReadFromJsonAsync<IEnumerable<VehicleModel>>();

                    return vehicles ?? new List<VehicleModel>();

                });

            }
            catch (BrokenCircuitException)
            {
                _logger.LogWarning("Circuit is open! Service is currently unavailable.");
                return new List<VehicleModel>();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"HTTP error occurred: {httpEx.Message}");
                return new List<VehicleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return new List<VehicleModel>();
            }
        }
    }
}