using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class ProducerRepository : IGenericRepository<Producer>
    {
        public readonly bicycle_storeContext _db;
        public ProducerRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<Producer> GetAll() => _db.Producers.ToList();
        public Producer GetById(int Id) => _db.Producers.FirstOrDefault(b => b.Id == Id);
        public bool Create(Producer item)
        {
            _db.Producers.Add(item);
            _db.SaveChanges();
            return true;
        }
        public bool Update(Producer item)
        {
            _db.Producers.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            _db.Producers.Remove(GetById(Id));
            _db.SaveChanges();
            return true;
        }
        public SelectList GetSelectList() => new SelectList(_db.Producers.ToList(), "Id", "Name");
    }
}
