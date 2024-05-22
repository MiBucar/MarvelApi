using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.ActionFilters.Hero
{
    public class ValidateHeroExistsAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _db;

        public ValidateHeroExistsAttribute(ApplicationDbContext db)
        {
            _db = db;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = (int)context.ActionArguments["id"];
                var hero = await _db.Heroes.FirstOrDefaultAsync(x => x.Id == id);
                if (hero == null)
                {
                    var response = new ApiResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> {"Not Found."}
                    };
                    context.Result = new JsonResult(response) {StatusCode = (int)HttpStatusCode.NotFound};
                    return;
                }
            }
            await next();
        }
    }
}