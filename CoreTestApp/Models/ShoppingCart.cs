using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;

namespace Imobiliare.UI.Models
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly ApplicationDbContext _bethanysImobilShopDbContext;

        public string? ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        private ShoppingCart(ApplicationDbContext bethanysImobilShopDbContext)
        {
            _bethanysImobilShopDbContext = bethanysImobilShopDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            ApplicationDbContext context = services.GetService<ApplicationDbContext>() ?? throw new Exception("Error initializing");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Imobil pie)
        {
            var shoppingCartItem =
                    _bethanysImobilShopDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Imobil.Id == pie.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Imobil = pie,
                    Amount = 1
                };

                _bethanysImobilShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _bethanysImobilShopDbContext.SaveChanges();
        }

        public int RemoveFromCart(Imobil pie)
        {
            var shoppingCartItem =
                    _bethanysImobilShopDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Imobil.Id == pie.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _bethanysImobilShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _bethanysImobilShopDbContext.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??=
                       _bethanysImobilShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Imobil)
                           .ToList();
        }

        public void ClearCart()
        {
            var cartItems = _bethanysImobilShopDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _bethanysImobilShopDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _bethanysImobilShopDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _bethanysImobilShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Imobil.Price * c.Amount).Sum();
            return total;
        }
    }
}
