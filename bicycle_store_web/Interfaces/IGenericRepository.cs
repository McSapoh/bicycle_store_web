using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public T GetById(int Id);
        public List<T> GetAll();
        public void Create(T item);
        public void Update(T item);
        public void Delete(int Id);
    }
}
