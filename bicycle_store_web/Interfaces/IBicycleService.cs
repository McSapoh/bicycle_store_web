using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IBicycleService
    {
        public Bicycle GetBicycle(int Id);
        public List<Bicycle> GetBicycles();
        public dynamic GetBicyclesWithoutPhoto();
        public bool DeleteBicycle(int Id);
        public bool SaveBicycle(Bicycle bicycle, IFormFile Photo);
        public SelectList GetSelectList();
    }
}
