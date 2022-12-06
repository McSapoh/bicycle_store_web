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
    public class UserService : IUserService
    {
        private readonly IShoppingCartService shopingCartService;
        private readonly IUserRepository _userRepo;
        public UserService(IShoppingCartService shopingCartService, IUserRepository userRepo)
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
        public List<User> GetUsers() => _userRepo.GetAll();
        [HttpPost]
        public bool ChangePermisions(int Id)
        {
            var user = _userRepo.GetById(Id);
            string Role = null;
            if (user != null)
            {
                if (user.Photo == null)
                    user.Photo = Properties.Resources.admin;
                if (user.Role == Roles.SuperAdmin.ToString())
                {
                    Role = Roles.SuperAdmin.ToString();
                    if (user.Photo == null)
                        user.Photo = Properties.Resources.admin;
                    _userRepo.Update(user);
                }
                else if (user.Role == Roles.Admin.ToString())
                {
                    Role = Roles.User.ToString();
                    if (user.Photo.SequenceEqual(Properties.Resources.admin))
                        user.Photo = Properties.Resources.user;
                    user.Role = Role;
                    _userRepo.Update(user);
                }
                else
                {
                    Role = Roles.Admin.ToString();
                    if (user.Photo.SequenceEqual(Properties.Resources.user))
                        user.Photo = Properties.Resources.admin;
                    user.Role = Role;
                    _userRepo.Update(user);
                }
                if (_userRepo.GetById(Id).Role == Role)
                    return true;
            }
            return false;
        }
        [HttpPost]
        public bool SaveUser(User user, IFormFile Photo)
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
                shopingCartService.CreateShopingCart(user.Id);
                _userRepo.Create(user);
            }
            else
                _userRepo.Update(user);

            if (_userRepo.GetById(user.Id) != null)
                return true;
            else
                return false;
        }
        [HttpPost]
        public bool DeleteUser(int Id)
        {
            _userRepo.Delete(Id);
            if (_userRepo.GetById(Id) == null)
                return true;
            else
                return false;
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
