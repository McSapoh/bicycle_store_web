using System;
using System.Collections.Generic;

#nullable disable

namespace bicycle_store_web
{
    public partial class Bicycle
    {
        public Bicycle()
        {
            BicycleOrders = new HashSet<BicycleOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float WheelDiameter { get; set; }
        public uint Price { get; set; }
        public uint Quantity { get; set; }
        public int TypeId { get; set; }
        public int CountryId { get; set; }
        public int ProducerId { get; set; }

        public virtual Country Country { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<BicycleOrder> BicycleOrders { get; set; }
    }
}
