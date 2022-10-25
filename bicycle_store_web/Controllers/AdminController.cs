using bicycle_store_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace bicycle_store_web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly bicycle_storeContext _db;
        private IQueryable bicycles;

        [BindProperty]
        public Bicycle bicycle { get; set; }
        [BindProperty]
        public Type type { get; set; }
        [BindProperty]
        public Producer producer { get; set; }
        public void AddImages ()
        {
            foreach (Bicycle b in _db.Bicycles)
                SaveBicycle(b, null);
            _db.SaveChanges();
        }
        public void AddData()
        {
            bicycles = _db.Bicycles.AsNoTracking()
                .Include(t => t.Type)
                .Include(p => p.Producer)
                .Include(c => c.Country);
            foreach (Bicycle b in bicycles)
            {
                b.Type.Bicycles.Add(b);
                b.Producer.Bicycles.Add(b);
                b.Country.Bicycles.Add(b);
            }
        }
        public AdminController(ILogger<AdminController> logger, bicycle_storeContext context)
        {
            _logger = logger;
            _db = context;
            AddData();
            //AddImages();
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index() => View();

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Bicycles() => View();

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Types() => View();

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Producers() => View();

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Users() 
        {

            var user = _db.Users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            ViewBag.Id = user.Id;
            return View();
        }
        public IActionResult _AdminPanel() => PartialView("AdminPanel");
        public IActionResult _AddBicycle(int? id) 
        {
            bicycle = new Bicycle();
            bicycle.Photo = Properties.Resources.bicycle;
            ViewBag.Type = new SelectList(_db.Types.ToList(), "Id", "Name");
            ViewBag.Country = new SelectList(_db.Countries.ToList(), "Id", "Name");
            ViewBag.Producer = new SelectList(_db.Producers.ToList(), "Id", "Name");
            if (id == null)
                return PartialView("_AddBicycle", bicycle);
            bicycle = _db.Bicycles.FirstOrDefault(b => b.Id == id);
            if (bicycle == null)
                return NotFound();
            return PartialView("_AddBicycle", bicycle);
        }
        public IActionResult _AddType(int? id)
        {
            type = new Type();
            if (id == null)
                return PartialView("_AddType", type);
            type = _db.Types.FirstOrDefault(t => t.Id == id);
            if (type == null)
                return NotFound();
            return PartialView("_AddType", type);
        }
        public IActionResult _AddProducer(int? id)
        {
            producer = new Producer();
            if (id == null)
                return PartialView("_AddProducer", producer);
            producer = _db.Producers.FirstOrDefault(t => t.Id == id);
            if (producer == null)
                return NotFound();
            return PartialView("_AddProducer", producer);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region API Calls
        // User actions
        public IActionResult GetUsers()
        {
            var list = _db.Users.Select(u => new {
                u.Id,
                u.FullName,
                u.Phone,
                u.Email,
                u.Adress,
                u.Username,
                u.Role,
                u.Photo
            }).ToList();
            return Json(new { data = list });
        }
        [HttpPost]
        public IActionResult ChangePermisions(int Id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (user.Photo == null)
                user.Photo = Properties.Resources.admin;
            if (user != null)
            {
                if (user.Role == "SuperAdmin")
                {
                    if (user.Photo == null)
                        user.Photo = Properties.Resources.admin;
                    _db.Users.Update(user);
                }
                else if (user.Role == "Admin")
                {
                    if (user.Photo.SequenceEqual(Properties.Resources.admin))
                        user.Photo = Properties.Resources.user;
                    user.Role = "User";
                    _db.Users.Update(user);
                }
                else
                {
                    if (user.Photo.SequenceEqual(Properties.Resources.user))
                        user.Photo = Properties.Resources.admin;
                    user.Role = "Admin";
                    _db.Users.Update(user);
                }
                _db.SaveChanges();
                return Json(new { success = true, message = "Successfully changed" });
            }
            return Json(new { success = false, message = "Error while changing" });
        }
        // Bicycle actions
        [HttpGet]
        public IActionResult GetBicycles()
        {
            var list = _db.Bicycles.Select(b => new { 
                b.Id, b.Name, 
                b.WheelDiameter, 
                b.Price, b.Quantity,
                b.TypeId, b.Type,
                b.CountryId, b.Country,
                b.ProducerId, b.Producer,
            }).ToList();
            return Json(new { data =  list });
        }
        public IActionResult DeleteBicycle(int Id)
        {
            var bicycleFromDb =  _db.Bicycles.FirstOrDefault(b => b.Id == Id);
            if (bicycleFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Bicycles.Remove(bicycleFromDb);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpPost]
        public IActionResult SaveBicycle(Bicycle bicycle, IFormFile Photo)
        {
            if (ModelState.IsValid)
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
                return Json(new { success = true, message = "Successfully saved" });
            }
            return Json(new { success = false, message = "Error while saving" });
        }
        [HttpGet]
        // Types actions
        public IActionResult GetTypes()
        {
            var list = _db.Types.Select(b => new {
                b.Id,
                b.Name,
                b.Description
            }).ToList();
            return Json(new { data = list });
        }
        public IActionResult DeleteType(int Id)
        {
            var typeFromDb = _db.Types.FirstOrDefault(t => t.Id == Id);
            if (typeFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Types.Remove(typeFromDb);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpPost]
        public IActionResult SaveType()
        {
            if (ModelState.IsValid)
            {
                if (type.Id == 0)
                    _db.Types.Add(type);
                else
                    _db.Types.Update(type);
                _db.SaveChanges();
                return Json(new { success = true, message = "Successfully saves" });
            }
            return Json(new { success = false, message = "Error while saving" });
        }
        // Producers actions
        public IActionResult GetProducers()
        {
            var list = _db.Producers.Select(p => new {
                p.Id,
                p.Name,
                p.Description
            }).ToList();
            return Json(new { data = list });
        }
        public IActionResult DeleteProducer(int Id)
        {
            var producerFromDb = _db.Producers.FirstOrDefault(t => t.Id == Id);
            if (producerFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Producers.Remove(producerFromDb);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpPost]
        public IActionResult SaveProducer()
        {
            if (ModelState.IsValid)
            {
                if (producer.Id == 0)
                    _db.Producers.Add(producer);
                else
                    _db.Producers.Update(producer);
                _db.SaveChanges();
                return Json(new { success = true, message = "Successfully saves" });
            }
            return Json(new { success = false, message = "Error while saving" });
        }
        #endregion
    }
}

/*
dotnet ef dbcontext scaffold "Server=localhost;User=root;Password=badyadya2004;Database=bicycle_store;" "Pomelo.EntityFrameworkCore.MySql"
dotnet build ef dbcontext scaffold "Server=localhost;User=root;Password=badyadya2004;Database=bicycle_store;" "Pomelo.EntityFrameworkCore.MySql" -OutputDir Models
 */