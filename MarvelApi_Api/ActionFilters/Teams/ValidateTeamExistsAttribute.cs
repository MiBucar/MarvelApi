using System.Net;
using MarvelApi_Api.ActionFilters.Character;
using MarvelApi_Api.Data;
using MarvelApi_Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.ActionFilters.Teams
{
    public class ValidateTeamExistsAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApiFilterResponseHelper _responseHelper;
        private readonly ILogger<ValidateTeamExistsAttribute> _logger;

        public ValidateTeamExistsAttribute(ApplicationDbContext dbContext, ApiFilterResponseHelper resposeHelper, ILogger<ValidateTeamExistsAttribute> logger)
        {
            _dbContext = dbContext;
            _responseHelper = resposeHelper;
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Entering {FilterName}", nameof(ValidateCharactersNotRelatedAttribute));

            if (context.ActionArguments.ContainsKey("id") && context.ActionArguments["id"] is int id)
            {
                var existingTeam = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Id == id);
                if (existingTeam == null)
                {
                    context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.NotFound, $"Team with id: {id} doesn't exist.");
                    return;
                }
                await next();
            }

            context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Id is required.");
            return;
        }
    }
}