using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password must be between 8 and 100 characters")]
        public string? Password { get; set; }
    }
}
