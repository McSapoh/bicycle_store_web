using bicycle_store_web.Models;

namespace bicycle_store_web.Interfaces
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        public int GetShoppingCartId(int Id);
    }
}
