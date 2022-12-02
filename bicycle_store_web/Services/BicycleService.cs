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
        public bool DeleteBicycle(int Id)
        {
            _bicycleRepo.Delete(Id);
            if (_bicycleRepo.GetById(Id) == null)
                return true;
            else
                return false;
        }
        [HttpPost]
        public bool SaveBicycle(Bicycle bicycle, IFormFile Photo)
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
            {
                _bicycleRepo.Create(bicycle);
                if (_bicycleRepo.GetById(bicycle.Id) != null)
                    return true;
            }
            else
            {
                _bicycleRepo.Update(bicycle);
                if (_bicycleRepo.GetById(bicycle.Id) != null)
                    return true;
            }
            return false;
        }
        public SelectList GetSelectList() => _bicycleRepo.GetSelectList();
    }
}
