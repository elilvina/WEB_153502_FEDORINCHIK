using System.Text.Json;
using System.Text;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using Azure.Core;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace WEB_153502_FEDORINCHIK.Services.GameService
{
    public class ApiGameService : IGameService
    {
        private readonly HttpClient _httpClient;
        private string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiGameService> _logger;
        private readonly HttpContext _httpContext;

        public ApiGameService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiGameService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value!;

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
            var token = _httpContext.GetTokenAsync("access_token").Result;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Games/");

            if (genreNormalizedName != null)
            {
                urlString.Append($"{genreNormalizedName}/");
            }

            if (pageNo > 1)
            {
                urlString.Append($"{pageNo}");
            }

            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
            }

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<ListModel<Game>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            return new ResponseData<ListModel<Game>>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }


        public async Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Games");
            var response = await _httpClient.PostAsJsonAsync(uri, game, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);
                if (formFile != null)
                {
                    await SaveImageAsync(data!.Data!.Id, formFile);
                }
                return data!;
            }
            _logger.LogError($"Game not created. Error: {response.StatusCode}");

            return new ResponseData<Game>
            {
                Success = false,
                ErrorMessage = $"Game not add. Error: {response.StatusCode}"
            };
        }

        public async Task DeleteGameAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Games/game-{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            }
        }

        public async Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Games/game-{id}");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<Game>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"No data received from the server. Error: {response.StatusCode}");

            return new ResponseData<Game>()
            {
                Success = false,
                ErrorMessage = $"No data received from the server. Error: {response.StatusCode}"
            };
        }


        public async Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Games/game-" + id);
            var response = await _httpClient.PutAsJsonAsync(uri, game, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"No data received from the server. Error: {response.StatusCode}");
            }
            else if (formFile != null)
            {
                int gameId = (await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions))!.Data!.Id;
                await SaveImageAsync(gameId, formFile);
            }
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Games/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;

            await _httpClient.SendAsync(request);

        }

    }
}
