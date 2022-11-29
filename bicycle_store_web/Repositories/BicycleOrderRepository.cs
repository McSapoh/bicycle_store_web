using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Repositories
{
    public class BicycleOrderRepository : IBicycleOrderRepository
    {
        public readonly bicycle_storeContext _db;
        public BicycleOrderRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<BicycleOrder> GetAll() => _db.BicycleOrders.ToList();
        public List<BicycleOrder> GetAll(int OrderId) => _db.BicycleOrders.Include(b => b.Bicycle)
            .Where(bo => bo.OrderId == OrderId).ToList();
        public BicycleOrder GetById(int Id) => _db.BicycleOrders.FirstOrDefault(b => b.BicycleOrderId == Id);
        public bool Create(BicycleOrder item)
        {
            _db.BicycleOrders.Add(item);
            _db.SaveChanges();
            return true;
        }
        public bool Update(BicycleOrder item)
        {
            _db.BicycleOrders.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            _db.BicycleOrders.Remove(GetById(Id));
            _db.SaveChanges();
            return true;
        }
    }
}
