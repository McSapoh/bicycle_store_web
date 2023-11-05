using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Models
{
    public class ShoppingCartOrder
    {
        [Key]
        public int Id { get; set; }
        public int BicycleId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
        public virtual Bicycle Bicycle { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
