using bicycle_store_web.Models;
using bicycle_store_web.Services;
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
        private readonly BicycleService bicycleService;
        private readonly UserService userService;
        private readonly TypeService typeService;
        private readonly ProducerService producerService;
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
            {
                //if(b.Id == 10)
                    SaveBicycle(b, null);
            }
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
        public AdminController(ILogger<AdminController> logger, bicycle_storeContext context,
            BicycleService bicycleService, ProducerService producerService, TypeService typeService,
            UserService userService)
        {
            _logger = logger;
            _db = context;
            this.bicycleService = bicycleService;
            this.producerService = producerService;
            this.typeService = typeService;
            this.userService = userService;
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
            ViewBag.Type = typeService.GetTypeSelectList();
            ViewBag.Country = new SelectList(_db.Countries.ToList(), "Id", "Name");
            ViewBag.Producer = producerService.GetProducerSelectList();
            if (id == null)
                return PartialView("_AddBicycle", bicycle);
            bicycle = bicycleService.GetBicycle(id.Value);
            if (bicycle == null)
                return NotFound();
            return PartialView("_AddBicycle", bicycle);
        }
        public IActionResult _AddType(int? id)
        {
            type = new Type();
            if (id == null)
                return PartialView("_AddType", type);
            type = typeService.GetType(id.Value);
            if (type == null)
                return NotFound();
            return PartialView("_AddType", type);
        }
        public IActionResult _AddProducer(int? id)
        {
            producer = new Producer();
            if (id == null)
                return PartialView("_AddProducer", producer);
            producer = producerService.GetProducer(id.Value);
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
        [HttpGet]
        public IActionResult GetUsers() => userService.GetUsers();
        [HttpPost]
        public IActionResult ChangePermisions(int Id) => userService.ChangePermisions(Id);
        // Bicycle actions
        [HttpGet]
        public IActionResult GetBicycles() => bicycleService.GetBicycles();
        public IActionResult DeleteBicycle(int Id) => bicycleService.DeleteBicycle(Id);

        [HttpPost]
        public IActionResult SaveBicycle(Bicycle bicycle, IFormFile Photo)
        {
            if (ModelState.IsValid)
                return bicycleService.SaveBicycle(bicycle, Photo);
            return Json(new { success = false, message = "Error while saving" });
        }
        [HttpGet]
        // Types actions
        [HttpGet]
        public IActionResult GetTypes() => typeService.GetTypes();
        [HttpPost]
        public IActionResult DeleteType(int Id) => typeService.DeleteType(Id);

        [HttpPost]
        public IActionResult SaveType()
        {
            if (ModelState.IsValid)
                return typeService.SaveType(type);
            return Json(new { success = false, message = "Error while saving" });
        }
        // Producers actions
        [HttpGet]
        public IActionResult GetProducers() => producerService.GetProducers();
        [HttpPost]
        public IActionResult DeleteProducer(int Id) => producerService.DeleteProducer(Id);

        [HttpPost]
        public IActionResult SaveProducer()
        {
            if (ModelState.IsValid)
                return producerService.SaveProducer(producer);
            return Json(new { success = false, message = "Error while saving" });
        }
        #endregion
    }
}

/*
dotnet ef dbcontext scaffold "Server=localhost;User=root;Password=badyadya2004;Database=bicycle_store;" "Pomelo.EntityFrameworkCore.MySql"
dotnet build ef dbcontext scaffold "Server=localhost;User=root;Password=badyadya2004;Database=bicycle_store;" "Pomelo.EntityFrameworkCore.MySql" -OutputDir Models
 */