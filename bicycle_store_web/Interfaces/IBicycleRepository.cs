﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bicycle_store_web.Interfaces
{
    public interface IBicycleRepository : IGenericRepository<Bicycle>
    {
        public IActionResult GetAllWithoutPhoto();
        public SelectList GetSelectList();
    }
}