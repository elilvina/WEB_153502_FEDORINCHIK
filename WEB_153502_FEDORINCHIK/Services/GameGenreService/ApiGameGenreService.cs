using System.Text.Json;
using System.Text;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Entities;

namespace WEB_153502_FEDORINCHIK.Services.GameGenreService
{
    public class ApiGameGenreService : IGameGenreService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiGameGenreService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;


        public ApiGameGenreService(HttpClient httpClient, ILogger<ApiGameGenreService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }


        public async Task<ResponseData<List<GameGenre>>> GetGameGenreListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}GameGenres/");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<GameGenre>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<List<GameGenre>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: { ex.Message}",
                    };

                }
            }
            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            return new ResponseData<List<GameGenre>>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }
    }
}
