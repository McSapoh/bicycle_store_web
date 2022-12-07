using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using System.Collections.Generic;

namespace bicycle_store_web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartOrderService _shoppingCartOrderService;
        private readonly IShoppingCartRepository _shoppingCartRepo;
        public ShoppingCartService(IShoppingCartOrderService shoppingCartOrderService,
            IShoppingCartRepository shoppingCartRepo)
        {
            _shoppingCartOrderService = shoppingCartOrderService;
            _shoppingCartRepo = shoppingCartRepo;
        }
        public void CreateShopingCart(int UserId) =>
            _shoppingCartRepo.Create(new ShoppingCart() { UserId = UserId });
        public bool AddToShoppingCart(int BicycleId, int UserId) => 
            _shoppingCartOrderService.SaveShoppingCartOrder(BicycleId, GetShoppingCartId(UserId));
        public List<ShoppingCartOrder> GetShoppingCart(int UserId) => 
            _shoppingCartOrderService.GetShoppingCartOrders(GetShoppingCartId(UserId));
        public int GetShoppingCartId(int UserId) =>
            _shoppingCartRepo.GetShoppingCartId(UserId);
        public bool RemoveFromShoppingCart(int ShoppingCartOrderId) => 
            _shoppingCartOrderService.DeleteShoppingCartOrder(ShoppingCartOrderId);
        public void ClearShoppingCart(int UserId)
        { 
            var cartOrders = _shoppingCartOrderService.GetShoppingCartOrders
                (_shoppingCartRepo.GetShoppingCartId(UserId));
            foreach (var cartOrder in cartOrders)
                _shoppingCartOrderService.DeleteShoppingCartOrder(cartOrder.Id);
        }
    }
}
