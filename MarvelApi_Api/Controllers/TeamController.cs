using System.Net;
using AutoMapper;
using MarvelApi_Api.ActionFilters.Teams;
using MarvelApi_Api.Helpers;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Team;
using MarvelApi_Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _autoMapper;
        private readonly ApiResponseHelper _apiResponseHelper;
        public TeamController(ITeamRepository teamRepository, ICharacterRepository characterRepository, IMapper autoMapper,
                                                ApiResponseHelper apiResponseHelper)
        {
            _teamRepository = teamRepository;
            _characterRepository = characterRepository;
            _autoMapper = autoMapper;
            _apiResponseHelper = apiResponseHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            try
            {
                var teams = await _teamRepository.GetAllAsync(includeProperties: "Members");
                var mappedTeam = _autoMapper.Map<IEnumerable<TeamDTO>>(teams);

                var response = _apiResponseHelper.GetApiResponseSuccess(mappedTeam, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _apiResponseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateTeamExistsAttribute))]
        public async Task<IActionResult> GetTeam(int id)
        {
            try
            {
                var team = await _teamRepository.GetAsync(x => x.Id == id);
                var mappedTeam = _autoMapper.Map<TeamDTO>(team);

                return Ok(_apiResponseHelper.GetApiResponseSuccess(mappedTeam, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                var response = _apiResponseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateTeamCreateAndUpdate))]
        public async Task<IActionResult> CreateTeam([FromBody] TeamCreateDTO teamCreateDTO)
        {
            try
            {
                var team = _autoMapper.Map<Team>(teamCreateDTO);
                await _teamRepository.CreateAsync(team);

                if (teamCreateDTO.MemberIds.Any())
                    await _characterRepository.AddTeamsToCharacter(team.Id, teamCreateDTO.MemberIds);

                var mappedTeam = _autoMapper.Map<TeamDTO>(team);

                var response = _apiResponseHelper.GetApiResponseSuccess(mappedTeam, HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, response);
            }
            catch (Exception ex)
            {
                var response = _apiResponseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidateTeamCreateAndUpdate))]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamUpdateDTO teamUpdateDTO)
        {
            try
            {
                var team = await _teamRepository.UpdateAsync(teamUpdateDTO);
                var mappedTeam = _autoMapper.Map<TeamDTO>(team);

                return Ok(_apiResponseHelper.GetApiResponseSuccess(mappedTeam, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                var response = _apiResponseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidateTeamExistsAttribute))]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                var deletedTeam = await _teamRepository.DeleteAsync(id);
                var mappedTeam = _autoMapper.Map<TeamDTO>(deletedTeam);

                var response = _apiResponseHelper.GetApiResponseSuccess(mappedTeam, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _apiResponseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}