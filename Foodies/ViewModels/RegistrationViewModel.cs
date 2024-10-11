namespace Foodies.ViewModels
{

    //merging UserIdentity required Info and BaseUser in one Model
    public class RegistrationViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? img { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Identical")]
        public string ConfirmPassword { get; set; }
        public string? phoneNumber { get; set; } = string.Empty;
        public string? City { get; set; }
    
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Location { get; set; }
    }
}
