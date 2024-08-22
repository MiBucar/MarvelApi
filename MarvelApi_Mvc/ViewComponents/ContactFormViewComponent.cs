using MarvelApi_Mvc.Models.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Mvc.ViewComponents
{
    public class ContactFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new MailDTO();
            return View(model);
        }
    }
}
