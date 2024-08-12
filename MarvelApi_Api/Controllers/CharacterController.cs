using System.Net;
using AutoMapper;
using MarvelApi_Api.ActionFilters.Character;
using MarvelApi_Api.Helpers;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using MarvelApi_Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _autoMapper;
        private readonly ApiResponseHelper _responseHelper;
        private readonly ILogger<CharacterController> _logger; 

        public CharacterController(ICharacterRepository characterRepository, IMapper autoMapper, ApiResponseHelper responseHelper,
            ILogger<CharacterController> logger)
        {
            _characterRepository = characterRepository;
            _autoMapper = autoMapper;
            _responseHelper = responseHelper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacters()
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(GetCharacters));

                var characters = await _characterRepository.GetAllAsync(includeProperties: "CharacterRelationships,Team");
                var mappedCharacters = _autoMapper.Map<IEnumerable<Character>, IEnumerable<CharacterDTO>>(characters);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacters, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("GetAllies/{id:int}")]
		[ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
		public async Task<IActionResult> GetAllies(int id)
        {
            var allies = await _characterRepository.GetAllies(id);
			var mappedAllies = _autoMapper.Map<IEnumerable<Character>, IEnumerable<CharacterDTO>>(allies);

			var response = _responseHelper.GetApiResponseSuccess(mappedAllies, HttpStatusCode.OK);
			return Ok(response);
        }

		[HttpGet("GetEnemies/{id:int}")]
		[ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
		public async Task<IActionResult> GetEnemies(int id)
		{
			var allies = await _characterRepository.GetEnemies(id);
			var mappedAllies = _autoMapper.Map<IEnumerable<Character>, IEnumerable<CharacterDTO>>(allies);

			var response = _responseHelper.GetApiResponseSuccess(mappedAllies, HttpStatusCode.OK);
			return Ok(response);
		}

		[HttpGet("{query}")]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(Search));

                if (string.IsNullOrEmpty(query))
                    return Ok(new List<Character>());

                var characters = await _characterRepository.GetAllAsync(x => x.Name.Contains(query), includeProperties: "CharacterRelationships,Team");
                var mappedCharacters = _autoMapper.Map<IEnumerable<Character>, IEnumerable<CharacterDTO>>(characters);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacters, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        public async Task<IActionResult> GetCharacter(int id)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(GetCharacter));

                var character = await _characterRepository.GetAsync(x => x.Id == id, includeProperties: "CharacterRelationships");
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateCharacterCreateAndUpdateAttribute))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreateDTO character)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(CreateCharacter));

                var allyIds = character.AllyIds.ToList();
                var enemyIds = character.EnemyIds.ToList();

                var mappedCharacter = _autoMapper.Map<CharacterCreateDTO, Character>(character);
                await _characterRepository.CreateAsync(mappedCharacter);

                if (allyIds.Any())
                    await _characterRepository.AddAllyAsync(mappedCharacter.Id, allyIds);
                if (enemyIds.Any())
                    await _characterRepository.AddEnemyAsync(mappedCharacter.Id, enemyIds);

                var characterToReturn = _autoMapper.Map<CharacterDTO>(mappedCharacter);

                var response = _responseHelper.GetApiResponseSuccess(characterToReturn, HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetCharacter), new { id = characterToReturn.Id }, response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{id:int}/allies")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharactersNotRelatedAttribute))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAllies(int id, [FromBody] List<int> allyIds)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(AddAllies));

                var character = await _characterRepository.AddAllyAsync(id, allyIds);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{id:int}/enemies")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharactersNotRelatedAttribute))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEnemies(int id, [FromBody] List<int> enemyIds)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(AddEnemies));

                var character = await _characterRepository.AddEnemyAsync(id, enemyIds);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{id:int}/team")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTeamToCharacter(int id, [FromBody] int teamId)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(AddTeamToCharacter));

                var character = await _characterRepository.AddTeamToCharacter(teamId, id);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharacterCreateAndUpdateAttribute))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCharacter(int id, [FromBody] CharacterUpdateDTO character)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(UpdateCharacter));

                var updatedCharacter = await _characterRepository.UpdateAsync(character);
                var characterToReturn = _autoMapper.Map<CharacterDTO>(updatedCharacter);

                var response = _responseHelper.GetApiResponseSuccess(characterToReturn, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            try
            {
                _logger.LogInformation("Accessing {endpoint} endpoint", nameof(DeleteCharacter));

                var character = await _characterRepository.DeleteAsync(id);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception ex)
        {
            _logger.LogError("{message}", ex.Message);
            var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }
    }
}