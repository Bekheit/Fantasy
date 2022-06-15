using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }
        public string TeamName { get; set; }
    }
}
