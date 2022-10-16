using bicycle_store_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index() => View();
        [Authorize]
        public IActionResult Bicycles() => View();
        [Authorize]
        public IActionResult Types() => View();
        [Authorize]
        public IActionResult Producers() => View();
        [Authorize]
        public IActionResult Users() => View();
        public IActionResult AdminPanel() => PartialView("AdminPanel");
        public IActionResult _AddBicycle(int? id) 
        {
            bicycle = new Bicycle();
            List<Type> type = _db.Types.ToList();
            ViewBag.Type = new SelectList(type, "Id", "Name");
            List<Country> country = _db.Countries.ToList();
            ViewBag.Country = new SelectList(country, "Id", "Name");
            List<Producer> producer = _db.Producers.ToList();
            ViewBag.Producer = new SelectList(producer, "Id", "Name");
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
            }).ToList();
            return Json(new { data = list });
        }
        [HttpPost]
        public IActionResult ChangePermisions(int Id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                if (user.Role == "Admin")
                {
                    user.Role = "User";
                    _db.Users.Update(user);
                }
                else
                {
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
        public IActionResult SaveBicycle()
        {
            if (ModelState.IsValid)
            {
                if (bicycle.Id == 0)
                    _db.Bicycles.Add(bicycle);
                else
                    _db.Bicycles.Update(bicycle);
                _db.SaveChanges();
                return Json(new { success = true, message = "Successfully saves" });
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

        //private string generatedToken = null;

        //[AllowAnonymous]
        //[Route("login")]
        //[HttpPost]
        //public IActionResult Login(User user)
        //{
        //    if (string.IsNullOrEmpty(user.FullName) || string.IsNullOrEmpty(user.Password))
        //    {
        //        return (RedirectToAction("Error"));
        //    }
        //    IActionResult response = Unauthorized();
        //    var validUser = user;

        //    if (validUser != null)
        //    {
        //        generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), validUser);
        //        if (generatedToken != null)
        //        {
        //            HttpContext.Session.SetString("Token", generatedToken);
        //            return RedirectToAction("MainWindow");
        //        }
        //        else
        //        {
        //            return (RedirectToAction("Error"));
        //        }
        //    }
        //    else
        //    {
        //        return (RedirectToAction("Error"));
        //    }
        //}

        //[Authorize]
        //[Route("mainwindow")]
        //[HttpGet]
        //public IActionResult MainWindow()
        //{
        //    string token = HttpContext.Session.GetString("Token");
        //    if (token == null)
        //    {
        //        return (RedirectToAction("Index"));
        //    }
        //    if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
        //    {
        //        return (RedirectToAction("Index"));
        //    }
        //    ViewBag.Message = BuildMessage(token, 50);
        //    return View();
        //}

    }
}

/*
dotnet ef dbcontext scaffold "Server=localhost;User=root;Password=badyadya2004;Database=bicycle_store;" "Pomelo.EntityFrameworkCore.MySql"
dotnet build ef dbcontext scaffold "Server=localhost;User=root;Password=badyadya2004;Database=bicycle_store;" "Pomelo.EntityFrameworkCore.MySql" -OutputDir Models
 */