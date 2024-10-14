namespace Foodies.ViewModels
{
    public class AddbranchViewmodel: RegistrationViewModel
    {
        public TimeSpan OpeningHour { get; set; }//0-23
        public TimeSpan ClosingHour { get; set; }
        public string? AddressId {  get; set; }

    }
}
