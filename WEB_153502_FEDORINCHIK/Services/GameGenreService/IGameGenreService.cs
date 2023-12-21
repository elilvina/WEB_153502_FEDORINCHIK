using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Domain.Entities;

namespace WEB_153502_FEDORINCHIK.Services.GameGenreService
{
    public interface IGameGenreService
    {
        public Task<ResponseData<List<GameGenre>>> GetGameGenreListAsync();
    }
}
