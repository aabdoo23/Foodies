namespace Foodies.ViewModels
{
    public class MenuViewModel
    {
        public Restaurant Restaurant { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public string SelectedCategory { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}
