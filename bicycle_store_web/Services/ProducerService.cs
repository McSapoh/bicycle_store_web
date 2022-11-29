using bicycle_store_web.Interfaces;
using bicycle_store_web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace bicycle_store_web.Services
{
    public class ProducerService
    {
        private readonly IProducerRepository _producerRepo;
        public ProducerService(IProducerRepository producerRepo)
        {
            this._producerRepo = producerRepo;
        }
        [HttpGet]
        public Producer GetById(int Id)
        {
            var producer = _producerRepo.GetById(Id);
            if (producer == null)
                return null;
            else
                return producer;
        }
        [HttpGet]
        public IActionResult GetProducers()
        {
            var list = _producerRepo.GetAll().Select(p => new
            {
                p.Id,
                p.Name,
                p.Description
            }).ToList();
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public IActionResult DeleteProducer(int Id)
        {
            if (_producerRepo.Delete(Id))
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            return new JsonResult(new { success = true, message = "Delete successful" });
        }
        [HttpPost]
        public IActionResult SaveProducer(Producer producer)
        {
            bool result;
            if (producer.Id == 0)
                result = _producerRepo.Create(producer);
            else
                result = _producerRepo.Update(producer);

            if (result)
                return new JsonResult(new { success = true, message = "Successfully saved" });
            return new JsonResult(new { success = false, message = "Error while saving" });
        }
        public SelectList GetSelectList() => _producerRepo.GetSelectList();
    }
}
