using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Services.GameService;

namespace WEB_153502_FEDORINCHIK.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IGameService _gameService;

        public DetailsModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _gameService.GetGameByIdAsync(id.Value);

            if (!response.Success)
                return NotFound();

            Game = response.Data!;

            return Page();
        }
    }
}
