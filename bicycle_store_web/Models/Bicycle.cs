using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Please enter bicycle name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter wheel diameter")]
        public float WheelDiameter { get; set; }
        [Required(ErrorMessage = "Please enter bicycle price")]
        public uint Price { get; set; }
        [Required(ErrorMessage = "Please enter quantity of bicycles this model")]
        public uint Quantity { get; set; }
        public byte[] Photo { get; set; }
        public int TypeId { get; set; }
        public int CountryId { get; set; }
        public int ProducerId { get; set; }

        public virtual Country Country { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<BicycleOrder> BicycleOrders { get; set; }
    }
}
