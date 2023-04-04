using CoreTestApp.Models;
using CoreTestApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoreTestApp.Components
{
    /// <summary>
    /// This is a reusable component which has it's own data, injected with DI
    /// It is an "uppgrade" of partial view(which gets the data from the surrounding view)
    /// </summary>
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartSummary(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            //var items = new List<ShoppingCartItem>() { new ShoppingCartItem(), new ShoppingCartItem() };

            var items = _shoppingCart.GetShoppingCartItems();
            //_shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            return View(shoppingCartViewModel);
        }
    }
}
