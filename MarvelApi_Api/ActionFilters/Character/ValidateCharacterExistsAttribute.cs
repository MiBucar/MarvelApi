using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.ActionFilters.Character
{
    public class ValidateCharacterExistsAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _db;

        public ValidateCharacterExistsAttribute(ApplicationDbContext db)
        {
            _db = db;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idsToCheck = new List<int>();

            if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
            {
                idsToCheck.Add(id);
            }

            if (context.ActionArguments.TryGetValue("allyIds", out var allyIdsObj) && allyIdsObj is List<int> allyIds)
            {
                idsToCheck.AddRange(allyIds);
            }

            if (context.ActionArguments.TryGetValue("enemyIds", out var enemyIdsObj) && enemyIdsObj is List<int> enemyIds)
            {
                idsToCheck.AddRange(enemyIds);
            }

            foreach (var charId in idsToCheck)
            {
                if (!await _db.Characters.AnyAsync(x => x.Id == charId))
                {
                    ReturnNotFoundContext(context ,charId);
                    return;
                }
            }

            await next();
        }

        private void ReturnNotFoundContext(ActionExecutingContext context, int id)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false,
                ErrorMessages = new List<string> { $"Character with id {id} not found." }
            };
            context.Result = new JsonResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
        }
    }
}