using bicycle_store_web.Models;
using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IShoppingCartOrderService
    {
        public List<ShoppingCartOrder> GetShoppingCartOrders(int ShoppingCartId);
        public bool SaveShoppingCartOrder(int BicycleId, int ShoppingCartId);
        public bool DeleteShoppingCartOrder(int ShoppingCartOrderId);
    }
}
