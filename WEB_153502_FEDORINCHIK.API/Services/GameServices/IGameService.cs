using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;

namespace WEB_153502_FEDORINCHIK.API.Services.GameServices
{
    public interface IGameService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="genreNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <param name="pageSize">количество объектов на странице</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3);


        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Game>> GetGameByIdAsync(int id);


        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемомго объекта</param>
        /// <param name="game">объект с новыми параметрами</param>
        /// <returns></returns>
        public Task UpdateGameAsync(int id, Game game);


        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемомго объекта</param>
        /// <returns></returns>
        public Task DeleteGameAsync(int id);


        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="game">Новый объект</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Game>> CreateGameAsync(Game game);


        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
