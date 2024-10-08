namespace Foodies.ViewModels
{
    public class AdminRegisterViewModel : RegisterationViewModel
    {
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string Hotline { get; set; }
        public string CuisineType { get; set; }

          public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
    }
}
