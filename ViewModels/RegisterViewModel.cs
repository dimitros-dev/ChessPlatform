using System.ComponentModel.DataAnnotations;

namespace ChessPlatform.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your username.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username length should be between 3 and 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can contain only letters, numbers and underscores.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password length should be 8 or more characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Confirmed password does not match the original password.")]
        public string ConfirmedPassword { get; set; }

        [Required(ErrorMessage = "Please enter your email adress.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
