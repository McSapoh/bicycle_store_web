using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace bicycle_store_web.Services
{
    public class ShoppingCartOrderService
    {
        private readonly BicycleService _bicycleService;
        private readonly IShoppingCartOrderRepository _shoppingCartOrderRepo;
        public ShoppingCartOrderService(IShoppingCartOrderRepository shoppingCartOrderRepo, BicycleService bicycleService)
        {
            _bicycleService = bicycleService;
            _shoppingCartOrderRepo = shoppingCartOrderRepo;
        }
        public IActionResult GetShoppingCartOrders(int ShoppingCartId)
        {
            var list = _shoppingCartOrderRepo.GetAll(ShoppingCartId).Select(o => new
            {
                o.Id, o.Bicycle.Name,
                o.Bicycle.Price, o.Quantity
            }).ToList();
            return new JsonResult(new { data = list });
        }
        public bool SaveShoppingCartOrder(int BicycleId, int ShoppingCartId)
        {
            if (_shoppingCartOrderRepo.CheckExistence(ShoppingCartId) == false)
                return false;
            if (_shoppingCartOrderRepo.CheckExistence(ShoppingCartId, BicycleId))
            {
                var shoppingCartOrder = _shoppingCartOrderRepo.GetById(ShoppingCartId, BicycleId);
                if (_bicycleService.GetBicycle(BicycleId).Quantity > shoppingCartOrder.Quantity)
                    shoppingCartOrder.Quantity++;
                else
                    return false;
                _shoppingCartOrderRepo.Update(shoppingCartOrder);
            }
            else
            {
                var shoppingCartOrder = new ShoppingCartOrder()
                {
                    BicycleId = BicycleId,
                    Quantity = 1,
                    ShoppingCartId = ShoppingCartId
                };
                _shoppingCartOrderRepo.Create(shoppingCartOrder);
            }
            return true;
        }
        public bool DeleteShoppingCartOrder(int ShoppingCartOrderId)
        {
            var shoppingCartOrder = _shoppingCartOrderRepo.GetById(ShoppingCartOrderId);
            _shoppingCartOrderRepo.Delete(shoppingCartOrder.Id);
            if (_shoppingCartOrderRepo.GetById(ShoppingCartOrderId) != null)
                return true;
            else
                return false;
        }
    }
}
