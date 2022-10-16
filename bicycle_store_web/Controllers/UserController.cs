using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bicycle_store_web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly bicycle_storeContext _db;
        [BindProperty]
        public User user { get; set; }
        [BindProperty]
        public List<Claim> claims { get; set; }
        [BindProperty]
        public ClaimsIdentity claimsIdentity { get; set; }
        [BindProperty]
        public ClaimsPrincipal claimsPrincipal { get; set; }
        public UserController(ILogger<UserController> logger, bicycle_storeContext context)
        {
            _logger = logger;
            _db = context;
        }
        public IActionResult Index() => View("Index");
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
                claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, Username));
                claims.Add(new Claim(ClaimTypes.Name, Username));
                claims.Add(new Claim(ClaimTypes.Role, user.Role));

                claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(ReturnUrl ?? "Index");
            }
            return BadRequest();
        }
        public IActionResult SaveUser()
        {
            if (ModelState.IsValid)
            {
                user.Role = "User";
                if (user.Id == 0)
                    _db.Users.Add(user);
                else
                    _db.Users.Update(user);
                _db.SaveChanges();
                Redirect("Index");
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
