using bicycle_store_web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bicycle_store_web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService shoppingCartService;
        private readonly UserService userService;
        public ShoppingCartController(ShoppingCartService shoppingCartService, UserService userService)
        {
            this.shoppingCartService = shoppingCartService;
            this.userService = userService;
        }
        [Authorize]
        public IActionResult Index() => View();
        [HttpGet]
        public IActionResult GetShoppingCart() => 
            shoppingCartService.GetShoppingCart(userService.GetUserId(User.Identity.Name));
        public IActionResult AddToShoppingCart(int BicycleId) => 
            shoppingCartService.AddToShoppingCart(BicycleId, userService.GetUserId(User.Identity.Name));

        public IActionResult RemoveFromShoppingCart(int BicycleId) =>
            shoppingCartService.RemoveFromShoppingCart(BicycleId, userService.GetUserId(User.Identity.Name));
    }
}
