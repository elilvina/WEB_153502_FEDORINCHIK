using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Extensions;

namespace WEB_153502_FEDORINCHIK.Components
{
    public class CartViewComponent: ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
