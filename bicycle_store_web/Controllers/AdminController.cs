using bicycle_store_web.Models;
using bicycle_store_web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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

        [BindProperty]
        public Bicycle bicycle { get; set; }
        [BindProperty]
        public Type type { get; set; }
        [BindProperty]
        public Producer producer { get; set; }
        public void AddImages ()
        {
            foreach (Bicycle b in _db.Bicycles)
                bicycleService.SaveBicycle(b, null);
            _db.SaveChanges();
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
        }
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
            ViewBag.Type = typeService.GetSelectList();
            ViewBag.Country = new SelectList(_db.Countries.ToList(), "Id", "Name");
            ViewBag.Producer = producerService.GetSelectList();
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
            type = typeService.GetById(id.Value);
            if (type == null)
                return NotFound();
            return PartialView("_AddType", type);
        }
        public IActionResult _AddProducer(int? id)
        {
            producer = new Producer();
            if (id == null)
                return PartialView("_AddProducer", producer);
            producer = producerService.GetById(id.Value);
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
        public IActionResult ChangePermisions(int Id)
        {
            if(userService.ChangePermisions(Id))
                return new JsonResult(new { success = true, message = "Successfully saved" });
            else
                return Json(new { success = false, message = "Error while saving" });
        }
        // Bicycle actions
        [HttpGet]
        public IActionResult GetBicycles() => bicycleService.GetBicyclesWithoutPhoto();
        public IActionResult DeleteBicycle(int Id)
        {
            if (bicycleService.DeleteBicycle(Id))
                return new JsonResult(new { success = true, message = "Delete successful" });
            else
                return new JsonResult(new { success = false, message = "Error while Deleting" });
        }
        [HttpPost]
        public IActionResult SaveBicycle(Bicycle bicycle, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (bicycleService.SaveBicycle(bicycle, Photo))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
            }

            return Json(new { success = false, message = "Error while saving" });
        }
        [HttpGet]
        // Types actions
        [HttpGet]
        public IActionResult GetTypes() => typeService.GetTypes();
        public IActionResult DeleteType(int Id)
        {
            if (typeService.DeleteType(Id))
                return new JsonResult(new { success = true, message = "Delete successful" });
            else
                return new JsonResult(new { success = false, message = "Error while Deleting" });
        }
        [HttpPost]
        public IActionResult SaveType()
        {
            if (ModelState.IsValid)
            {
                if (typeService.SaveType(type))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
            }

            return Json(new { success = false, message = "Error while saving" });
        }
        // Producers actions
        [HttpGet]
        public IActionResult GetProducers() => producerService.GetProducers();
        public IActionResult DeleteProducer(int Id)
        {
            if (producerService.DeleteProducer(Id))
                return new JsonResult(new { success = true, message = "Delete successful" });
            else
                return new JsonResult(new { success = false, message = "Error while Deleting" });
        }

        [HttpPost]
        public IActionResult SaveProducer()
        {
            if (ModelState.IsValid)
            {
                if (producerService.SaveProducer(producer))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
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