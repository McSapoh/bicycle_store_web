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
        public IActionResult SaveShoppingCartOrder(int BicycleId, int ShoppingCartId)
        {
            if (_shoppingCartOrderRepo.CheckExistence(ShoppingCartId) == false)
                return new JsonResult(new { success = false, message = "Shopping cart does not exist" }); ;
            bool result;
            if (_shoppingCartOrderRepo.CheckExistence(ShoppingCartId, BicycleId))
            {
                var shoppingCartOrder = _shoppingCartOrderRepo.GetById(ShoppingCartId, BicycleId);
                if (_bicycleService.GetBicycle(BicycleId).Quantity > shoppingCartOrder.Quantity)
                    shoppingCartOrder.Quantity++;
                else
                    return new JsonResult(new { success = false, message = "Cannot add more goods than we have" });
                result = _shoppingCartOrderRepo.Update(shoppingCartOrder);
            }
            else
            {
                var shoppingCartOrder = new ShoppingCartOrder()
                {
                    BicycleId = BicycleId,
                    Quantity = 1,
                    ShoppingCartId = ShoppingCartId
                };
                result = _shoppingCartOrderRepo.Create(shoppingCartOrder);
            }
           if (result)
                return new JsonResult(new { success = true, message = "Successfully saved" });
            return new JsonResult(new { success = false, message = "Error while saving" });
        }
        public IActionResult DeleteShoppingCartOrder(int BicycleId, int ShoppingCartId)
        {
            var shoppingCartOrder = _shoppingCartOrderRepo.GetById(ShoppingCartId, BicycleId);
            if (_shoppingCartOrderRepo.Delete(shoppingCartOrder.Id))
                return new JsonResult(new { success = true, message = "Successfully deleted" });
            return new JsonResult(new { success = false, message = "Error while Deleting" });
        }
    }
}
