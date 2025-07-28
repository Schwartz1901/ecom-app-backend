using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Product.Domain.Aggregates.Enumerations
{
    internal class Category : Enumeration
    {
        public static Category Herb = new (1, nameof(Herb));
        public static Category Tea = new(2, nameof(Tea));
        public static Category Incense = new(3, nameof(Incense));
        public static Category Oil = new(4, nameof(Oil));
        public static Category Misc = new(10, nameof(Misc));

        public Category(int id, string name) : base (id, name) { }
        
    }
}
