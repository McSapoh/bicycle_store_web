using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using bicycle_store_web.Interfaces;
using System.Collections.Generic;

namespace bicycle_store_web.Services
{
    public class BicycleService
    {
        private readonly IBicycleRepository _bicycleRepo;
        public BicycleService(IBicycleRepository bicycleRepo)
        {
            _bicycleRepo = bicycleRepo;
        }
        [HttpGet]
        public Bicycle GetBicycle(int Id)
        {
            var bicycle = _bicycleRepo.GetById(Id);
            if (bicycle == null)
                return null;
            else
                return bicycle;
        }
        [HttpGet]
        public IActionResult GetBicycles()
        {
            var list = _bicycleRepo.GetAll().Select(b => new
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
        public IActionResult GetBicyclesWithoutPhoto() => _bicycleRepo.GetAllWithoutPhoto();
        [HttpPost]
        public IActionResult DeleteBicycle(int Id)
        {
            if (_bicycleRepo.Delete(Id))
                return new JsonResult(new { success = true, message = "Delete successful" });
            return new JsonResult(new { success = false, message = "Error while Deleting" });
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
                if (_bicycleRepo.Create(bicycle))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
            else
                if (_bicycleRepo.Update(bicycle))
                    return new JsonResult(new { success = true, message = "Successfully saved" });

            return new JsonResult(new { success = false, message = "Error while saving" });
        }
        public SelectList GetSelectList() => _bicycleRepo.GetSelectList();
    }
}
