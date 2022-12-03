using bicycle_store_web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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
        public List<Producer> GetProducers() => _producerRepo.GetAll();
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
