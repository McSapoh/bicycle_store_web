using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            ShoppingCartOrders = new HashSet<ShoppingCartOrder>();
        }
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ShoppingCartOrder> ShoppingCartOrders { get; set; }
    }
}
