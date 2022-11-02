using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using bicycle_store_web.Enums;

namespace bicycle_store_web.Services
{
    public class UserService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        private readonly bicycle_storeContext _db;
        private readonly ShoppingCartService shopingCartService;
        public UserService(IWebHostEnvironment WebHostEnvironment, bicycle_storeContext context,
            ShoppingCartService shopingCartService)
        {
            this.WebHostEnvironment = WebHostEnvironment;
            this.shopingCartService = shopingCartService;
            _db = context;
        }
        [HttpGet]
        public int GetUserId(string Username) => _db.Users.FirstOrDefault(u => u.Username == Username).Id;
        [HttpGet]
        public string GetUserRole(int UserId) => 
            _db.Users.FirstOrDefault(u => u.Id == UserId).Role.ToString();
        [HttpGet]
        public User GetUser(string Username) => _db.Users.FirstOrDefault(u => u.Username == Username);
        [HttpGet]
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
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public IActionResult ChangePermisions(int Id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (user.Photo == null)
                user.Photo = Properties.Resources.admin;
            if (user != null)
            {
                if (user.Role == Roles.SuperAdmin.ToString())
                {
                    if (user.Photo == null)
                        user.Photo = Properties.Resources.admin;
                    _db.Users.Update(user);
                }
                else if (user.Role == Roles.Admin.ToString())
                {
                    if (user.Photo.SequenceEqual(Properties.Resources.admin))
                        user.Photo = Properties.Resources.user;
                    user.Role = Roles.User.ToString();
                    _db.Users.Update(user);
                }
                else
                {
                    if (user.Photo.SequenceEqual(Properties.Resources.user))
                        user.Photo = Properties.Resources.admin;
                    user.Role = Roles.Admin.ToString();
                    _db.Users.Update(user);
                }
                _db.SaveChanges();
                return new JsonResult(new { success = true, message = "Successfully changed" });
            }
            return new JsonResult(new { success = false, message = "Error while changing" });
        }
        [HttpPost]
        public IActionResult SaveUser(User user, IFormFile Photo)
        {
            if (user.Role == null)
                user.Role = Roles.User.ToString();

            if (Photo == null)
            {
                if (user.Role == Roles.User.ToString())
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
            {
                _db.Users.Add(user);
                shopingCartService.CreateShopingCart(user.Id);
            }
            else
                _db.Users.Update(user);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Successfully saved" });
        }
        [HttpPost]
        public IActionResult DeleteUser(int Id)
        {
            var userFromDb = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (userFromDb == null)
            {
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            }
            _db.Users.Remove(userFromDb);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Delete successful" });
        }
        public ClaimsPrincipal GetClaims (User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
