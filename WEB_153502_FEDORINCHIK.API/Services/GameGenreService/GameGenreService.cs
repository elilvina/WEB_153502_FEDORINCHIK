using Microsoft.EntityFrameworkCore;
using WEB_153502_FEDORINCHIK.API.Data;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;

namespace WEB_153502_FEDORINCHIK.API.Services.GameGenreService
{
    public class GameGenreService: IGameGenreService
    {
        private readonly AppDbContext _dbContext;

        public GameGenreService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseData<List<GameGenre>>> GetGameGenreListAsync()
        {
            return new ResponseData<List<GameGenre>>
            {
                Data = await _dbContext.GameGenres.ToListAsync(),
            };
        }

    }
}
