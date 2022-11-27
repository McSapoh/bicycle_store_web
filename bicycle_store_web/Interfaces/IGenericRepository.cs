using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public T GetById(int Id);
        public List<T> GetAll();
        public bool Create(T item);
        public bool Update(T item);
        public bool Delete(int Id);
    }
}
