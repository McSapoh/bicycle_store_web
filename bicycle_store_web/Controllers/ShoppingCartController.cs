using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;

namespace bicycle_store_web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IUserService userService;
        public ShoppingCartController(IShoppingCartService shoppingCartService, IUserService userService)
        {
            this.shoppingCartService = shoppingCartService;
            this.userService = userService;
        }
        [Authorize]
        public IActionResult Index() => View();
        [HttpGet]
        public IActionResult GetShoppingCart()
        {
			Log.Information($"{System.Reflection.MethodBase.GetCurrentMethod()} started");

			var list = shoppingCartService.GetShoppingCart(userService.GetUserId(User.Identity.Name)).
                Select(o => new
                {
                    o.Id,
                    o.Bicycle.Name,
                    o.Bicycle.Price,
                    o.Quantity
                }).ToList();
			Log.Information($"{System.Reflection.MethodBase.GetCurrentMethod()} finished");

			return new JsonResult(new { data = list });
        }
        public IActionResult AddToShoppingCart(int BicycleId)
        {
			Log.Information($"{System.Reflection.MethodBase.GetCurrentMethod()} started");

            JsonResult result;
            bool serviceResult = shoppingCartService.AddToShoppingCart(BicycleId, userService.GetUserId(User.Identity.Name));
			if (serviceResult)
                result = new JsonResult(new { success = true, message = "Successfully added to shopping cart" });
            else
                result = new JsonResult(new { success = false, message = "Error while adding to shopping cart" });

			Log.Information($"{System.Reflection.MethodBase.GetCurrentMethod()} finished");

			return result;
        }
        public IActionResult RemoveFromShoppingCart(int ShoppingCartOrderId)
        {
			Log.Information($"{System.Reflection.MethodBase.GetCurrentMethod()} started");

			JsonResult result;
			bool serviceResult = shoppingCartService.RemoveFromShoppingCart(ShoppingCartOrderId);
			if (serviceResult)
				result = new JsonResult(new { success = true, message = "Successfully removed from shopping cart" });
            else
				result = new JsonResult(new { success = false, message = "Error while removing from shopping cart" });

			Log.Information($"{System.Reflection.MethodBase.GetCurrentMethod()} finished");

			return result;
        }
    }
}
