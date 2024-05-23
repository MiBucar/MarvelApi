using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Hero;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MarvelApi_Api.ActionFilters.Hero
{
    public class ValidateHeroPropertiesAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;

        public ValidateHeroPropertiesAttribute(ApplicationDbContext db)
        {
            _dbContext = db;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var response = new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Information enetered was not valid." }
                };

                context.Result = new JsonResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
                return;
            }
            if (context.ActionArguments["hero"] is HeroCreateDTO heroCreate && _dbContext.Heroes.Any(x => x.Name == heroCreate.Name))
            {
                SetBadRequestResult(context, heroCreate.Name);
            }
            else if (context.ActionArguments["hero"] is HeroUpdateDTO heroUpdate && _dbContext.Heroes.Any(x => x.Name == heroUpdate.Name))
            {
                SetBadRequestResult(context, heroUpdate.Name);
            }
        }

        public void SetBadRequestResult(ActionExecutingContext context, string heroName){
            var response = new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> {$"Character already exists: {heroName}"}
                };

                context.Result = new JsonResult(response) {StatusCode = (int)HttpStatusCode.BadRequest};
                return;
        }
    }
}