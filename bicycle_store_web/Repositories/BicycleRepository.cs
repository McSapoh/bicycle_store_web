using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class BicycleRepository : IGenericRepository<Bicycle>
    {
        public readonly bicycle_storeContext _db;
        public BicycleRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<Bicycle> GetAll() => _db.Bicycles.ToList();
        public Bicycle GetById(int Id) => _db.Bicycles.FirstOrDefault(b => b.Id == Id);
        public bool Create(Bicycle item)
        {
            _db.Bicycles.Add(item);
            _db.SaveChanges();
            return true;
        }
        public bool Update(Bicycle item)
        {
            _db.Bicycles.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            _db.Bicycles.Remove(GetById(Id));
            _db.SaveChanges();
            return true;
        }
        public SelectList GetSelectList() => new SelectList(_db.Bicycles.ToList(), "Id", "Name");
    }
}
