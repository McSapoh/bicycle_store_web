using bicycle_store_web.Models;
using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace bicycle_store_web.Services
{
    public class ShoppingCartOrderService
    {
        private readonly BicycleService bicycleService;
        private readonly ShoppingCartOrderRepository shoppingCartOrderRepo;
        public ShoppingCartOrderService(bicycle_storeContext context, BicycleService bicycleService)
        {
            this.bicycleService = bicycleService;
            shoppingCartOrderRepo = new ShoppingCartOrderRepository(context);
        }
        public IActionResult GetShoppingCartOrders(int ShoppingCartId)
        {
            var list = shoppingCartOrderRepo.GetAll(ShoppingCartId).Select(o => new
            {
                o.Id, o.Bicycle.Name,
                o.Bicycle.Price, o.Quantity
            }).ToList();
            return new JsonResult(new { data = list });
        }
        public IActionResult SaveShoppingCartOrder(int BicycleId, int ShoppingCartId)
        {
            if (shoppingCartOrderRepo.CheckExistence(ShoppingCartId) == false)
                return new JsonResult(new { success = false, message = "Shopping cart does not exist" }); ;
            bool result;
            if (shoppingCartOrderRepo.CheckExistence(ShoppingCartId, BicycleId))
            {
                var shoppingCartOrder = shoppingCartOrderRepo.GetById(ShoppingCartId, BicycleId);
                if (bicycleService.GetBicycle(BicycleId).Quantity > shoppingCartOrder.Quantity)
                    shoppingCartOrder.Quantity++;
                else
                    return new JsonResult(new { success = false, message = "Cannot add more goods than we have" });
                result = shoppingCartOrderRepo.Update(shoppingCartOrder);
            }
            else
            {
                var shoppingCartOrder = new ShoppingCartOrder()
                {
                    BicycleId = BicycleId,
                    Quantity = 1,
                    ShoppingCartId = ShoppingCartId
                };
                result = shoppingCartOrderRepo.Create(shoppingCartOrder);
            }
           if (result)
                return new JsonResult(new { success = true, message = "Successfully saved" });
            return new JsonResult(new { success = false, message = "Error while saving" });
        }
        public IActionResult DeleteShoppingCartOrder(int BicycleId, int ShoppingCartId)
        {
            var shoppingCartOrder = shoppingCartOrderRepo.GetById(ShoppingCartId, BicycleId);
            if (shoppingCartOrderRepo.Delete(shoppingCartOrder.Id))
                return new JsonResult(new { success = true, message = "Successfully deleted" });
            return new JsonResult(new { success = false, message = "Error while Deleting" });
        }
    }
}
