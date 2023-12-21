using System.Threading.Tasks;
using WEB_153502_FEDORINCHIK.Domain.Enities;
using WEB_153502_FEDORINCHIK.Domain.Models;

namespace WEB_153502_FEDORINCHIK.Services.MovieService
{
    public interface IMovieService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="genreNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? genreNormalizedName, int pageNo = 1);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Movie>> GetMovieByIdAsync(int id);

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемомго объекта</param>
        /// <param name="movie">объект с новыми параметрами</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        public Task UpdateMovieAsync(int id, Movie movie, IFormFile? formFile);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемомго объекта</param>
        /// <returns></returns>
        public Task DeleteMovieAsync(int id);

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="movie">Новый объект</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Movie>> CreateMovieAsync(Movie movie, IFormFile? formFile);

    }
}