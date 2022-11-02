using bicycle_store_web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Services
{
    public class ShoppingCartService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        private readonly bicycle_storeContext _db;
        private readonly ShoppingCartOrderService shoppingCartOrderService;
        public ShoppingCartService(IWebHostEnvironment WebHostEnvironment, bicycle_storeContext context,
            ShoppingCartOrderService shoppingCartOrderService)
        {
            this.shoppingCartOrderService = shoppingCartOrderService;
            this.WebHostEnvironment = WebHostEnvironment;
            _db = context;
        }
        public void CreateShopingCart(int UserId) => 
            _db.ShopingCarts.Add(new ShoppingCart() { UserId = UserId });

        public IActionResult AddToShoppingCart(int BicycleId, int UserId) => 
            shoppingCartOrderService.AddShoppingCartOrder(BicycleId, GetShoppingCartId(UserId));
        public IActionResult GetShoppingCart(int UserId) =>
            shoppingCartOrderService.GetShoppingCartOrders(GetShoppingCartId(UserId));
        public int GetShoppingCartId(int UserId) => 
            _db.ShopingCarts.FirstOrDefault(o => o.UserId == UserId).Id;
        public IActionResult RemoveFromShoppingCart(int BicycleId, int UserId) => 
            shoppingCartOrderService.DeleteShoppingCartOrder(BicycleId, GetShoppingCartId(UserId));
        public void ClearShoppingCart(int UserId)
        {
            var CartId = GetShoppingCartId(UserId);
            var cartOrders = _db.ShoppingCartOrders.AsNoTracking().Where(o => o.ShoppingCartId == CartId).ToList();
            foreach (var cartOrder in cartOrders)
            {
                _db.ShoppingCartOrders.Remove(cartOrder);
            }
            
        }
    }
}
