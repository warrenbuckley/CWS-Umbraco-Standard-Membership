using DataAnnotationsExtensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CWSUmbracoStandardMembership.Models
{
    public class ChangePasswordViewModel
    {
        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your current password")]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters long")]
        public string CurrentPassword { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters long")]
        public string Password { get; set; }

        [UIHint("Password")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [EqualTo("Password", ErrorMessage = "Your passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}