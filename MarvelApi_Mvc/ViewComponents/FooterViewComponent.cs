using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Mvc.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
