using System.Text.Json;
using System.Text;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Enities;

namespace WEB_153502_FEDORINCHIK.Services.MovieService
{
    public class ApiMovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiMovieService> _logger;

        public ApiMovieService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiMovieService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value!;

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Movies/");

            if (genreNormalizedName != null)
            {
                urlString.Append($"{genreNormalizedName}/");
            };

            if (pageNo > 1)
            {
                urlString.Append($"{pageNo}");
            };

            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
            }

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Movie>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<ListModel<Movie>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            return new ResponseData<ListModel<Movie>>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }


        public async Task<ResponseData<Movie>> CreateMovieAsync(Movie movie, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Movies");
            var response = await _httpClient.PostAsJsonAsync(uri, movie, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Movie>>(_serializerOptions);
                if (formFile != null)
                {
                    await SaveImageAsync(data!.Data!.Id, formFile);
                }
                return data!;
            }
            _logger.LogError($"Movie not created. Error: {response.StatusCode}");

            return new ResponseData<Movie>
            {
                Success = false,
                ErrorMessage = $"Movie not add. Error: {response.StatusCode}"
            };
        }

        public async Task DeleteMovieAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Movies/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            }
        }

        public async Task<ResponseData<Movie>> GetMovieByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Movies/movie-{id}");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Movie>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<Movie>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            return new ResponseData<Movie>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }


        public async Task UpdateMovieAsync(int id, Movie movie, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Movies/" + id);
            var response = await _httpClient.PutAsJsonAsync(uri, movie, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            }
            else if (formFile != null)
            {
                int toolId = (await response.Content.ReadFromJsonAsync<ResponseData<Movie>>(_serializerOptions))!.Data!.Id;
                await SaveImageAsync(toolId, formFile);
            }
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress?.AbsoluteUri}Movies/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
        }

    }
}
