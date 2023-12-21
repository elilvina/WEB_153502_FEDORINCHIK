using WEB_153502_FEDORINCHIK.Domain.Entities;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Extensions;

namespace WEB_153502_FEDORINCHIK.Services.CartService
{
    public class SessionCart : Cart
    {
        private readonly ISession _session;

        public SessionCart(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext!.Session;

            Cart? sessionCart = _session.Get<Cart>(nameof(Cart));

            if (sessionCart is not null)
                base.CartItems = sessionCart.CartItems;
        }

        public override void AddToCart(Game game)
        {
            base.AddToCart(game);
            _session.Set<Cart>(nameof(Cart), this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            _session.Set(nameof(Cart), this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            _session.Remove(nameof(Cart));
        }
    }
}
