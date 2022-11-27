using bicycle_store_web.Migrations;
using bicycle_store_web.Models;
using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Services
{
    public class OrderService
    {
        private readonly ShoppingCartService shoppingCartService;
        private readonly ShoppingCartOrderRepository shoppingCartOrderRepo;
        private readonly BicycleService bicycleService;
        private readonly OrderRepository orderRepo;
        private readonly BicycleRepository bicycleRepo;
        private readonly BicycleOrderRepository bicycleOrderRepo;
        public OrderService(bicycle_storeContext context, BicycleService bicycleService, 
            ShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
            this.bicycleService = bicycleService;
            orderRepo = new OrderRepository(context);
            bicycleRepo = new BicycleRepository(context);
            bicycleOrderRepo = new BicycleOrderRepository(context);
            shoppingCartOrderRepo = new ShoppingCartOrderRepository(context);
        }
        public IActionResult GetAdminOrders()
        {
            var UserOrders = orderRepo.GetAll();
            var UserBicycleOrders = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                var bicycleOrders = bicycleOrderRepo.GetAll(userOrder.OrderId);
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
        public IActionResult GetUserOrders(int UserId)
        {
            var UserOrders = orderRepo.GetAll(UserId);
            var UserBicycleOrders = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                var bicycleOrders = bicycleOrderRepo.GetAll(userOrder.OrderId);
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
            var CartOrders = shoppingCartOrderRepo.GetAll(ShoppingCartId);
            var Order = new Order()
            {
                Data = DateTime.Now,
                UserId = UserId,
                Cost = GetTotalCost(CartOrders),
                Status = OrderStatus.Processing.ToString()
            };
            Order.OrderId = orderRepo.GetMaxId();
            orderRepo.Create(Order);
            foreach (var CartOrder in CartOrders)
            {
                var bicycle = bicycleService.GetBicycle(CartOrder.BicycleId);
                bicycle.Quantity -= (uint)CartOrder.Quantity;
                var res1 = bicycleRepo.Update(bicycle);
                var res2 = bicycleOrderRepo.Create(new BicycleOrder()
                {
                    BicycleId = CartOrder.BicycleId,
                    Quantity = CartOrder.Quantity,
                    BicycleCost = (int)(CartOrder.Quantity * CartOrder.Bicycle.Price),
                    OrderId = Order.OrderId,
                });
                if (res1 || res2 == false)
                    return new JsonResult(new { success = true, message = "Error while creating" });
            }
            shoppingCartService.ClearShoppingCart(UserId);
            return new JsonResult(new { success = true, message = "Order successfully created" });
        }
        public IActionResult SendOrder(int OrderId)
        {
            var order = orderRepo.GetById(OrderId);
            order.Status = OrderStatus.Sended.ToString();
            if (orderRepo.Update(order))
                return new JsonResult(new { success = true, message = "Successfully sended" });
            return new JsonResult(new { success = false, message = "Error while sending" });
        }
        public IActionResult ConfirmReceipt(int OrderId)
        {
            var order = orderRepo.GetById(OrderId);
            order.Status = OrderStatus.Received.ToString();
            if (orderRepo.Update(order))
                return new JsonResult(new { success = true, message = "Successfully sended" });
            return new JsonResult(new { success = false, message = "Error while sending" });
        }
    }
}
