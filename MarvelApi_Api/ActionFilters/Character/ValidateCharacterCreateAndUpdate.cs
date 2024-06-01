using System.Net;
using MarvelApi_Api.Data;
using MarvelApi_Api.Helpers;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MarvelApi_Api.ActionFilters.Character
{
    public class ValidateCharacterCreateAndUpdateAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApiFilterResponseHelper _responseHelper;
        private readonly ILogger<ValidateCharacterCreateAndUpdateAttribute> _logger;

        public ValidateCharacterCreateAndUpdateAttribute(ApplicationDbContext db, ApiFilterResponseHelper resposeHelper,
            ILogger<ValidateCharacterCreateAndUpdateAttribute> logger)
        {
            _dbContext = db;
            _responseHelper = resposeHelper;
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Entering {FilterName}", nameof(ValidateCharacterCreateAndUpdateAttribute));

            if (!context.ModelState.IsValid)
            {
                context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Information entered was not valid.");
                return;
            }

            if (context.ActionArguments["character"] is CharacterCreateDTO characterCreate)
            {
                if (await _dbContext.Characters.AnyAsync(x => x.Name == characterCreate.Name))
                {
                    context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, $"{characterCreate.Name} already exists.");
                    return;
                }

                var characterIds = characterCreate.AllyIds.Union(characterCreate.EnemyIds).ToList();
                if (!await CharacterIdsExist(context, characterIds))
                    return;                
            }

            else if (context.ActionArguments["character"] is CharacterUpdateDTO characterUpdate)
            {
                if (await _dbContext.Characters.AnyAsync(x => x.Id != characterUpdate.Id && x.Name == characterUpdate.Name))
                {
                    context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, $"{characterUpdate.Name} already exists.");
                    return;
                }

                var characterIds = characterUpdate.AllyIds.Union(characterUpdate.EnemyIds).ToList();
                if (!await CharacterIdsExist(context, characterIds))
                    return;                
            }
            await next();
        }

        private async Task<bool> CharacterIdsExist(ActionExecutingContext context, List<int> characterIds)
        {
            var existingCharacters = await _dbContext.Characters.Where(x => characterIds.Contains(x.Id)).Select(y => y.Id).ToListAsync();
            var missingIds = characterIds.Except(existingCharacters).ToList();

            if (missingIds.Any())
            {
                context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.NotFound,
                                                                                     missingIds.Select(id => $"Character with id {id} not found").ToList());
                return false;
            }
            return true;
        }
    }
}