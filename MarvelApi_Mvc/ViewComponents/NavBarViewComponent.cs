using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Mvc.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
