using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bicycle_store_web.Services
{
    public class BicycleService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        private readonly bicycle_storeContext _db;
        public BicycleService(IWebHostEnvironment WebHostEnvironment, bicycle_storeContext context)
        {
            this.WebHostEnvironment = WebHostEnvironment;
            _db = context;
        }
        [HttpGet]
        public Bicycle GetBicycle(int id)
        {
            var bicycle = _db.Bicycles.FirstOrDefault(b => b.Id == id);
            if (bicycle == null)
                return null;
            else
                return bicycle;
        }
        [HttpGet]
        public IActionResult GetBicycles()
        {
            var list = _db.Bicycles.Select(b => new {
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
        [HttpPost]
        public IActionResult DeleteBicycle(int Id)
        {
            var bicycleFromDb = _db.Bicycles.FirstOrDefault(b => b.Id == Id);
            if (bicycleFromDb == null)
            {
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            }
            _db.Bicycles.Remove(bicycleFromDb);
            _db.SaveChanges();
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
                _db.Bicycles.Add(bicycle);
            else
                _db.Bicycles.Update(bicycle);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Successfully saved" });
        }
        public SelectList GetBicycleSelectList() => new SelectList(_db.Bicycles.ToList(), "Id", "Name");

    }
}
