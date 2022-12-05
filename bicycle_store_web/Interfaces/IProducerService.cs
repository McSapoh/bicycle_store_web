using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IProducerService
    {
        public Producer GetById(int Id);
        public List<Producer> GetProducers();
        public bool DeleteProducer(int Id);
        public bool SaveProducer(Producer producer);
        public SelectList GetSelectList();
    }
}
