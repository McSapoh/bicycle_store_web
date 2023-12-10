using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace bicycle_store_web.Controllers
{
    public class UserController : Controller
    {
        private readonly bicycle_storeContext _db;
        private readonly IUserService _userService;
        private readonly IBicycleService _bicycleService;
        [BindProperty]
        public User user { get; set; }
        [BindProperty]
        public Order order { get; set; }
        [BindProperty]
        public IFormFile img { get; set; }
        public void AddImages()
        {
            foreach (User u in _db.Users)
                SaveUser(u, null);
            _db.SaveChanges();
        }
        public IQueryable<Bicycle> AddData()
        {
            var bicycles = _db.Bicycles.AsNoTracking()
                .Include(t => t.Type)
                .Include(p => p.Producer)
                .Include(c => c.Country);
            foreach (Bicycle b in bicycles)
            {
                b.Type.Bicycles.Add(b);
                b.Producer.Bicycles.Add(b);
                b.Country.Bicycles.Add(b);
            }
            return bicycles;
        }
        public UserController(bicycle_storeContext context, IUserService _userService, IBicycleService _bicycleService)
        {
            _db = context;
            this._userService = _userService;
            this._bicycleService = _bicycleService;
            //AddImages();
        }
        public IActionResult Bicycles() => View("Bicycles", _bicycleService.GetBicycles());
        public IActionResult SetProfile() 
        {
            if (User.Identity.IsAuthenticated)
                user = _userService.GetById(_userService.GetUserId(User.Identity.Name));
            else
                user = new User();
            return View(user);
        }
        public IActionResult Register() {
            user = new User();
            return View("Register", user); 
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl) 
        {
            ViewData["returnUrl"] = ReturnUrl;
            return View(); 
        }
        public IActionResult Logout ()
        {
            HttpContext.SignOutAsync();
            return Redirect("Bicycles");
        }
        [HttpPost("User/Login")]
        public IActionResult IsLogin(string Username, string Password, string ReturnUrl)
        {
            user = _userService.GetById(_userService.GetUserId(Username));
            if(user != null && user.Password == Password)
            {
                HttpContext.SignInAsync(_userService.GetClaims(user));
                if ((user.Role == "Admin" || user.Role == "SuperAdmin") && ReturnUrl == null)
                    return Redirect("/Admin/Bicycles");
                return Redirect(ReturnUrl ?? "Bicycles");
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult SaveUser(User user, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (_userService.SaveUser(user, Photo))
                    return new JsonResult(new { success = true, message = "Successfully saved" });
            }

            return Json(new { success = false, message = "Error while saving" });
        }
        public IActionResult DeleteUser(int Id)
        {
            if (_userService.DeleteUser(Id))
                return new JsonResult(new { success = true, message = "Delete successful" });
            else
                return new JsonResult(new { success = false, message = "Error while Deleting" });
        }
        public IActionResult GetUserRole() => Json(new
        {
            data = _userService.GetUserRole(_userService.GetUserId(User.Identity.Name))
        });
        public IActionResult GetUserId() => Json(new
        {
            data = _userService.GetUserId(User.Identity.Name)
        });


        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
