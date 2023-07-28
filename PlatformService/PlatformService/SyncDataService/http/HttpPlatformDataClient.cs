using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataService.http
{
    public class HttpPlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public HttpPlatformDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task SendPlatformToCommandService(PlatformReadDto platformCreateDto)
        {
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(platformCreateDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_configuration["CommandService"], httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("----> Data sync sucessfuly");
            } else
            {
                Console.WriteLine("----> Data sync not sucessfuly");
            }
        }
    }
}
