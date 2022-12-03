using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bicycle_store_web.Interfaces
{
    public interface ITypeRepository : IGenericRepository<Bicycle>
    {
        public SelectList GetSelectList();
    }
}
