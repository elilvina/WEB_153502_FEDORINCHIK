using WEB_153502_FEDORINCHIK.API.Data;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Enities;
using Microsoft.EntityFrameworkCore;

namespace WEB_153502_FEDORINCHIK.API.Services.MovieGenreService
{
    public class MovieGenreService : IMovieGenreService
    {
        private readonly AppDbContext _dbContext;

        public MovieGenreService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseData<List<MovieGenre>>> GetMovieGenreListAsync()
        {
            return new ResponseData<List<MovieGenre>>
            {
                Data = await _dbContext.MovieGenres.ToListAsync(),
            };
        }
    }
}
