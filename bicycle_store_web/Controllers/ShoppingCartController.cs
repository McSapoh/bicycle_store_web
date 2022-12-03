using bicycle_store_web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public IActionResult GetShoppingCart()
        {
            var list = shoppingCartService.GetShoppingCart(userService.GetUserId(User.Identity.Name)).
                Select(o => new
                {
                    o.Id,
                    o.Bicycle.Name,
                    o.Bicycle.Price,
                    o.Quantity
                }).ToList();
            return new JsonResult(new { data = list });
        }
        public IActionResult AddToShoppingCart(int BicycleId)
        {
            if(shoppingCartService.AddToShoppingCart(BicycleId, userService.GetUserId(User.Identity.Name)))
                return new JsonResult(new { success = true, message = "Successfully added to shopping cart" });
            else
                return new JsonResult(new { success = false, message = "Error while adding to shopping cart" });
        }
        public IActionResult RemoveFromShoppingCart(int ShoppingCartOrderId)
        {
            if (shoppingCartService.RemoveFromShoppingCart(ShoppingCartOrderId))
                return new JsonResult(new { success = true, message = "Successfully removed from shopping cart" });
            else
                return new JsonResult(new { success = false, message = "Error while removing from shopping cart" });
        }
    }
}
