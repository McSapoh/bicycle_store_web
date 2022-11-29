using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public List<Order> GetAll(int Id);
        public int GetMaxId();
    }
}
