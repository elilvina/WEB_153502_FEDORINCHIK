using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153502_FEDORINCHIK.API.Data;
using WEB_153502_FEDORINCHIK.API.Services.GameGenreService;
using WEB_153502_FEDORINCHIK.API.Services.GameServices;
using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;

namespace WEB_153502_FEDORINCHIK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService, IHttpContextAccessor httpContextAccessor)
        {
            _gameService = gameService;

            var _httpContext = httpContextAccessor.HttpContext!;
            var token = _httpContext.GetTokenAsync("access_token").Result;
        }

        // GET: api/Games
        [HttpGet("{pageNo:int}")]
        [HttpGet("{gameGenre?}/{pageNo:int?}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(string? gameGenre, int pageNo = 1, int pageSize = 3)
        {
            var response = await _gameService.GetGameListAsync(gameGenre, pageNo, pageSize);
            return response.Success ? Ok(response) : BadRequest(response);
        }


        // GET: api/Games/game-5
        [HttpGet("game-{id}")]
        [Authorize]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var response = await _gameService.GetGameByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // PUT: api/Games/game-5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("game-{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            try
            {
                await _gameService.UpdateGameAsync(id, game);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Game>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return Ok(new ResponseData<Game>()
            {
                Data = game,
                Success = true,
            });
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            var response = await _gameService.CreateGameAsync(game);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/Games/game-5
        [Authorize]
        [HttpDelete("game-{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                await _gameService.DeleteGameAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Game>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return NoContent();
        }

        // POST: api/Tools/5
        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _gameService.SaveImageAsync(id, formFile);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }

        private async Task<bool> GameExists(int id)
        {
            return (await _gameService.GetGameByIdAsync(id)).Success;
        }
    }
}
