using System;
using System.Collections.Generic;

#nullable disable

namespace bicycle_store_web
{
    public partial class BicycleOrder
    {
        public int BicycleOrderId { get; set; }
        public int BicycleId { get; set; }
        public int Quantity { get; set; }
        public int BicycleCost { get; set; }
        public int OrderId { get; set; }        

        public virtual Bicycle Bicycles { get; set; }
        public virtual Order Order { get; set; }
    }
}
