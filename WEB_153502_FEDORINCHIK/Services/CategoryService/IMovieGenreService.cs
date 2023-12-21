using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Enities;

namespace WEB_153502_FEDORINCHIK.Services.MovieGenreService
{
    public interface IMovieGenreService
    {
        public Task<ResponseData<List<MovieGenre>>> GetCategoryListAsync();
    }
}
