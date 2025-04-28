using System.ComponentModel.DataAnnotations;

namespace user_service.domain
{
    public class User
    {
        [Key]
        public uint Id { get; init; }
        
        public required string Name { get; set; }
        
        public required string Email { get; set; }
        
        public required string Password { get; set; }
    }
}
