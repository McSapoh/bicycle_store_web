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
        public IActionResult CreateOrder()
        {
            if(orderService.CreateOrder(userService.GetUserId(User.Identity.Name)))
                return new JsonResult(new { success = true, message = "Order successfully created" });
            else
                return new JsonResult(new { success = false, message = "Error while creating order" });
        }
        [HttpPost]
        public IActionResult SendOrder(int OrderId)
        {
            if (orderService.SendOrder(OrderId))
                return new JsonResult(new { success = true, message = "Successfully sended" });
            else
                return new JsonResult(new { success = false, message = "Error while sending" });
        }
        [HttpPost]
        public IActionResult ConfirmReceipt(int OrderId)
        {
            if (orderService.ConfirmReceipt(OrderId))
                return new JsonResult(new { success = true, message = "Successfully confirmed" });
            else
                return new JsonResult(new { success = false, message = "Error while confirming" });
        }
    }
}
