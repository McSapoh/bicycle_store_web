using bicycle_store_web.Interfaces;
using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace bicycle_store_web.Services
{
    public class TypeService
    {
        private readonly ITypeRepository _typeRepo;
        public TypeService(ITypeRepository typeRepo)
        {
            _typeRepo = typeRepo;
        }
        [HttpGet]
        public Type GetById(int Id)
        {
            var type = _typeRepo.GetById(Id);
            if (type == null)
                return null;
            else
                return type;
        }
        [HttpGet]
        public IActionResult GetTypes()
        {
            var list = _typeRepo.GetAll().Select(p => new
            {
                p.Id,
                p.Name,
                p.Description
            }).ToList();
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public bool DeleteType(int Id)
        {
            _typeRepo.Delete(Id);
            if (_typeRepo.GetById(Id) == null)
                return true;
            else
                return false;
        }
        [HttpPost]
        public bool SaveType(Type type)
        {
            if (type.Id == 0)
            {
                _typeRepo.Create(type);
                if (_typeRepo.GetById(type.Id) != null)
                    return true;
            }
            else
            {
                _typeRepo.Update(type);
                if (_typeRepo.GetById(type.Id) != null)
                    return true;
            }
            return false;
        }
        public SelectList GetSelectList() => _typeRepo.GetSelectList();
    }
}
