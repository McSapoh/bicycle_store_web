using bicycle_store_web.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_store_web.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly bicycle_storeContext _db;
        public UserRepository(bicycle_storeContext _db)
        {
            this._db = _db;
        }
        public List<User> GetAll() => _db.Users.ToList();
        public User GetById(int Id) => _db.Users.FirstOrDefault(b => b.Id == Id);
        public int GetUserId(string Username) => _db.Users.FirstOrDefault(u => u.Username == Username).Id;
        public string GetUserRole(int UserId) => _db.Users.FirstOrDefault(u => u.Id == UserId).Role.ToString();
        public bool Create(User item)
        {
            _db.Users.Add(item);
            _db.SaveChanges();
            return true;
        }
        public bool Update(User item)
        {
            _db.Users.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            _db.Users.Remove(GetById(Id));
            _db.SaveChanges();
            return true;
        }
    }
}
