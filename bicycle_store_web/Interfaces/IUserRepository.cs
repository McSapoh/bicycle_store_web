namespace bicycle_store_web.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public int GetUserId(string Username);
        public string GetUserRole(int UserId);
    }
}
