using bicycle_store_web.Interfaces;
using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace bicycle_store_web.Services
{
    public class TypeService
    {
        private readonly ITypeRepository _typeRepo;
        public TypeService(ITypeRepository typeRepo)
        {
            _typeRepo = typeRepo;
        }
        [HttpGet]
        public Type GetById(int Id)
        {
            var type = _typeRepo.GetById(Id);
            if (type == null)
                return null;
            else
                return type;
        }
        [HttpGet]
        public IActionResult GetTypes()
        {
            var list = _typeRepo.GetAll().Select(p => new
            {
                p.Id,
                p.Name,
                p.Description
            }).ToList();
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public IActionResult DeleteType(int Id)
        {
            if (_typeRepo.Delete(Id))
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            return new JsonResult(new { success = true, message = "Delete successful" });
        }
        [HttpPost]
        public IActionResult SaveType(Type type)
        {
            if (type.Id == 0)
                if (_typeRepo.Create(type))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
                else
                if (_typeRepo.Update(type))
                    return new JsonResult(new { success = true, message = "Successfully saved" });

            return new JsonResult(new { success = false, message = "Error while saving" });
        }
        public SelectList GetSelectList() => _typeRepo.GetSelectList();
    }
}
