using bicycle_store_web.Models;
using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IShoppingCartService
    {
        public void CreateShopingCart(int UserId);
        public bool AddToShoppingCart(int BicycleId, int UserId);
        public List<ShoppingCartOrder> GetShoppingCart(int UserId);
        public int GetShoppingCartId(int UserId);
        public bool RemoveFromShoppingCart(int ShoppingCartOrderId);
        public void ClearShoppingCart(int UserId);
    }
}
