using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Repositories
{
    public class ShoppingCartOrderRepository : IShoppingCartOrderRepository
    {
        public readonly bicycle_storeContext _db;
        public ShoppingCartOrderRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<ShoppingCartOrder> GetAll() => _db.ShoppingCartOrders.ToList();
        public List<ShoppingCartOrder> GetAll(int ShoppingCartId) => 
            _db.ShoppingCartOrders.AsNoTracking()
                .Include(o => o.Bicycle).Where(o => o.ShoppingCartId == ShoppingCartId).ToList();
        public ShoppingCartOrder GetById(int Id) => _db.ShoppingCartOrders.FirstOrDefault(b => b.Id == Id);
        public ShoppingCartOrder GetById(int Id, int BicycleId) => 
            _db.ShoppingCartOrders.FirstOrDefault(o => o.ShoppingCartId == Id && o.BicycleId == BicycleId);
        public void Create(ShoppingCartOrder item)
        {
            _db.ShoppingCartOrders.Add(item);
            _db.SaveChanges();
        }
        public void Update(ShoppingCartOrder item)
        {
            _db.ShoppingCartOrders.Update(item);
            _db.SaveChanges();
        }
        public void Delete(int Id)
        {
            _db.ShoppingCartOrders.Remove(GetById(Id));
            _db.SaveChanges();
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
            }
            catch (Exception) { }
            return false;
        }
    }
}
