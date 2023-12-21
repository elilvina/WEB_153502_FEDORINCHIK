using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Enities;

namespace WEB_153502_FEDORINCHIK.API.Services.MovieGenreService
{
    public interface IMovieGenreService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<MovieGenre>>> GetMovieGenreListAsync();
    }
}
