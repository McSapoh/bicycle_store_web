using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly bicycle_storeContext _db;
        [BindProperty]
        public User user { get; set; }
        public UserController(ILogger<UserController> logger, bicycle_storeContext context)
        {
            _logger = logger;
            _db = context;
        }
        public IActionResult Index() => View("Index");
        public IActionResult AddUser() {
            user = new User();
            return View("AddUser", user); 
        }
        public IActionResult SaveUser()
        {
            if (ModelState.IsValid)
            {
                user.IsAdmin = false;
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
