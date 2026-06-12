using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models
{
    public class ApplicationModel : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 2,
         ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Last Name must be between 2 and 50 characters")]
        public string? LastName { get; set; }
    }
}
