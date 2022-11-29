using Microsoft.AspNetCore.Mvc.Rendering;

namespace bicycle_store_web.Interfaces
{
    public interface ITypeRepository : IGenericRepository<Type>
    {
        public SelectList GetSelectList();
    }
}
