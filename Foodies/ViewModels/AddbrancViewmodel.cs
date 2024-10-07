namespace Foodies.ViewModels
{
    public class AddbrancViewmodel: RegisterationViewModel
    {
        public int restid {  get; set; }
        public TimeSpan OpeningHour { get; set; }//0-23
        public TimeSpan ClosingHour { get; set; }
        public int addressid {  get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string img  { get; set; } 



    }
}
