using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Extensions;
using WEB_153502_FEDORINCHIK.Services.GameService;

namespace WEB_153502_FEDORINCHIK.Controllers
{
    public class CartController : Controller
    {
        private readonly IGameService _gameService;
        private readonly Cart _cart;
        public CartController(IGameService gameService, Cart cart)
        {
            _gameService = gameService;
            _cart = cart;
        }


        [Authorize]
        [Route("[controller]")]
        public IActionResult Index()
        {
            return View(_cart.CartItems);
        }


        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            var response = await _gameService.GetGameByIdAsync(id);
            if (response.Success)
            {
                _cart.AddToCart(response.Data!);
            }

            return Redirect(returnUrl);
        }


        [Authorize]
        [Route("[controller]/remove/{id:int}")]
        public async Task<ActionResult> Remove(int id, string returnUrl)
        {
            await Task.Run(() => _cart.RemoveItems(id));
            return Redirect(returnUrl);
        }
    }
}
