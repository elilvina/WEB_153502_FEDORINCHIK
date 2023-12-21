using Microsoft.AspNetCore.Mvc;

namespace WEB_153502_FEDORINCHIK.ViewComponents
{
    public class CartComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
