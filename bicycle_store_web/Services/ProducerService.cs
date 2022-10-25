using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Services
{
    public class ProducerService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        private readonly bicycle_storeContext _db;
        public ProducerService(IWebHostEnvironment WebHostEnvironment, bicycle_storeContext context)
        {
            this.WebHostEnvironment = WebHostEnvironment;
            _db = context;
        }
        [HttpGet]
        public Producer GetProducer(int id)
        {
            var producer = _db.Producers.FirstOrDefault(p => p.Id == id);
            if (producer == null)
                return null;
            else
                return producer;
        }
        [HttpGet]
        public IActionResult GetProducers()
        {
            var list = _db.Producers.Select(p => new {
                p.Id,
                p.Name,
                p.Description
            }).ToList();
            return new JsonResult(new { data = list });
        }
        [HttpPost]
        public IActionResult DeleteProducer(int Id)
        {
            var producerFromDb = _db.Producers.FirstOrDefault(t => t.Id == Id);
            if (producerFromDb == null)
            {
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            }
            _db.Producers.Remove(producerFromDb);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Delete successful" });
        }

        [HttpPost]
        public IActionResult SaveProducer(Producer producer)
        {
            if (producer.Id == 0)
                _db.Producers.Add(producer);
            else
                _db.Producers.Update(producer);
            _db.SaveChanges();
            return new JsonResult(new { success = true, message = "Successfully saves" });
        }
        public SelectList GetProducerSelectList() => new SelectList(_db.Producers.ToList(), "Id", "Name");
    }
}
