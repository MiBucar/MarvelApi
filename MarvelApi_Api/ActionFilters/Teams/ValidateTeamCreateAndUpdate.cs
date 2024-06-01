using System.Net;
using MarvelApi_Api.ActionFilters.Character;
using MarvelApi_Api.Data;
using MarvelApi_Api.Helpers;
using MarvelApi_Api.Models.DTOs.Team;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.ActionFilters.Teams
{
    public class ValidateTeamCreateAndUpdate : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApiFilterResponseHelper _responseHelper;
        private readonly ILogger<ValidateTeamCreateAndUpdate> _logger;

        public ValidateTeamCreateAndUpdate(ApplicationDbContext dbContext, ApiFilterResponseHelper responseHelper, ILogger<ValidateTeamCreateAndUpdate> logger)
        {
            _dbContext = dbContext;
            _responseHelper = responseHelper;
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Entering {FilterName}", nameof(ValidateCharactersNotRelatedAttribute));

            if (!context.ModelState.IsValid)
            {
                context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Entered properties are not valid.");
                return;
            }

            if (context.ActionArguments.TryGetValue("teamCreateDTO", out var createDto) && createDto is TeamCreateDTO teamCreateDTO)
            {
                if (await TeamNameExists(teamCreateDTO.Name))
                {
                    context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.NotAcceptable, $"Team with name: {teamCreateDTO.Name} already exists.");
                    return;
                }

                if (!await ValidateMemberIdsExist(context, teamCreateDTO.MemberIds))
                    return;
            }

            if (context.ActionArguments.TryGetValue("teamUpdateDTO", out var updateDto) && updateDto is TeamUpdateDTO teamUpdateDTO)
            {
                if (context.ActionArguments.ContainsKey("id") && context.ActionArguments["id"] is int id){
                    if (id != teamUpdateDTO.Id)
                    {
                        context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Ids don't match.");
                        return;
                    }
                }

                if (!await TeamIdExists(teamUpdateDTO.Id))
                {
                    context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, $"Team with id: {teamUpdateDTO.Id} doesn't exist.");
                    return;
                }

                if (await OtherTeamWithNameExists(teamUpdateDTO.Id, teamUpdateDTO.Name))
                {
                    context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, $"Team with name: {teamUpdateDTO.Name} already exists.");
                    return;
                }

                if (!await ValidateMemberIdsExist(context, teamUpdateDTO.MemberIds))
                    return;
            }

            await next();
        }

        private async Task<bool> TeamNameExists(string name)
        {
            return await _dbContext.Teams.AnyAsync(x => x.Name == name);
        }

        private async Task<bool> TeamIdExists(int id)
        {
            return await _dbContext.Teams.AnyAsync(x => x.Id == id);
        }

        private async Task<bool> OtherTeamWithNameExists(int id, string name)
        {
            return await _dbContext.Teams.AnyAsync(x => x.Id != id && x.Name == name);
        }

        private async Task<bool> ValidateMemberIdsExist(ActionExecutingContext context, List<int> memberIds)
        {
            var existingMembers = await _dbContext.Characters.Where(x => memberIds.Contains(x.Id)).ToListAsync();

            if (existingMembers.Count != memberIds.Count)
            {
                var missingIds = memberIds.Except(existingMembers.Select(x => x.Id)).ToList();
                var errorMessages = missingIds.Select(id => $"Member with id: {id} doesn't exist").ToList();
                context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessages);
                return false;
            }
            return true;
        }
    }
}