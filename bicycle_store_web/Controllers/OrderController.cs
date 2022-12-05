using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace bicycle_store_web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUserService userService;
        private readonly IOrderService orderService;
        public OrderController(IUserService userService, IOrderService orderService)
        {
            this.userService = userService;
            this.orderService = orderService;
        }
        public IActionResult UserOrders() => View();
        public IActionResult AdminOrders() => View();
        [HttpGet]
        public IActionResult GetAdminOrders()
        {
            var ResultList = orderService.GetAdminOrders().Select(o => new
            {
                o.OrderId,
                o.Bicycle.Name,
                o.Quantity,
                o.BicycleCost,
                o.Order.Status,
                o.Order.User.FullName
            }).ToList();
            return new JsonResult(new { data = ResultList });
        }
        [HttpGet]
        public IActionResult GetUserOrders()
        {
            var ResultList = orderService.GetUserOrders(userService.GetUserId(User.Identity.Name))
                .Select(o => new
                {
                    o.OrderId,
                    o.Bicycle.Name,
                    o.Quantity,
                    o.BicycleCost,
                    o.Order.Status,
                    o.Order.UserId,
                }).ToList();
            return new JsonResult(new { data = ResultList });
        }
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
