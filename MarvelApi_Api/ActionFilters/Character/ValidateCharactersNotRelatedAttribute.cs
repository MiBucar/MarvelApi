using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.ActionFilters.Character
{
    public class ValidateCharactersNotRelatedAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;

        public ValidateCharactersNotRelatedAttribute(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.ContainsKey("id"))
            {
                var characterId = (int)context.ActionArguments["id"];
                var character = await _dbContext.Characters.Include(x => x.Allies).Include(y => y.Enemies).FirstOrDefaultAsync(z => z.Id == characterId);

                if (context.ActionArguments.ContainsKey("allyIds") && context.ActionArguments["allyIds"] is List<int> allyIds)
                {
                    foreach (var id in allyIds)
                    {
                        var existingChar = await _dbContext.Characters.FirstOrDefaultAsync(x => x.Id == id);
                        if (character.Allies.Select(x => x.Id).Contains(id))
                        {
                            CreateConflictResult(context, character.Name, existingChar.Name, "allies");
                            return;
                        }
                    }
                }

                if (context.ActionArguments.ContainsKey("enemyIds") && context.ActionArguments["enemyIds"] is List<int> enemyIds)
                {
                    foreach (var id in enemyIds)
                    {
                        var existingChar = await _dbContext.Characters.FirstOrDefaultAsync(x => x.Id == id);
                        if (character.Enemies.Select(x => x.Id).Contains(id))
                        {
                            CreateConflictResult(context, character.Name, existingChar.Name, "enemies");
                            return;
                        }
                    }
                }
            }
            await next();
        }

        private void CreateConflictResult(ActionExecutingContext context, string charOne, string charTwo, string relationType)
        {
            var response = new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotAcceptable,
                ErrorMessages = new List<string> { $"{charOne} and {charTwo} are already {relationType}." }
            };
            context.Result = new JsonResult(response) { StatusCode = (int)HttpStatusCode.NotAcceptable };
        }
    }
}