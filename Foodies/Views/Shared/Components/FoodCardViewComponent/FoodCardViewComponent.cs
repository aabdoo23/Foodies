using Foodies.ViewModels.Components;
using Microsoft.AspNetCore.Mvc;

[ViewComponent(Name = "FoodCardViewComponent")]
public class FoodCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Customer customer, int id, string title, string rating, string imageUrl, string url)
    {
        // Create a new view model and populate it with the provided data
        var viewModel = new FoodCardViewModel
        {
            Customer = customer, // Pass the Customer object
            Id = id,
            Url = url,
            Title = title,
            ImageUrl = imageUrl,
            Rating = rating
        };

        return View(viewModel); // Return the view with the populated view model
    }
}
