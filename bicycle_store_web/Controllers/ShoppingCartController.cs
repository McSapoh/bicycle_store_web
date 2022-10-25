using bicycle_store_web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly bicycle_storeContext _db;
        private readonly ShoppingCartService shoppingCartService;
        private readonly OrderService orderService;
        private readonly UserService userService;
        private readonly BicycleService bicycleService;
        public ShoppingCartController(ILogger<ShoppingCartController> logger, bicycle_storeContext context,
            ShoppingCartService shoppingCartService, UserService userService, BicycleService bicycleService, 
            OrderService orderService)
        {
            _logger = logger;
            _db = context;
            this.shoppingCartService = shoppingCartService;
            this.userService = userService;
            this.bicycleService = bicycleService;
            this.orderService = orderService;
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
