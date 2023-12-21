using Microsoft.EntityFrameworkCore;
using WEB_153502_FEDORINCHIK.Domain.Entities;

namespace WEB_153502_FEDORINCHIK.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            var gameGenres = new List<GameGenre>
            {
                new GameGenre { Name = "Шутер", NormalizedName = "shooter"},
                new GameGenre { Name = "Гонки", NormalizedName = "race"},
                new GameGenre { Name = "Файтинг", NormalizedName = "fighting"},
                new GameGenre { Name = "Симулятор", NormalizedName = "simulator"}
            };

            await context.GameGenres.AddRangeAsync(gameGenres);
            await context.SaveChangesAsync();


            string imageRoot = $"{app.Configuration["AppUrl"]!}/images";

            var games = new List<Game>
            {

                new Game
                {
                    Name = "Call of Duty: Modern Warfare",
                    Description = "Самый реалистичный шутер от первого лица",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("shooter")),
                    Price = 49.99m,
                    Path = $"{imageRoot}/cod.png"
                },
                new Game
                {
                    Name = "Need for Speed: Heat",
                    Description = "Горячие гонки по улицам ночного города",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("race")),
                    Price = 39.99m,
                    Path = $"{imageRoot}/nfs.png"
                },
                new Game
                {
                    Name = "Street Fighter 6",
                    Description = "Легендарные бои в жанре файтинга",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("fighting")),
                    Price = 29.99m,
                    Path = $"{imageRoot}/sf6.png"
                },
                new Game
                {
                    Name = "The Sims 4",
                    Description = "Создайте свой мир в симуляторе жизни",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("simulator")),
                    Price = 34.99m,
                    Path = $"{imageRoot}/sims4.png"
                },
                new Game
                {
                    Name = "Assassin's Creed Valhalla",
                    Description = "Станьте викингом и завоюйте новые земли",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("shooter")),
                    Price = 59.99m,
                    Path = $"{imageRoot}/acv.png"
                },
                new Game
                {
                    Name = "FIFA 22",
                    Description = "Лучшая футбольная симуляция всех времен",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("simulator")),
                    Price = 49.99m,
                    Path = $"{imageRoot}/fifa22.png"
                },
                new Game
                {
                    Name = "Mortal Kombat 11",
                    Description = "Сражайтесь в брутальных боях на жизнь и смерть",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("fighting")),
                    Price = 39.99m,
                    Path = $"{imageRoot}/mk11.png"
                },
                new Game
                {
                    Name = "Grand Theft Auto V",
                    Description = "Откройте мир преступности и приключений",
                    Genre = await context.GameGenres.SingleAsync(g => g.NormalizedName.Equals("race")),
                    Price = 29.99m,
                    Path = $"{imageRoot}/gta5.png"
                }

            };

            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();

        }
    }
}
