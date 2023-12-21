using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153502_FEDORINCHIK.Services.GameGenreService;
using WEB_153502_FEDORINCHIK.Services.GameService;
using WEB_153502_FEDORINCHIK.Controllers;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;


namespace WEB_153502_FEDORINCHIK.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void IndexReturns404WhenGenresAreReceivedUnsuccessfully()
        {
            var genreService = new Mock<IGameGenreService>();
            genreService.Setup(m => m.GetGameGenreListAsync())
                .ReturnsAsync(new ResponseData<List<GameGenre>> { Success = false });

            var gameService = new Mock<IGameService>();
            gameService.Setup(m => m.GetGameListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Game>> { Success = true });

            var controller = new ProductController(gameService.Object, genreService.Object);

            var result = controller.Index(null).Result;

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void IndexReturns404WhenGamesAreReceivedUnsuccessfully()
        {
            var genreService = new Mock<IGameGenreService>();
            genreService.Setup(m => m.GetGameGenreListAsync())
                .ReturnsAsync(new ResponseData<List<GameGenre>> { Success = true });

            var gameService = new Mock<IGameService>();
            gameService.Setup(m => m.GetGameListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Game>> { Success = false });

            var controller = new ProductController(gameService.Object, genreService.Object);

            var result = controller.Index(null).Result;

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void IndexViewDataContainsValidCurrentGenreWhenGenreParameterIsNull()
        {
            var genreService = new Mock<IGameGenreService>();
            genreService.Setup(m => m.GetGameGenreListAsync())
                .ReturnsAsync(new ResponseData<List<GameGenre>> { Success = true });

            var gameService = new Mock<IGameService>();
            gameService.Setup(m => m.GetGameListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Game>> { Success = true });

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            var controller = new ProductController(gameService.Object, genreService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = controller.Index(null).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var fff = viewResult.ViewData["currentGenre"] as GameGenre;
            Assert.Null((viewResult.ViewData["currentGenre"] as GameGenre)!.NormalizedName);
        }

        [Fact]
        public void IndexViewDataContainsValidCurrentGenreWhenGenreParameterIsNotNull()
        {
            var testGenres = new List<GameGenre>
            {
                new GameGenre { Id = 1, Name = "Шутер", NormalizedName = "shooter"},
                new GameGenre { Id = 2, Name = "Гонки", NormalizedName = "race"},
            };

            var testGames = new List<Game>
            {
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
            };

            var genreService = new Mock<IGameGenreService>();
            genreService.Setup(m => m.GetGameGenreListAsync())
                .ReturnsAsync(new ResponseData<List<GameGenre>>
                {
                    Success = true,
                    Data = testGenres
                });

            var gameService = new Mock<IGameService>();
            gameService.Setup(m => m.GetGameListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Game>>
                {
                    Success = true,
                    Data = new() { Items = testGames }
                });


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            var controller = new ProductController(gameService.Object, genreService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = controller.Index("shooter").Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(testGenres.First(c => c.NormalizedName == "shooter"),(viewResult.ViewData["currentGenre"] as GameGenre));
        }

        [Fact]
        public void IndexRightModel()
        {
            var testGenres = new List<GameGenre>
            {
                new GameGenre { Id = 1, Name = "Шутер", NormalizedName = "shooter"},
                new GameGenre { Id = 2, Name = "Гонки", NormalizedName = "race"},
            };

            var testGames = new List<Game>
            {
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
            };

            var genreService = new Mock<IGameGenreService>();
            genreService.Setup(m => m.GetGameGenreListAsync())
                .ReturnsAsync(new ResponseData<List<GameGenre>>
                {
                    Success = true,
                    Data = testGenres
                });

            var model = new ListModel<Game>() { Items = testGames };
            var gameService = new Mock<IGameService>();
            gameService.Setup(m => m.GetGameListAsync(It.IsAny<string?>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Game>>
                {
                    Success = true,
                    Data = model
                });


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            var controller = new ProductController(gameService.Object, genreService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = controller.Index(null).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<ListModel<Game>>(viewResult.Model);
            Assert.Equal(model, modelResult);
        }
    }

}