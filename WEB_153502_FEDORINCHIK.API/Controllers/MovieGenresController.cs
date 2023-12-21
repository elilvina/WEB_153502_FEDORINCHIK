using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153502_FEDORINCHIK.API.Data;
using WEB_153502_FEDORINCHIK.Domain.Enities;
using WEB_153502_FEDORINCHIK.API.Services.MovieGenreService;
using WEB_153502_FEDORINCHIK.Domain.Models;

namespace WEB_153502_FEDORINCHIK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieGenresController : ControllerBase
    {
        private readonly IMovieGenreService _movieGenreService;

        public MovieGenresController(IMovieGenreService movieGenreService)
        {
            _movieGenreService = movieGenreService;
        }

        // GET: api/MovieGenres
        [HttpGet]
        public async Task<ActionResult<ResponseData<MovieGenre>>> GetMovieGenres()
        {
            var responce = await _movieGenreService.GetMovieGenreListAsync();
            return responce.Success ? Ok(responce) : BadRequest(responce);
        }
    }
}
