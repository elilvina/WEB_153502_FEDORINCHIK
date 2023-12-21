using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Extensions;
using WEB_153502_FEDORINCHIK.Services.GameGenreService;
using WEB_153502_FEDORINCHIK.Services.GameService;

namespace WEB_153502_FEDORINCHIK.Controllers
{

    public class ProductController : Controller
    {
        private IGameGenreService _gameGenreService;
        private IGameService _gameService;

        public ProductController(IGameService gameService, IGameGenreService gameGenreService)
        {
            _gameGenreService = gameGenreService;
            _gameService = gameService;
        }

        [Route("Catalog")]
        [Route("Catalog/{gameGenreNormalized?}")]
        public async Task<IActionResult> Index(string? gameGenreNormalized, int pageNo = 1)
        {
            var genresResponse = await _gameGenreService.GetGameGenreListAsync();
            if (!genresResponse.Success)
                return NotFound(genresResponse.ErrorMessage);

            var genres = genresResponse.Data;

            ViewData["genres"] = genres;
            ViewData["currentGenre"] =
                gameGenreNormalized == null ?
                new GameGenre { Name = "Все", NormalizedName = null } :
                genres?.SingleOrDefault(g=> g.NormalizedName == gameGenreNormalized);


            if (gameGenreNormalized == "Все")
                gameGenreNormalized = null;

            var gameResponse = await _gameService.GetGameListAsync(gameGenreNormalized, pageNo);
            if (!gameResponse.Success)
                return NotFound(gameResponse.ErrorMessage);

            if (Request.IsAjaxRequest())
            {
                ListModel<Game> data = gameResponse.Data!;
                return PartialView("_ProductIndexPartial", new
                {
                    Items = data.Items,
                    CurrentPage = pageNo,
                    TotalPages =  data.TotalPages,
                    GameGenreNormalized = gameGenreNormalized
                });
            }
            else
            {
                //return View(new ListModel<Game>
                //{
                //    Items = gameResponse.Data.Items,
                //    CurrentPage = pageNo,
                //    TotalPages = gameResponse.Data.TotalPages,
                //});
                return View(gameResponse.Data);
            }
            
        }
    }
}
