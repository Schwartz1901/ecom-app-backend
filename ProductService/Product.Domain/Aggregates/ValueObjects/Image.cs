using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Aggregates.ValueObjects
{
    public class Image: ValueObject
    {
        public string ImageUrl  { get; set; }
        public string ImageAlt { get; set; }
        public Image() { }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ImageUrl;
            yield return ImageAlt;
        }
    }
}
