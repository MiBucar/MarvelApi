using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.ActionFilters.Character
{
    public class ValidateCharactersNotRelatedAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApiFilterResponseHelper _responseHelper;
        private readonly ILogger<ValidateCharactersNotRelatedAttribute> _logger;

        public ValidateCharactersNotRelatedAttribute(ApplicationDbContext dbContext, ApiFilterResponseHelper responseHelper,
            ILogger<ValidateCharactersNotRelatedAttribute> logger)
        {
            _dbContext = dbContext;
            _responseHelper = responseHelper;
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Entering {FilterName}", nameof(ValidateCharactersNotRelatedAttribute));
            if (context.ActionArguments.ContainsKey("id"))
            {
                var characterId = (int)context.ActionArguments["id"];
                var character = await _dbContext.Characters.Include(x => x.CharacterRelationships).FirstOrDefaultAsync(z => z.Id == characterId);

                if (context.ActionArguments.ContainsKey("allyIds") && context.ActionArguments["allyIds"] is List<int> allyIds)
                {
                    var existingRelations = _dbContext.CharacterRelationships.Where(x => x.CharacterId == characterId &&
                                                                                                                                        allyIds.Contains(x.RelatedCharacterId)).ToList();
                   if (existingRelations.Any()){
                        CreateConflictResult(context, existingRelations.Select(x => x.RelatedCharacterId).ToList());
                        return;
                   }
                }

                if (context.ActionArguments.ContainsKey("enemyIds") && context.ActionArguments["enemyIds"] is List<int> enemyIds)
                {
                    var existingRelations = _dbContext.CharacterRelationships.Where(x => x.CharacterId == characterId &&
                                                                                                                                        enemyIds.Contains(x.RelatedCharacterId)).ToList();
                   if (existingRelations.Any()){
                        CreateConflictResult(context, existingRelations.Select(x => x.RelatedCharacterId).ToList());
                        return;
                   }
                }
            }
            await next();
        }

        private void CreateConflictResult(ActionExecutingContext context, List<int> ids)
        {
            context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.Conflict, 
                                                    ids.Select(x => $"Relation already exists with character of id: {x}").ToList());
        }
    }
}