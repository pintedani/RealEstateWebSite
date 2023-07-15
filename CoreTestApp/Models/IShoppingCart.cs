using Imobiliare.Entities;
using System.IO.Pipelines;

namespace Imobiliare.UI.Models
{
    public interface IShoppingCart
    {
        void AddToCart(Imobil pie);
        int RemoveFromCart(Imobil pie);
        List<ShoppingCartItem> GetShoppingCartItems();
        void ClearCart();
        decimal GetShoppingCartTotal();
        List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
