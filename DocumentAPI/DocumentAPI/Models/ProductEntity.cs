
using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Models
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
    }
}
