using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class TypeRepository : IGenericRepository<Type>
    {
        public readonly bicycle_storeContext _db;
        public TypeRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<Type> GetAll() => _db.Types.ToList();
        public Type GetById(int Id) => _db.Types.FirstOrDefault(b => b.Id == Id);
        public bool Create(Type item)
        {
            _db.Types.Add(item);
            _db.SaveChanges();
            return true;
        }
        public bool Update(Type item)
        {
            _db.Types.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            _db.Types.Remove(GetById(Id));
            _db.SaveChanges();
            return true;
        }
        public SelectList GetSelectList() => new SelectList(_db.Types.ToList(), "Id", "Name");
    }
}
