using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        public readonly bicycle_storeContext _db;
        public ProducerRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<Producer> GetAll() => _db.Producers.ToList();
        public Producer GetById(int Id) => _db.Producers.FirstOrDefault(b => b.Id == Id);
        public void Create(Producer item)
        {
            _db.Producers.Add(item);
            _db.SaveChanges();
        }
        public void Update(Producer item)
        {
            _db.Producers.Update(item);
            _db.SaveChanges();
        }
        public void Delete(int Id)
        {
            _db.Producers.Remove(GetById(Id));
            _db.SaveChanges();
        }
        public SelectList GetSelectList() => new SelectList(_db.Producers.ToList(), "Id", "Name");
    }
}
