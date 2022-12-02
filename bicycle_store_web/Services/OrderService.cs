using bicycle_store_web.Interfaces;
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
        private readonly BicycleService bicycleService;
        private readonly IOrderRepository _orderRepo;
        private readonly IBicycleRepository _bicycleRepo;
        private readonly IBicycleOrderRepository _bicycleOrderRepo;
        private readonly IShoppingCartOrderRepository _shoppingCartOrderRepo;
        public OrderService(BicycleService bicycleService, ShoppingCartService shoppingCartService, 
            IOrderRepository orderRepo, IBicycleRepository bicycleRepo,
            IBicycleOrderRepository bicycleOrderRepo, IShoppingCartOrderRepository shoppingCartOrderRepo)
        {
            this.shoppingCartService = shoppingCartService;
            this.bicycleService = bicycleService;
            _orderRepo = orderRepo;
            _bicycleRepo = bicycleRepo;
            _bicycleOrderRepo = bicycleOrderRepo;
            _shoppingCartOrderRepo = shoppingCartOrderRepo;
        }
        public IActionResult GetAdminOrders()
        {
            var UserOrders = _orderRepo.GetAll();
            var UserBicycleOrders = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                var bicycleOrders = _bicycleOrderRepo.GetAll(userOrder.OrderId);
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
            var UserOrders = _orderRepo.GetAll(UserId);
            var UserBicycleOrders = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                var bicycleOrders = _bicycleOrderRepo.GetAll(userOrder.OrderId);
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
        public bool CreateOrder(int UserId)
        {
            var ShoppingCartId = shoppingCartService.GetShoppingCartId(UserId);
            var CartOrders = _shoppingCartOrderRepo.GetAll(ShoppingCartId);
            var Order = new Order()
            {
                Data = DateTime.Now,
                UserId = UserId,
                Cost = GetTotalCost(CartOrders),
                Status = OrderStatus.Processing.ToString()
            };
            Order.OrderId = _orderRepo.GetMaxId();
            _orderRepo.Create(Order);
            foreach (var CartOrder in CartOrders)
            {
                var bicycle = bicycleService.GetBicycle(CartOrder.BicycleId);
                bicycle.Quantity -= (uint)CartOrder.Quantity;
                _bicycleRepo.Update(bicycle);
                _bicycleOrderRepo.Create(new BicycleOrder()
                {
                    BicycleId = CartOrder.BicycleId,
                    Quantity = CartOrder.Quantity,
                    BicycleCost = (int)(CartOrder.Quantity * CartOrder.Bicycle.Price),
                    OrderId = Order.OrderId,
                });
            }
            shoppingCartService.ClearShoppingCart(UserId);
            return true;
        }
        public bool SendOrder(int OrderId)
        {
            var order = _orderRepo.GetById(OrderId);
            order.Status = OrderStatus.Sended.ToString();
            _orderRepo.Update(order);
            if (_orderRepo.GetById(OrderId).Status == OrderStatus.Sended.ToString())
                return true;
            else
                return false;
        }
        public bool ConfirmReceipt(int OrderId)
        {
            var order = _orderRepo.GetById(OrderId);
            order.Status = OrderStatus.Received.ToString();
            _orderRepo.Update(order);
            if (_orderRepo.GetById(OrderId).Status == OrderStatus.Received.ToString())
                return true;
            else
                return false;
        }
    }
}
