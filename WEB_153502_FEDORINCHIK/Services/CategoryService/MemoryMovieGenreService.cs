using WEB_153502_FEDORINCHIK.Domain.Enities;
using WEB_153502_FEDORINCHIK.Domain.Models;

namespace WEB_153502_FEDORINCHIK.Services.MovieGenreService
{
    public class MemoryMovieGenreService : IMovieGenreService
    {
        public Task<ResponseData<List<MovieGenre>>> GetCategoryListAsync()
        {
            var movieGenres = new List<MovieGenre>
            {
                new MovieGenre { Id = 1, Name = "Ужасы", NormalizedName = "horror"},
                new MovieGenre { Id = 2, Name = "Комедии", NormalizedName = "comedy"},
                new MovieGenre { Id = 3, Name = "Документальные", NormalizedName = "documentary"},
                new MovieGenre { Id = 4, Name = "Любимые", NormalizedName = "love"}
            };
            var result = new ResponseData<List<MovieGenre>>();
            result.Data = movieGenres;
            return Task.FromResult(result);
        }
    }
}
