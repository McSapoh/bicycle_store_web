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
        public string ValidateProducer(Producer producer)
        {
            var producerFromDb = _producerRepo.GetById(producer.Id);
            if (producerFromDb != null)
            {
                if (producer.Name == producerFromDb.Name)
                    if (producer.Description == producerFromDb.Description)
                        return "There is no changes in current producer";
                return null;
            }
            return "Producer is not exists in database";
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
        public bool DeleteProducer(int Id)
        {
            _producerRepo.Delete(Id);
            if (_producerRepo.GetById(Id) == null)
                return true;
            else
                return false;
        }
        [HttpPost]
        public bool SaveProducer(Producer producer)
        {
            if (producer.Id == 0)
            {
                _producerRepo.Create(producer);
                if (_producerRepo.GetById(producer.Id) != null)
                    return true;
            }
            else
            {
                _producerRepo.Update(producer);
                if (_producerRepo.GetById(producer.Id) != null)
                    return true;
            }
            return false;
        }
        public SelectList GetSelectList() => _producerRepo.GetSelectList();
    }
}
