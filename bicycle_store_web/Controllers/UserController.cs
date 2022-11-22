using bicycle_store_web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace bicycle_store_web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly bicycle_storeContext _db;
        private readonly UserService userService;
        private readonly BicycleService bicycleService;
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
        public UserController(ILogger<UserController> logger, bicycle_storeContext context, 
            UserService userService, BicycleService bicycleService)
        {
            _logger = logger;
            _db = context;
            this.userService = userService;
            this.bicycleService = bicycleService;
            //AddImages();
        }
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
            ViewBag.Bicycles = bicycleService.GetBicycleSelectList();
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
            return Redirect("Bicycles");
        }
        [HttpPost("User/Login")]
        public IActionResult IsLogin(string Username, string Password, string ReturnUrl)
        {
            user = userService.GetUser(Username);
            if(user != null && user.Password == Password)
            {
                HttpContext.SignInAsync(userService.GetClaims(user));
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
                return userService.SaveUser(user, Photo);
            return Json(new { success = false, message = "Error while saving" });
        }
        public IActionResult DeleteUser(int Id) => userService.DeleteUser(Id);
        public IActionResult GetUserRole() => Json(new
        {
            data = userService.GetUserRole(userService.GetUserId(User.Identity.Name))
        });
        public IActionResult GetUserId() => Json(new
        {
            data = userService.GetUserId(User.Identity.Name)
        });
    }
}
