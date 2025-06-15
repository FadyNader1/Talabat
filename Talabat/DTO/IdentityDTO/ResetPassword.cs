using System.ComponentModel.DataAnnotations;

namespace Talabat.DTO.IdentityDTO
{
    public class ResetPassword
    {
        public string Token { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string NewPassword { get; set; }
    }
}
