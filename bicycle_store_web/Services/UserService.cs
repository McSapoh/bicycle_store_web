using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using bicycle_store_web.Enums;
using bicycle_store_web.Interfaces;

namespace bicycle_store_web.Services
{
    public class UserService
    {
        private readonly ShoppingCartService shopingCartService;
        private readonly IUserRepository _userRepo;
        public UserService(ShoppingCartService shopingCartService, IUserRepository userRepo)
        {
            this.shopingCartService = shopingCartService;
            _userRepo = userRepo;
        }
        [HttpGet]
        public int GetUserId(string Username) => _userRepo.GetUserId(Username);
        [HttpGet]
        public string GetUserRole(int UserId) => _userRepo.GetUserRole(UserId);
        [HttpGet]
        public User GetById(int Id) => _userRepo.GetById(Id);
        [HttpGet]
        public IActionResult GetUsers()
        {
            var list = _userRepo.GetAll().Select(u => new
            {
                u.Id, u.FullName,
                u.Phone, u.Email,
                u.Adress, u.Username,
                u.Role, u.Photo
            }).ToList();
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public IActionResult ChangePermisions(int Id)
        {
            var user = _userRepo.GetById(Id);
            if (user.Photo == null)
                user.Photo = Properties.Resources.admin;
            if (user != null)
            {
                bool result;
                if (user.Role == Roles.SuperAdmin.ToString())
                {
                    if (user.Photo == null)
                        user.Photo = Properties.Resources.admin;
                    result = _userRepo.Update(user);
                }
                else if (user.Role == Roles.Admin.ToString())
                {
                    if (user.Photo.SequenceEqual(Properties.Resources.admin))
                        user.Photo = Properties.Resources.user;
                    user.Role = Roles.User.ToString();
                    result = _userRepo.Update(user);
                }
                else
                {
                    if (user.Photo.SequenceEqual(Properties.Resources.user))
                        user.Photo = Properties.Resources.admin;
                    user.Role = Roles.Admin.ToString();
                    result = _userRepo.Update(user);
                }
                if (result == true)
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

            bool result;
            if (user.Id == 0)
            {
                shopingCartService.CreateShopingCart(user.Id);
                result = _userRepo.Create(user);
            }
            else
                result = _userRepo.Update(user);

            if (result == true)
                return new JsonResult(new { success = true, message = "Successfully changed" });
            return new JsonResult(new { success = false, message = "Error while changing" });
        }
        [HttpPost]
        public IActionResult DeleteUser(int Id)
        {
            if (_userRepo.Delete(Id))
                return new JsonResult(new { success = true, message = "Successfully deleted" });
            return new JsonResult(new { success = false, message = "Error while deleting" });
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
