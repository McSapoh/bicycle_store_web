using bicycle_store_web.Models;
using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IShoppingCartOrderRepository : IGenericRepository<ShoppingCartOrder>
    {
        public List<ShoppingCartOrder> GetAll(int ShoppingCartId);
        public ShoppingCartOrder GetById(int Id, int BicycleId);
        public bool CheckExistence(int ShoppingCartId);
        public bool CheckExistence(int ShoppingCartId, int BicycleId);
    }
}
