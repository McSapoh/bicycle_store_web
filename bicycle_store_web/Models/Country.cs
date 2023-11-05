using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace bicycle_store_web
{
    public partial class Country
    {
        public Country()
        {
            Bicycles = new HashSet<Bicycle>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Bicycle> Bicycles { get; set; }
    }
}
