using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153502_FEDORINCHIK.API.Data;
using WEB_153502_FEDORINCHIK.API.Services.GameServices;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;

namespace WEB_153502_FEDORINCHIK.Tests
{
    public class GameServiceTests: IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public GameServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new AppDbContext(_contextOptions);

            context.Database.EnsureCreated();
          

            context.GameGenres.AddRange(
                new GameGenre { Id = 1, Name = "Шутер", NormalizedName = "shooter" },
                new GameGenre { Id = 2, Name = "Гонки", NormalizedName = "race" },
                new GameGenre { Id = 3, Name = "Файтинг", NormalizedName = "fighting" },
                new GameGenre { Id = 4, Name = "Симулятор", NormalizedName = "simulator" });

            context.Games.AddRange(
                new Game
                {
                    Id = 1,
                    Name = "Call of Duty: Modern Warfare",
                    Description = "Самый реалистичный шутер от первого лица",
                    GenreId = 1,
                    Price = 49.99m,
                },
                new Game
                {
                    Id = 2,
                    Name = "Need for Speed: Heat",
                    Description = "Горячие гонки по улицам ночного города",
                    GenreId = 2,
                    Price = 39.99m,
                },
                new Game
                {
                    Id = 3,
                    Name = "Street Fighter 6",
                    Description = "Легендарные бои в жанре файтинга",
                    GenreId = 3,
                    Price = 29.99m,
                },
                new Game
                {
                    Id = 4,
                    Name = "The Sims 4",
                    Description = "Создайте свой мир в симуляторе жизни",
                    GenreId = 4,
                    Price = 34.99m,
                },
                new Game
                {
                    Id = 5,
                    Name = "Assassin's Creed Valhalla",
                    Description = "Станьте викингом и завоюйте новые земли",
                    GenreId = 1,
                    Price = 59.99m,
                },
                new Game
                {
                    Id = 6,
                    Name = "FIFA 22",
                    Description = "Лучшая футбольная симуляция всех времен",
                    GenreId = 4,
                    Price = 49.99m,
                },
                new Game
                {
                    Id = 7,
                    Name = "Mortal Kombat 11",
                    Description = "Сражайтесь в брутальных боях на жизнь и смерть",
                    GenreId = 3,
                    Price = 39.99m,
                },
                new Game
                {
                    Id = 8,
                    Name = "Grand Theft Auto V",
                    Description = "Откройте мир преступности и приключений",
                    GenreId = 2,
                    Price = 29.99m,
                });

            context.SaveChanges();
        }

        AppDbContext CreateContext() => new AppDbContext(_contextOptions);
        public void Dispose() => _connection.Dispose();

        [Fact]
        public void ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new GameService(context, null!, null!, null!, null!);

            var result = service.GetGameListAsync(null).Result;

            Assert.IsType<ResponseData<ListModel<Game>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data!.CurrentPage);
            Assert.Equal(3, result.Data!.Items!.Count);
            Assert.Equal(3, result.Data!.TotalPages);
            Assert.Equal(context.Games.First(), result.Data.Items[0]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ServiceChoosesRightSpecifiedPage(int pageNo)
        {
            using var context = CreateContext();
            var service = new GameService(context, null!, null!, null!, null!);

            var result = service.GetGameListAsync(null, pageNo).Result;

            Assert.IsType<ResponseData<ListModel<Game>>>(result);
            Assert.True(result.Success);
            Assert.Equal(pageNo, result.Data!.CurrentPage);
            Assert.Equal(3, result.Data!.TotalPages);
            Assert.Equal(context.Games.Skip((pageNo - 1) * 3).First(), result.Data.Items![0]);
        }

        [Fact]
        public void ServicePerformsCorrectFilteringByCategory()
        {
            using var context = CreateContext();
            var service = new GameService(context, null!, null!, null!, null!);

            var result = service.GetGameListAsync("shooter").Result;

            Assert.IsType<ResponseData<ListModel<Game>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data!.CurrentPage);
            Assert.Equal(2, result.Data.Items!.Count);
            Assert.Equal(1, result.Data.TotalPages);
            Assert.All(result.Data.Items, item => Assert.True(item.GenreId == 1));
        }


        [Fact]
        public void ServiceDoesNotAllowToSetPageSizeMoreThanMax()
        {
            using var context = CreateContext();
            var service = new GameService(context, null!, null!, null!, null!);

            var result = service.GetGameListAsync(null, 1, service.MaxPageSize + 1).Result;

            Assert.IsType<ResponseData<ListModel<Game>>>(result);
            Assert.True(result.Data!.Items!.Count <= service.MaxPageSize);
        }

        [Fact]
        public void ServiceDoesNotAllowToSetPageNumberMoreThanPagesCount()
        {
            using var context = CreateContext();
            var service = new GameService(context, null!, null!, null!, null!);

            var result = service.GetGameListAsync(null!, 100).Result;

            Assert.IsType<ResponseData<ListModel<Game>>>(result);
            Assert.False(result.Success);
        }

    }
}
