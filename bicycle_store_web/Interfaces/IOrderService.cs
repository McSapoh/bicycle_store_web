using bicycle_store_web.Models;
using System.Collections.Generic;

namespace bicycle_store_web.Interfaces
{
    public interface IOrderService
    {
        public List<BicycleOrder> GetAdminOrders();
        public List<BicycleOrder> GetUserOrders(int UserId);
        public int GetTotalCost(List<ShoppingCartOrder> CartOrders);
        public bool CreateOrder(int UserId);
        public bool SendOrder(int OrderId);
        public bool ConfirmReceipt(int OrderId);
    }
}
