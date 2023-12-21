﻿using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Enities;

namespace WEB_153502_FEDORINCHIK.API.Services.MovieService
{
    public interface IMovieService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="genreNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <param name="pageSize">количество объектов на странице</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3);


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
        /// <returns></returns>
        public Task UpdateMovieAsync(int id, Movie movie);


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
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Movie>> CreateMovieAsync(Movie movie);


        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
