using System.Text.Json;
using System.Text;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Enities;
using WEB_153502_FEDORINCHIK.Services.MovieGenreService;

namespace WEB_153502_FEDORINCHIK.Services.CategoryService
{
    public class ApiMovieGenreService : IMovieGenreService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiMovieGenreService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;


        public ApiMovieGenreService(HttpClient httpClient, ILogger<ApiMovieGenreService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }


        public async Task<ResponseData<List<MovieGenre>>> GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}MovieGenres/");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<MovieGenre>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<List<MovieGenre>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}",
                    };

                }
            }
            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            return new ResponseData<List<MovieGenre>>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }
    }
}
