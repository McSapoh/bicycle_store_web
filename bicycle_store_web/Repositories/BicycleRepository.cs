using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class BicycleRepository : IBicycleRepository
    {
        public readonly bicycle_storeContext _db;
        public BicycleRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<Bicycle> GetAll() => _db.Bicycles.AsNoTracking()
                .Include(t => t.Type)
                .Include(p => p.Producer)
                .Include(c => c.Country).ToList();

        public IActionResult GetAllWithoutPhoto()
        {
            var list = _db.Bicycles.Select(b => new
            {
                b.Id,
                b.Name,
                b.WheelDiameter,
                b.Price,
                b.Quantity,
                b.TypeId,
                b.Type,
                b.CountryId,
                b.Country,
                b.ProducerId,
                b.Producer,
            }).ToList();
            return new JsonResult(new { data = list });
        }
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
