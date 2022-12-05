using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bicycle_store_web.Interfaces
{
    public interface IBicycleRepository : IGenericRepository<Bicycle>
    {
        public SelectList GetSelectList();
        dynamic GetAllWithoutPhoto();
    }
}
