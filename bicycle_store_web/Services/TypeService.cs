using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Services
{
    public class TypeService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        private readonly bicycle_storeContext _db;
        public TypeService(IWebHostEnvironment WebHostEnvironment, bicycle_storeContext context)
        {
            this.WebHostEnvironment = WebHostEnvironment;
            _db = context;
        }
        [HttpGet]
        public Type GetType(int id)
        {
            var type = _db.Types.FirstOrDefault(t => t.Id == id);
            if (type == null)
                return null;
            else
                return type;
        }
        [HttpGet]
        public IActionResult GetTypes()
        {
            var list = _db.Types.Select(p => new {
                p.Id,
                p.Name,
                p.Description
            }).ToList();
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public IActionResult DeleteType(int Id)
        {
            var typeFromDb = _db.Types.FirstOrDefault(t => t.Id == Id);
            if (typeFromDb == null)
            {
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            }
            _db.Types.Remove(typeFromDb);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Delete successful" });
        }

        [HttpPost]
        public IActionResult SaveType(Type type)
        {
            if (type.Id == 0)
                _db.Types.Add(type);
            else
                _db.Types.Update(type);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Successfully saves" });
        }
        public SelectList GetTypeSelectList() => new SelectList(_db.Types.ToList(), "Id", "Name");
    }
}
