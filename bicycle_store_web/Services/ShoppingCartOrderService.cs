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
    public class ShoppingCartOrderService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        private readonly bicycle_storeContext _db;
        private readonly BicycleService bicycleService;

        public ShoppingCartOrderService(IWebHostEnvironment WebHostEnvironment, bicycle_storeContext context,
            BicycleService bicycleService)
        {
            this.bicycleService = bicycleService;
            this.WebHostEnvironment = WebHostEnvironment;
            _db = context;
        }
        public IActionResult GetShoppingCartOrders(int ShoppingCartId)
        {
            var l = _db.ShoppingCartOrders.Where(o => o.ShoppingCartId == ShoppingCartId);
            var list = l.Select(o => new
            {
                o.Id, o.Bicycle.Name, o.Bicycle.Price, o.Quantity
            }).ToList();
            return new JsonResult(new { data = list });
        }
        public List<ShoppingCartOrder> GetListShoppingCartOrders(int ShoppingCartId)
        {
            var l = _db.ShoppingCartOrders.AsNoTracking().Include(b => b.Bicycle)
                .Where(o => o.ShoppingCartId == ShoppingCartId);
            return l.ToList();
        }
        public IActionResult AddShoppingCartOrder(int BicycleId, int ShoppingCartId)
        {
            if (CheckExistence(ShoppingCartId) == false)
                return new JsonResult(new { success = false, message = "Error while adding" });
            var bicycle = bicycleService.GetBicycle(BicycleId);
            ShoppingCartOrder shoppingCartOrder;
            try
            {
                shoppingCartOrder = _db.ShoppingCartOrders.First(o => o.ShoppingCartId == ShoppingCartId && o.BicycleId == BicycleId);
                if (bicycleService.GetBicycle(BicycleId).Quantity > shoppingCartOrder.Quantity)
                    shoppingCartOrder.Quantity++;
                else
                    return new JsonResult(new 
                        { success = false, message = "Cannot add more bicycles, then we have in the store" });
                _db.ShoppingCartOrders.Update(shoppingCartOrder);
            }
            catch (Exception)
            {
                shoppingCartOrder = new ShoppingCartOrder()
                {
                    BicycleId = BicycleId,
                    Quantity = 1,
                    ShoppingCartId = ShoppingCartId
                };
                _db.ShoppingCartOrders.Add(shoppingCartOrder);
            }
            _db.SaveChanges();
            if (CheckExistence(ShoppingCartId, BicycleId))
                return new JsonResult(new { success = true, message = "Successfully added" });
            else
                return new JsonResult(new { success = false, message = "Error while adding" });

        }
        public void ChangeQuantity(int ShoppingCartId, int Quantity)
        {
            var shoppingCartOrder = _db.ShoppingCartOrders.FirstOrDefault(o => o.ShoppingCartId == ShoppingCartId);
            shoppingCartOrder.Quantity = Quantity;
            _db.ShoppingCartOrders.Update(shoppingCartOrder);
            _db.SaveChanges();
        }
        public IActionResult DeleteShoppingCartOrder(int BicycleId, int ShoppingCartId)
        {
            ShoppingCartOrder shoppingCartOrder;
            try
            {
                shoppingCartOrder = _db.ShoppingCartOrders.First(o => o.ShoppingCartId == ShoppingCartId && o.BicycleId == BicycleId);
                _db.ShoppingCartOrders.Remove(shoppingCartOrder);
            }
            catch (Exception) { 
                return new JsonResult(new { success = false, message = "Error while Deleting" }); 
            }
            _db.SaveChanges();
            if (CheckExistence(ShoppingCartId, BicycleId))
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            else
                return new JsonResult(new { success = true, message = "Successfully deleted" });
        }
        public bool CheckExistence(int ShoppingCartId)
        {
            try
            {
                if (_db.ShopingCarts.First(o => o.Id == ShoppingCartId) != null)
                    return true;
            }
            catch (Exception) { }
            return false;
        }
        public bool CheckExistence(int ShoppingCartId, int BicycleId)
        {
            try
            {
                if (_db.ShoppingCartOrders.First(o => o.ShoppingCartId == ShoppingCartId && o.BicycleId == BicycleId) != null)
                    return true;
            } catch (Exception) { }
            return false;
        }
        public bool CheckExistence(int ShoppingCartId, int BicycleId, int Quantity)
        {
            try
            {
                if (_db.ShoppingCartOrders.First(o => o.ShoppingCartId == ShoppingCartId && 
                    o.BicycleId == BicycleId &&
                    o.Quantity == Quantity) != null)
                    return true;
            }
            catch (Exception) { }
            return false;
        }
    }
}
