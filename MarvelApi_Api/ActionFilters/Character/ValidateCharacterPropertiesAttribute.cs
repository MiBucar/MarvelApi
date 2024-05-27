using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Character;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MarvelApi_Api.ActionFilters.Character
{
    public class ValidateCharacterPropertiesAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;

        public ValidateCharacterPropertiesAttribute(ApplicationDbContext db)
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
            if (context.ActionArguments["character"] is CharacterCreateDTO characterCreate && _dbContext.Characters.Any(x => x.Name == characterCreate.Name))
            {
                SetBadRequestResult(context, characterCreate.Name);
            }
            else if (context.ActionArguments["character"] is CharacterUpdateDTO characterUpdate && _dbContext.Characters.Any(x => x.Name == characterUpdate.Name))
            {
                SetBadRequestResult(context, characterUpdate.Name);
            }
        }

        public void SetBadRequestResult(ActionExecutingContext context, string characterName){
            var response = new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> {$"Character already exists: {characterName}"}
                };

                context.Result = new JsonResult(response) {StatusCode = (int)HttpStatusCode.BadRequest};
                return;
        }
    }
}