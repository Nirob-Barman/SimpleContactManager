using System.ComponentModel.DataAnnotations;

namespace SimpleContactManager.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "The Id field is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "The PhoneNumber field is required.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The PhoneNumber field must be between 5 and 15 characters.")]
        [RegularExpression(@"^[\d\s\+\-\(\)]+$", ErrorMessage = "The PhoneNumber field contains invalid characters. It can include digits, spaces, plus signs, hyphens, and parentheses.")]
        public string? PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        public string? Email { get; set; }
        [StringLength(250, ErrorMessage = "The Address field cannot exceed 250 characters.")]
        public string? Address { get; set; }
    }
}
