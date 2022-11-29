using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public readonly bicycle_storeContext _db;
        public ShoppingCartRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<ShoppingCart> GetAll() => throw new NotImplementedException();
        public ShoppingCart GetById(int Id) => _db.ShopingCarts.FirstOrDefault(o => o.Id == Id);
        public int GetShoppingCartId(int Id) => _db.ShopingCarts.FirstOrDefault(o => o.UserId == Id).Id;
        public bool Create(ShoppingCart item)
        {
            _db.ShopingCarts.Add(item);
            _db.SaveChanges();
            return true;
        }
        public bool Update(ShoppingCart item)
        {
            _db.ShopingCarts.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            _db.ShopingCarts.Remove(GetById(Id));
            _db.SaveChanges();
            return true;
        }
    }
}
