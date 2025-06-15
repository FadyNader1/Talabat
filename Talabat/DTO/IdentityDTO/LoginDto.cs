using System.ComponentModel.DataAnnotations;

namespace Talabat.DTO.IdentityDTO
{
    public class LoginDto
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
