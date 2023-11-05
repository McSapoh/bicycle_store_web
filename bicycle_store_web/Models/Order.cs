using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace bicycle_store_web
{
    public partial class Order
    {
        public Order()
        {
            BicycleOrders = new HashSet<BicycleOrder>();
        }
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int OrderId { get; set; }
        public int Cost { get; set; }
        public DateTime Data { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<BicycleOrder> BicycleOrders { get; set; }
    }
}
