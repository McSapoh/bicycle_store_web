using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface ITypeService
    {
        public Type GetById(int Id);
        public List<Type> GetTypes();
        public bool DeleteType(int Id);
        public bool SaveType(Type type);
        public SelectList GetSelectList();
    }
}
