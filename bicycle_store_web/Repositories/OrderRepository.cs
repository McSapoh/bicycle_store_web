using bicycle_store_web.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public readonly bicycle_storeContext _db;
        public OrderRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<Order> GetAll() => _db.Orders.Include(o => o.User).ToList();
        public List<Order> GetAll(int Id) => _db.Orders.Where(o => o.UserId == Id).ToList();
        public Order GetById(int Id) => _db.Orders.FirstOrDefault(o => o.OrderId == Id);
        public int GetMaxId()
        {
            int id;
            try
            { id = _db.Orders.Max(o => o.OrderId) + 1; }
            catch (Exception)
            { id = 1; }
            return id;
        }
        public bool Create(Order item)
        {
            _db.Orders.Add(item);
            _db.SaveChanges();
            return true;
        }
        public bool Update(Order item)
        {
            _db.Orders.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            _db.Orders.Remove(GetById(Id));
            _db.SaveChanges();
            return true;
        }
    }
}
