using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153502_FEDORINCHIK.API.Data;
using WEB_153502_FEDORINCHIK.Domain.Enities;
using WEB_153502_FEDORINCHIK.API.Services.MovieGenreService;
using WEB_153502_FEDORINCHIK.API.Services.MovieService;
using WEB_153502_FEDORINCHIK.Domain.Models;
using System.Linq.Expressions;

namespace WEB_153502_FEDORINCHIK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/Movies
        [HttpGet("{pageNo:int}")]
        [HttpGet("{movieGenre?}/{pageNo:int?}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(string? movieGenre, int pageNo = 1, int pageSize = 3)
        {
            var response = await _movieService.GetMovieListAsync(movieGenre, pageNo, pageSize);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // GET: api/Movies/movie-5
        [HttpGet("movie-{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var response = await _movieService.GetMovieByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            try
            {
                await _movieService.UpdateMovieAsync(id, movie);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Movie>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return Ok(new ResponseData<Movie>()
            {
                Data = movie,
                Success = true,
            });
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            var response = await _movieService.CreateMovieAsync(movie);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Movie>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return NoContent();
        }

        // POST: api/Tools/5
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _movieService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        private async Task<bool> MovieExists(int id)
        {
            return (await _movieService.GetMovieByIdAsync(id)).Success;
        }
    }
}