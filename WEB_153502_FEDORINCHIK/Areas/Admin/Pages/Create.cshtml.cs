using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Services.GameService;

namespace WEB_153502_FEDORINCHIK.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IGameService _gameService;

        public CreateModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var responce = await _gameService.CreateGameAsync(Game, Image);

            if (!responce.Success)
                return Page();

            return RedirectToPage("./Index");
        }
    }
}
