using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace bicycle_store_web.Interfaces
{
    public interface IUserService
    {
        public int GetUserId(string Username);
        public string GetUserRole(int UserId);
        public User GetById(int Id);
        public List<User> GetUsers();
        public bool ChangePermisions(int Id);
        public bool SaveUser(User user, IFormFile Photo);
        public bool DeleteUser(int Id);
        public ClaimsPrincipal GetClaims(User user);
    }
}
