using bicycle_store_web.Migrations;
using bicycle_store_web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Services
{
    public class OrderService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        public int UserId { get; private set; }

        private readonly bicycle_storeContext _db;
        private readonly ShoppingCartOrderService shoppingCartOrderService;
        private readonly ShoppingCartService shoppingCartService;
        private readonly BicycleService bicycleService;
        private readonly UserService userService;
        public OrderService(IWebHostEnvironment WebHostEnvironment, bicycle_storeContext context,
            ShoppingCartOrderService shoppingCartOrderService, BicycleService bicycleService, 
            UserService userService, ShoppingCartService shoppingCartService)
        {
            this.userService = userService;
            this.shoppingCartOrderService = shoppingCartOrderService;
            this.shoppingCartService = shoppingCartService;
            this.bicycleService = bicycleService;
            this.WebHostEnvironment = WebHostEnvironment;
            _db = context;
        }
        public int GetTotalCost(List<ShoppingCartOrder> CartOrders)
        {
            int TotalCost = 0;
            foreach (var CartOrder in CartOrders)
                TotalCost += (int)(CartOrder.Quantity * CartOrder.Bicycle.Price);
            return TotalCost;
        }
        public IActionResult CreateOrder(int UserId)
        {
            var ShoppingCartId = shoppingCartService.GetShoppingCartId(UserId);
            var CartOrders = shoppingCartOrderService.GetListShoppingCartOrders(ShoppingCartId);
            var Order = new Order()
            {
                Data = DateTime.Now,
                UserId = UserId,
                Cost = GetTotalCost(CartOrders),
                Status = OrderStatus.Processing.ToString()
            };
            try
            {
                Order.OrderId = _db.Orders.Max(o => o.OrderId);
            }
            catch (Exception)
            {
                Order.OrderId = 1;
            }
            _db.Orders.Add(Order);
            foreach (var CartOrder in CartOrders)
            {
                _db.BicycleOrders.Add(new BicycleOrder()
                {
                    BicycleId = CartOrder.BicycleId,
                    Quantity = CartOrder.Quantity,
                    BicycleCost = (int)(CartOrder.Quantity * CartOrder.Bicycle.Price),
                    OrderId = Order.OrderId,
                });
            }
            shoppingCartService.ClearShoppingCart(UserId);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Іuccessfully purchased" });
        }
    }
}
