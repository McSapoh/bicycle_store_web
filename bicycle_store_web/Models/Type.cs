using System;
using System.Collections.Generic;

#nullable disable

namespace bicycle_store_web
{
    public partial class Type
    {
        public Type()
        {
            Bicycles = new HashSet<Bicycle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Bicycle> Bicycles { get; set; }
    }
}
