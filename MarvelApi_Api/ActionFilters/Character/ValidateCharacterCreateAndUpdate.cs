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

        public ValidateCharacterCreateAndUpdateAttribute(ApplicationDbContext db, ApiFilterResponseHelper resposeHelper)
        {
            _dbContext = db;
            _responseHelper = resposeHelper;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Information enetered was not valid.");
                return;
            }
            if (context.ActionArguments["character"] is CharacterCreateDTO characterCreate && await _dbContext.Characters.AnyAsync(x => x.Name == characterCreate.Name))
            {
                var characterIds = characterCreate.AllyIds.Union(characterCreate.EnemyIds).ToList();
                if (!await CharacterIdsExist(context, characterIds))
                    return;

                context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, characterCreate.Name);
                return;
            }
            else if (context.ActionArguments["character"] is CharacterUpdateDTO characterUpdate && await _dbContext.Characters.AnyAsync(x => x.Id != characterUpdate.Id && x.Name == characterUpdate.Name))
            {
                var characterIds = characterUpdate.AllyIds.Union(characterUpdate.EnemyIds).ToList();
                if (!await CharacterIdsExist(context, characterIds))
                    return;

                context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, characterUpdate.Name);
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