using Microsoft.AspNetCore.Mvc.Rendering;

namespace bicycle_store_web.Interfaces
{
    public interface IProducerRepository : IGenericRepository<Producer>
    {
        public SelectList GetSelectList();
    }
}
