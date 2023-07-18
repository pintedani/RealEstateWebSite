using Imobiliare.UI.Models;
using Imobiliare.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IImobilRepository _pieRepository;
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartController(IImobilRepository pieRepository, IShoppingCart shoppingCart)
        {
            _pieRepository = pieRepository;
            _shoppingCart = shoppingCart;

        }
        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            return View(shoppingCartViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int pieId)
        {
            var selectedPie = _pieRepository.AllImobils.FirstOrDefault(p => p.Id == pieId);

            if (selectedPie != null)
            {
                _shoppingCart.AddToCart(selectedPie);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int pieId)
        {
            var selectedPie = _pieRepository.AllImobils.FirstOrDefault(p => p.Id == pieId);

            if (selectedPie != null)
            {
                _shoppingCart.RemoveFromCart(selectedPie);
            }
            return RedirectToAction("Index");
        }
    }
}
