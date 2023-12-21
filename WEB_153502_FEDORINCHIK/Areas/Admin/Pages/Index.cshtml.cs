using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Services.GameService;

namespace WEB_153502_FEDORINCHIK.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;

        public IndexModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IList<Game> Game { get;set; } = default!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; } 

        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            var response = await _gameService.GetGameListAsync(null, pageNo);

            if (!response.Success)
                return NotFound();

            Game = response.Data?.Items!;
            CurrentPage = response.Data?.CurrentPage ?? 0;
            TotalPages = response.Data?.TotalPages ?? 0;

            return Page();
        }
    }
}
