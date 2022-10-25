using System;
using System.Collections.Generic;

#nullable disable

namespace bicycle_store_web
{
    public partial class Order
    {
        public Order()
        {
            BicycleOrders = new HashSet<BicycleOrder>();
        }

        public int OrderId { get; set; }
        public int Cost { get; set; }
        public DateTime Data { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<BicycleOrder> BicycleOrders { get; set; }
    }
}
