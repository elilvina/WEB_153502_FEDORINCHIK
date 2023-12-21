using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Services.GameService;

namespace WEB_153502_FEDORINCHIK.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IGameService _gameService;

        public EditModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _gameService.UpdateGameAsync(Game.Id, Game, Image);

            return RedirectToPage("./Index");
        }

        private async Task<bool> GameExists(int id)
        {
            var response = await _gameService.GetGameByIdAsync(id);
            return response.Success;
        }
    }
}
