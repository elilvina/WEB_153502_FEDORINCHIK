using System.Text.Json;
using System.Text;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;

namespace WEB_153502_FEDORINCHIK.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        public event Action DataChanged;
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly ILogger<DataService> _logger;
		private readonly int _pageSize = 3;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider, ILogger<DataService> logger)
        {
            _httpClient = httpClient;
			_logger = logger;
			_accessTokenProvider = accessTokenProvider;
            _pageSize = configuration.GetSection("PageSize").Get<int>();
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public List<GameGenre> Genres { get; set; }
        public List<Game> GameList { get; set; }
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task GetGameListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/Games/");
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

                var ff = new Uri(urlString.ToString());

				var response = await _httpClient.GetAsync(ff);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>(_jsonSerializerOptions);
                        GameList = responseData?.Data?.Items;
                        TotalPages = responseData?.Data?.TotalPages ?? 0;
                        CurrentPage = responseData?.Data?.CurrentPage ?? 0;
                        DataChanged?.Invoke();
						_logger.LogInformation("<------ Clothes list received successfully ------>");
					}
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
                }
            }
     
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/games/{id}");
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return (await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_jsonSerializerOptions))?.Data;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                        return null;
                    }
                }
                Success = false;
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
            }
            return null;          
        }

        public async Task GetGenreListAsync()
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}api/GameGenres/");
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseData = await response.Content.ReadFromJsonAsync<ResponseData<List<GameGenre>>>(_jsonSerializerOptions);
                        Genres = responseData?.Data;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
                }
            }           
        }
    }
}
