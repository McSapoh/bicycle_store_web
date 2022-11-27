using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace bicycle_store_web.Services
{
    public class TypeService
    {
        private readonly TypeRepository typeRepo;
        public TypeService(bicycle_storeContext context)
        {
            typeRepo = new TypeRepository(context);
        }
        [HttpGet]
        public Type GetById(int Id)
        {
            var type = typeRepo.GetById(Id);
            if (type == null)
                return null;
            else
                return type;
        }
        [HttpGet]
        public IActionResult GetTypes()
        {
            var list = typeRepo.GetAll().Select(p => new
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
            if (typeRepo.Delete(Id))
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            return new JsonResult(new { success = true, message = "Delete successful" });
        }
        [HttpPost]
        public IActionResult SaveType(Type type)
        {
            if (type.Id == 0)
                if (typeRepo.Create(type))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
                else
                if (typeRepo.Update(type))
                    return new JsonResult(new { success = true, message = "Successfully saved" });

            return new JsonResult(new { success = false, message = "Error while saving" });
        }
        public SelectList GetSelectList() => typeRepo.GetSelectList();
    }
}
