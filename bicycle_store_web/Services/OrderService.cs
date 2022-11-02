using bicycle_store_web.Migrations;
using bicycle_store_web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public Order GetOrder(int OrderId) => _db.Orders.FirstOrDefault(o => o.OrderId == OrderId);
        public IActionResult GetUserOrders(int UserId)
        {
            var UserOrders = _db.Orders.Where(o => o.UserId == UserId).ToList();
            var UserBicycleOrders = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                var bicycleOrders = _db.BicycleOrders.Include(b => b.Bicycle)
                    .Where(bo => bo.OrderId == userOrder.OrderId).ToList();
                foreach (var bicycleOrder in bicycleOrders)
                {
                    UserBicycleOrders.Add(bicycleOrder);
                }
            }
            var ResultList = UserBicycleOrders.Select(o => new
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
        public IActionResult GetAdminOrders()
        {
            var UserOrders = _db.Orders.Include(o => o.User).ToList();
            var UserBicycleOrders = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                var bicycleOrders = _db.BicycleOrders.Include(b => b.Bicycle)
                    .Where(bo => bo.OrderId == userOrder.OrderId).ToList();
                foreach (var bicycleOrder in bicycleOrders)
                {
                    UserBicycleOrders.Add(bicycleOrder);
                }
            }
            var ResultList = UserBicycleOrders.Select(o => new
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
                Order.OrderId = _db.Orders.Max(o => o.OrderId) + 1;
            }
            catch (Exception)
            {
                Order.OrderId = 1;
            }
            _db.Orders.Add(Order);
            foreach (var CartOrder in CartOrders)
            {
                var bicycle = bicycleService.GetBicycle(CartOrder.BicycleId);
                bicycle.Quantity -= (uint)CartOrder.Quantity;
                _db.Bicycles.Update(bicycle);
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
            return new JsonResult(new { success = true, message = "Successfully purchased" });
        }
        public IActionResult SendOrder(int OrderId)
        {
            var order = GetOrder(OrderId);
            order.Status = OrderStatus.Sended.ToString();
            _db.Orders.Update(order);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Successfully sended" });
        }
        public IActionResult ConfirmReceipt(int OrderId)
        {
            var order = GetOrder(OrderId);
            order.Status = OrderStatus.Received.ToString();
            _db.Orders.Update(order);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Successfully sended" });
        }
    }
}
