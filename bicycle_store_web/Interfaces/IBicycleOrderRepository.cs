using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IBicycleOrderRepository : IGenericRepository<BicycleOrder>
    {
        public List<BicycleOrder> GetAll(int OrderId);
    }
}
