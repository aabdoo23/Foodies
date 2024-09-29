using Foodies.ViewModels.Components;
using Microsoft.AspNetCore.Mvc;

[ViewComponent(Name = "FoodCardViewComponent")]
public class FoodCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string title, /*string subTitle, string description,*/ /*string price,*/ string rating, string imageUrl, string url)
    {
        var viewModel = new FoodCardViewModel { Url = url, Title = title/*, SubTitle = subTitle, Description = description*/,/* Price = price,*/ ImageUrl=imageUrl, Rating = rating };
        return View(viewModel);
    }
}
