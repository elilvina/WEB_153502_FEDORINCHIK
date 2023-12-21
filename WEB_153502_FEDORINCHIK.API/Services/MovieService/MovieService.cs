using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WEB_153502_FEDORINCHIK.API.Data;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Enities;

namespace WEB_153502_FEDORINCHIK.API.Services.MovieService
{
    public class MovieService : IMovieService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _dbContext;


        public MovieService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<ResponseData<Movie>> CreateMovieAsync(Movie movie)
        {

            try
            {
                _dbContext.Movies.Add(movie);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseData<Movie>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }

            return new ResponseData<Movie>
            {
                Data = movie,
                Success = true,
            };
        }


        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie is null)
                return;


            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<ResponseData<Movie>> GetMovieByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie is null)
            {
                return new ResponseData<Movie>
                {
                    Success = false,
                    ErrorMessage = "Movie not founded",
                };
            }

            return new ResponseData<Movie>
            {
                Data = movie,
                Success = true,
            };
        }


        public async Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _dbContext.Movies.AsQueryable();
            var dataList = new ListModel<Movie>();
            query = query.Where(d => genreNormalizedName == null || d.Genre!.NormalizedName.Equals(genreNormalizedName));

            var count = await query.CountAsync();

            if (count == 0)
            {
                return new ResponseData<ListModel<Movie>>
                {
                    Data = dataList,
                    Success = true,
                };
            }

            //int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            int totalPages =
                    count % pageSize == 0 ?
                    count / pageSize :
                    count / pageSize + 1;

            if (pageNo > totalPages)
            {
                return new ResponseData<ListModel<Movie>>
                {
                    Success = false,
                    ErrorMessage = "No such page"
                };
            }

            dataList.Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return new ResponseData<ListModel<Movie>>
            {
                Data = dataList,
                Success = true,
            };
        }


        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie is null)
            {
                return new ResponseData<string>
                {
                    Success = false,
                    ErrorMessage = "No item found",
                };
            }

            var host = "https://"/* + _httpContextAccessor.HttpContext?.Request.Host*/;
            var imageFolder = Path.Combine(/*_webHostEnvironment.WebRootPath, */"images");

            if (formFile is not null)
            {
                if (!string.IsNullOrEmpty(movie.Path))
                {
                    var prevImage = Path.GetFileName(movie.Path);
                    var prevImagePath = Path.Combine(imageFolder, prevImage);
                    if (File.Exists(prevImagePath))
                    {
                        File.Delete(prevImagePath);
                    }
                }
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                var filePath = Path.Combine(imageFolder, fName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                movie.Path = $"{host}/images/{fName}";
                await _dbContext.SaveChangesAsync();
            }

            return new ResponseData<string>()
            {
                Data = movie.Path,
                Success = true,
            };
        }


        public async Task UpdateMovieAsync(int id, Movie movie)
        {
            var updatingMovie = await _dbContext.Movies.FindAsync(id);

            if (updatingMovie is null)
                return;

            updatingMovie.Name = movie.Name;
            updatingMovie.Description = movie.Description;
            updatingMovie.Price = movie.Price;
            updatingMovie.Path = movie.Path;
            updatingMovie.Genre = movie.Genre;
        }
    }
}
