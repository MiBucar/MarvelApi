using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.ActionFilters.Character
{
    public class ValidateCharacterExistsAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiFilterResponseHelper _responseHelper;
        private readonly ILogger<ValidateCharacterExistsAttribute> _logger;

        public ValidateCharacterExistsAttribute(ApplicationDbContext db, ApiFilterResponseHelper resposeHelper, ILogger<ValidateCharacterExistsAttribute> logger)
        {
            _db = db;
            _responseHelper = resposeHelper;
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Entering {FilterName}", nameof(ValidateCharacterExistsAttribute));

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
                    context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.NotFound, $"Character with id {charId} not found.");
                    return;
                }
            }

            await next();
        }
    }
}