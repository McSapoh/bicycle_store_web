using bicycle_store_web.Models;
using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bicycle_store_web.Services
{
    public class ShoppingCartService
    {
        private readonly ShoppingCartOrderService shoppingCartOrderService;
        private readonly ShoppingCartRepository shoppingCartRepo;
        private readonly ShoppingCartOrderRepository shoppingCartOrderRepo;
        public ShoppingCartService(bicycle_storeContext context, ShoppingCartOrderService shoppingCartOrderService)
        {
            this.shoppingCartOrderService = shoppingCartOrderService;
            shoppingCartRepo = new ShoppingCartRepository(context);
            shoppingCartOrderRepo = new ShoppingCartOrderRepository(context);
        }
        public void CreateShopingCart(int UserId) =>
            shoppingCartRepo.Create(new ShoppingCart() { UserId = UserId });
        public IActionResult AddToShoppingCart(int BicycleId, int UserId) => 
            shoppingCartOrderService.SaveShoppingCartOrder(BicycleId, GetShoppingCartId(UserId));
        public IActionResult GetShoppingCart(int UserId) => 
            shoppingCartOrderService.GetShoppingCartOrders(GetShoppingCartId(UserId));
        public int GetShoppingCartId(int UserId) =>
            shoppingCartRepo.GetShoppingCartId(UserId);
        public IActionResult RemoveFromShoppingCart(int BicycleId, int UserId) => 
            shoppingCartOrderService.DeleteShoppingCartOrder(BicycleId, GetShoppingCartId(UserId));
        public void ClearShoppingCart(int UserId)
        { 
            var cartOrders = shoppingCartOrderRepo.GetAll(shoppingCartRepo.GetShoppingCartId(UserId));
            foreach (var cartOrder in cartOrders)
                shoppingCartOrderService.DeleteShoppingCartOrder(cartOrder.BicycleId, cartOrder.ShoppingCartId);
        }
    }
}
