using Microsoft.AspNetCore.Mvc;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Services.GameGenreService;

namespace WEB_153502_FEDORINCHIK.Services.GameService
{
    public class MemoryGameService : IGameService
    {
        private List<Game> _games = new();
        private List<GameGenre> _genres = new();
        private IConfiguration _config;

        public MemoryGameService([FromServices] IConfiguration config, IGameGenreService gameGenreService)
        {
            _config  = config;
            SetupData();
        }


        public Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGameAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var filteredGames =
                genreNormalizedName != null ?
                _games.Where(game => game.Genre?.NormalizedName == genreNormalizedName).ToList() :
                _games;

            int itemsPerPage = _config.GetValue<int>("ItemsPerPage");

            int totalPages =
                filteredGames.Count() % itemsPerPage == 0 ?
                filteredGames.Count() / itemsPerPage :
                filteredGames.Count() / itemsPerPage + 1;         


            var responseData = new ResponseData<ListModel<Game>>
            {
                Data = new ListModel<Game>
                {
                    Items = filteredGames.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages,
                }
            };

            return Task.FromResult(responseData);
        }

        public Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        private void SetupData()
        {
            _games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Name = "Call of Duty: Modern Warfare",
                    Description = "Самый реалистичный шутер от первого лица",
                    Genre = new GameGenre { Id = 1, Name = "Шутер", NormalizedName = "shooter" },
                    Price = 49.99m,
                    Path = "/images/cod.png"
                },
                new Game
                {
                    Id = 2,
                    Name = "Need for Speed: Heat",
                    Description = "Горячие гонки по улицам ночного города",
                    Genre = new GameGenre { Id = 2, Name = "Гонки", NormalizedName = "race" },
                    Price = 39.99m,
                    Path = "/images/nfs.png"
                },
                new Game
                {
                    Id = 3,
                    Name = "Street Fighter 6",
                    Description = "Легендарные бои в жанре файтинга",
                    Genre = new GameGenre { Id = 3, Name = "Файтинг", NormalizedName = "fighting" },
                    Price = 29.99m,
                    Path = "/images/sf6.png"
                },
                new Game
                {
                    Id = 4,
                    Name = "The Sims 4",
                    Description = "Создайте свой мир в симуляторе жизни",
                    Genre = new GameGenre { Id = 4, Name = "Симулятор", NormalizedName = "simulator" },
                    Price = 34.99m,
                    Path = "/images/sims4.png"
                },
                new Game
                {
                    Id = 5,
                    Name = "Assassin's Creed Valhalla",
                    Description = "Станьте викингом и завоюйте новые земли",
                    Genre = new GameGenre { Id = 1, Name = "Шутер", NormalizedName = "shooter" },
                    Price = 59.99m,
                    Path = "/images/acv.png"
                },
                new Game
                {
                    Id = 6,
                    Name = "FIFA 22",
                    Description = "Лучшая футбольная симуляция всех времен",
                    Genre = new GameGenre { Id = 4, Name = "Симулятор", NormalizedName = "simulator" },
                    Price = 49.99m,
                    Path = "/images/fifa22.png"
                },
                new Game
                {
                    Id = 7,
                    Name = "Mortal Kombat 11",
                    Description = "Сражайтесь в брутальных боях на жизнь и смерть",
                    Genre = new GameGenre { Id = 3, Name = "Файтинг", NormalizedName = "fighting" },
                    Price = 39.99m,
                    Path = "/images/mk11.png"
                },
                new Game
                {
                    Id = 8,
                    Name = "Grand Theft Auto V",
                    Description = "Откройте мир преступности и приключений",
                    Genre = new GameGenre { Id = 2, Name = "Гонки", NormalizedName = "race" },
                    Price = 29.99m,
                    Path = "/images/gta5.png"
                }


            };
        }
    }
}
