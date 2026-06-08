using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models
{
    public class SignupModel
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]  
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords do not match")]
        public string? Password { get; set; }

        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
