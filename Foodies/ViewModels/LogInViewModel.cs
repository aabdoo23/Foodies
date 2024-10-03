namespace Foodies.ViewModels
{
    public class LogInViewModel
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
