using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using bicycle_store_web.Repositories;

namespace bicycle_store_web.Services
{
    public class BicycleService
    {
        private readonly BicycleRepository bicycleRepo;
        public BicycleService(bicycle_storeContext context)
        {
            bicycleRepo = new BicycleRepository(context);
        }
        [HttpGet]
        public Bicycle GetBicycle(int Id)
        {
            var bicycle = bicycleRepo.GetById(Id);
            if (bicycle == null)
                return null;
            else
                return bicycle;
        }
        [HttpGet]
        public IActionResult GetBicycles()
        {
            var list = bicycleRepo.GetAll().Select(b => new {
                b.Id,b.Name,
                b.WheelDiameter, b.Price,
                b.Quantity, b.TypeId,
                b.Type, b.CountryId,
                b.Country, b.ProducerId,
                b.Producer,
            }).ToList();
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public IActionResult DeleteBicycle(int Id)
        {
            if (bicycleRepo.Delete(Id))
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            return new JsonResult(new { success = true, message = "Delete successful" });
        }
        [HttpPost]
        public IActionResult SaveBicycle(Bicycle bicycle, IFormFile Photo)
        {
            if (Photo == null)
                bicycle.Photo = Properties.Resources.bicycle;
            else
            {
                using (var fs1 = Photo.OpenReadStream())
                using (var ms1 = new MemoryStream())
                {
                    fs1.CopyTo(ms1);
                    bicycle.Photo = ms1.ToArray();
                }
            }

            if (bicycle.Id == 0)
                if (bicycleRepo.Create(bicycle))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
            else
                if (bicycleRepo.Update(bicycle))
                    return new JsonResult(new { success = true, message = "Successfully saved" });

            return new JsonResult(new { success = false, message = "Error while saving" });
        }
        public SelectList GetSelectList() => bicycleRepo.GetSelectList();
    }
}
