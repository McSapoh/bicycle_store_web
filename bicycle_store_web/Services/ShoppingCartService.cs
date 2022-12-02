using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bicycle_store_web.Services
{
    public class ShoppingCartService
    {
        private readonly ShoppingCartOrderService _shoppingCartOrderService;
        private readonly IShoppingCartRepository _shoppingCartRepo;
        private readonly IShoppingCartOrderRepository _shoppingCartOrderRepo;
        public ShoppingCartService(ShoppingCartOrderService shoppingCartOrderService,
            IShoppingCartRepository shoppingCartRepo,
            IShoppingCartOrderRepository shoppingCartOrderRepo)
        {
            _shoppingCartOrderService = shoppingCartOrderService;
            _shoppingCartRepo = shoppingCartRepo;
            _shoppingCartOrderRepo = shoppingCartOrderRepo;
        }
        public void CreateShopingCart(int UserId) =>
            _shoppingCartRepo.Create(new ShoppingCart() { UserId = UserId });
        public bool AddToShoppingCart(int BicycleId, int UserId)
        {
            _shoppingCartOrderService.SaveShoppingCartOrder(BicycleId, GetShoppingCartId(UserId));
            return true;
        }
        public IActionResult GetShoppingCart(int UserId) => 
            _shoppingCartOrderService.GetShoppingCartOrders(GetShoppingCartId(UserId));
        public int GetShoppingCartId(int UserId) =>
            _shoppingCartRepo.GetShoppingCartId(UserId);
        public bool RemoveFromShoppingCart(int ShoppingCartOrderId)
        {
            _shoppingCartOrderService.DeleteShoppingCartOrder(ShoppingCartOrderId);
            return true;
        }

        public void ClearShoppingCart(int UserId)
        { 
            var cartOrders = _shoppingCartOrderRepo.GetAll(_shoppingCartRepo.GetShoppingCartId(UserId));
            foreach (var cartOrder in cartOrders)
                _shoppingCartOrderService.DeleteShoppingCartOrder(cartOrder.Id);
        }
    }
}
