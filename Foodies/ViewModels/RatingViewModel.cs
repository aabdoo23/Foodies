namespace Foodies.ViewModels
{
    public class RatingViewModel
    {
        public string RestaurantId { get; set; }
        
        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5.")]
        public decimal Rate { get; set; }
        public string? RestaurantName { get; set; }
    }
}
