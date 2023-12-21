using Microsoft.AspNetCore.Mvc;
using WEB_153502_FEDORINCHIK.Domain.Enities;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Services.MovieGenreService;

namespace WEB_153502_FEDORINCHIK.Services.MovieService
{
    public class MemoryMovieService : IMovieService
    {
        private List<Movie> _movies = new();
        private List<MovieGenre> _genres = new();
        private IConfiguration _config;

        public MemoryMovieService([FromServices] IConfiguration config, IMovieGenreService movieGenreService)
        {
            _config = config;
            SetupData();
        }


        public Task<ResponseData<Movie>> CreateMovieAsync(Movie movie, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovieAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Movie>> GetMovieByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var filteredMovies =
                genreNormalizedName != null ?
                _movies.Where(movie => movie.Genre?.NormalizedName == genreNormalizedName).ToList() :
                _movies;

            int itemsPerPage = _config.GetValue<int>("ItemsPerPage");

            int totalCount = filteredMovies.Count();
            int totalPages = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / itemsPerPage) : 0;

            var responseData = new ResponseData<ListModel<Movie>>
            {
                Data = new ListModel<Movie>
                {
                    Items = filteredMovies.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages,
                }
            };

            return Task.FromResult(responseData);
        }


        public Task UpdateMovieAsync(int id, Movie movie, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        private void SetupData()
        {
            _movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 49.99m,
                    Path = "/images/princessdiary.jpg"
                },
                new Movie
                {
                    Id = 2,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 59.99m,
                    Path = "/images/princessdiary.jpg"
                },
                new Movie
                {
                    Id = 3,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 69.99m,
                    Path = "/images/princessdiary.jpg"
                },
                new Movie
                {
                    Id = 4,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 79.99m,
                    Path = "/images/princessdiary.jpg"
                },
                new Movie
                {
                    Id = 5,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 29.99m,
                    Path = "/images/princessdiary.jpg"
                },
                new Movie
                {
                    Id = 6,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 39.99m,
                    Path = "/images/princessdiary.jpg"
                },
                new Movie
                {
                    Id = 7,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 19.99m,
                    Path = "/images/princessdiary.jpg"
                },
                new Movie
                {
                    Id = 8,
                    Name = "Дневники принцессы",
                    Description = "любимка моя",
                    Genre = new MovieGenre { Id = 1, Name = "Любимые", NormalizedName = "love" },
                    Price = 20.99m,
                    Path = "/images/princessdiary.jpg"
                }
            };
        }
    }
}
