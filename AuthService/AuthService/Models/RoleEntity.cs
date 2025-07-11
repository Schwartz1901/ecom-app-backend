
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class RoleEntity
    {
        [Key]
        public int Id { get; set; }
        public string RoleDescription { get; set; }
        public string RoleNotation { get; set; }
        public int NumberOfUser {  get; set; }
        public int abcdef { get; set; }
    }
}
