using Microsoft.Extensions.Options;
using System.Text;
using Users.Data.Models;
using Users.Services;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Users.Data
{
    public class OktaService : IOktaService
    {
        private readonly IOptions<OktaTokenSettings> _options;
        private readonly HttpClient _client;

        public OktaService(IOptions<OktaTokenSettings> options, IHttpClientFactory httpClientFactory)
        {
            _options = options;
            _client = httpClientFactory.CreateClient("OktaClient");
        }

        public async Task<OktaResponse> GetTokenAsync(string username, string password)
        {
            try
            {
                var token = new OktaResponse();
                var oktaDomain = _options.Value.OktaDomain;
                var clientId = _options.Value.ClientId;
                var clientSecret = _options.Value.ClientSecret;

                var clientCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientCreds);

                var postData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "scope", "openid email profile address phone" },
                { "username", username },
                { "password", password }
            };

                var request = new HttpRequestMessage(HttpMethod.Post, $"{oktaDomain}/oauth2/default/v1/token")
                {
                    Content = new FormUrlEncodedContent(postData)
                };

                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<OktaResponse>(result);
                    token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException(error);
                }
                return token;
            } 
            catch (Exception ex)
            {
                throw new Exception($"Unable to get token. {ex.Message}");
            }
        }

        public async Task<bool> AuthenticateToken(string token)
        {
            try
            {
                var oktaDomain = _options.Value.OktaDomain;
                var clientId = _options.Value.ClientId;
                var clientSecret = _options.Value.ClientSecret;
                var clientCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientCreds);

                var postData = new Dictionary<string, string>
                {
                   { "token", token }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, $"{oktaDomain}/oauth2/default/v1/introspect")
                {
                    Content = new FormUrlEncodedContent(postData)
                };

                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                var introspectionResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                if (introspectionResult != null && introspectionResult.TryGetValue("active", out var active))
                {
                    return Convert.ToBoolean(active);
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to authenticate token. {ex.Message}");
            }
        }


        public async Task<UserInfo> GetUserAsync(string token)
        {
            try
            {
                var oktaDomain = _options.Value.OktaDomain;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"{oktaDomain}/oauth2/default/v1/userinfo");
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                var userInfo = JsonConvert.DeserializeObject<UserInfo>(responseContent);

                if (userInfo == null)
                {
                    throw new Exception("Failed to deserialize user information.");
                }

                return userInfo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to get user information from Okta. {ex.Message}");
            }
        }

    }
}
