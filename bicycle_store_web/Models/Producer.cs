using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace bicycle_store_web
{
    public partial class Producer
    {
        public Producer()
        {
            Bicycles = new HashSet<Bicycle>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Bicycle> Bicycles { get; set; }
    }
}
