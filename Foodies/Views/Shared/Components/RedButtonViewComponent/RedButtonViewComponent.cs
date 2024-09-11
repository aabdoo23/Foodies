using Foodies.ViewModels.Components;
using Microsoft.AspNetCore.Mvc;

[ViewComponent(Name = "ButtonViewComponent")]
public class ButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string text, string icon, string url)
    {
        var viewModel = new ButtonViewModel { Text = text, Icon = icon, Url = url };
        return View(viewModel);
    }
}
