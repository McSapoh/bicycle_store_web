using bicycle_store_web.Services;
using Microsoft.AspNetCore.Mvc;

namespace bicycle_store_web.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserService userService;
        private readonly OrderService orderService;
        public OrderController(UserService userService, OrderService orderService)
        {
            this.userService = userService;
            this.orderService = orderService;
        }
        public IActionResult UserOrders() => View();
        public IActionResult AdminOrders() => View();
        [HttpGet]
        public IActionResult GetAdminOrders() =>
            orderService.GetAdminOrders();
        [HttpGet]
        public IActionResult GetUserOrders() => 
            orderService.GetUserOrders(userService.GetUserId(User.Identity.Name));
        [HttpPost]
        public IActionResult CreateOrder() => 
            orderService.CreateOrder(userService.GetUserId(User.Identity.Name));
        [HttpPost]
        public IActionResult SendOrder(int OrderId) => orderService.SendOrder(OrderId);
        [HttpPost]
        public IActionResult ConfirmReceipt(int OrderId) => orderService.ConfirmReceipt(OrderId);
    }
}
