using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace bicycle_store_web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly bicycle_storeContext _db;
        [BindProperty]
        public User user { get; set; }
        [BindProperty]
        public Order order { get; set; }
        [BindProperty]
        public IFormFile img { get; set; }
        public List<Claim> claims { get; set; }
        public ClaimsIdentity claimsIdentity { get; set; }
        public ClaimsPrincipal claimsPrincipal { get; set; }
        public void AddImages()
        {
            foreach (User u in _db.Users)
                //SaveUser(u, null);
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
        public UserController(ILogger<UserController> logger, bicycle_storeContext context)
        {
            _logger = logger;
            _db = context;
            //AddImages();
        }
        public IActionResult Index() => View();
        public IActionResult Bicycles() => View("Bicycles", AddData());
        public IActionResult SetProfile() 
        {
            if (User.Identity.IsAuthenticated)
                user = _db.Users.FirstOrDefault(u => u.FullName == User.Identity.Name);
            else
                user = new User();
            return View(user);
        }
        public IActionResult OrderCreating()
        {
            ViewBag.Bicycles = new SelectList(_db.Bicycles.ToList(), "Id", "Name");
            order = new Order();
            var bicycleOrder = new BicycleOrder();
            order.BicycleOrders.Add(bicycleOrder);
            return View();
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
            return Redirect("Index");
        }
        [HttpPost("User/Login")]
        public IActionResult IsLogin(string Username, string Password, string ReturnUrl)
        {
            user = _db.Users.FirstOrDefault(u => u.Username == Username);
            if(user != null && user.Password == Password)
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, Username),
                    new Claim(ClaimTypes.Name, Username),
                    new Claim(ClaimTypes.Role, user.Role),
                };

                claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(claimsPrincipal);
                if ((user.Role == "Admin" || user.Role == "SuperAdmin") && ReturnUrl == null)
                {
                    return Redirect("/Admin/Index");
                }
                return Redirect(ReturnUrl ?? "Index");
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult SaveUser([FromForm] User user, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (user.Role == null)
                    user.Role = "User";

                if (Photo == null)
                {
                    if (user.Role == "User")
                        user.Photo = Properties.Resources.user;
                    else
                        user.Photo = Properties.Resources.admin;
                }
                else
                {
                    using (var fs1 = Photo.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        user.Photo = ms1.ToArray();
                        if (Photo == null)
                            user.Photo = Properties.Resources.user;
                    }
                }


                if (user.Id == 0)
                    _db.Users.Add(user);
                else
                    _db.Users.Update(user);
                _db.SaveChanges();
                return Json(new { success = true, message = "Successfully saved" });
            }
            return Json(new { success = false, message = "Error while saving" });
        }
        public IActionResult DeleteUser(int Id)
        {
            var userFromDb = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Users.Remove(userFromDb);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
