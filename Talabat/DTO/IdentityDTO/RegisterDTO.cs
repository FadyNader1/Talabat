using System.ComponentModel.DataAnnotations;

namespace Talabat.DTO.IdentityDTO
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(30)]
        public string Fname {  get; set; }

        [MaxLength(30)]
        [Required]
        public string Lname {  get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        public string Country { get; set; }
        public string City {  get; set; }

        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string Confirm_Password { get; set; }

    }
}
