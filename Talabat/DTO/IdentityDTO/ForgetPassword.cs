using System.ComponentModel.DataAnnotations;

namespace Talabat.DTO.IdentityDTO
{
    public class ForgetPassword
    {

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}
