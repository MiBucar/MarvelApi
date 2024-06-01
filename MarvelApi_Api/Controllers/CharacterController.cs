using System.Net;
using AutoMapper;
using MarvelApi_Api.ActionFilters.Character;
using MarvelApi_Api.Helpers;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using MarvelApi_Api.Repository;
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

        public CharacterController(ICharacterRepository characterRepository, IMapper autoMapper, ApiResponseHelper responseHelper)
        {
            _characterRepository = characterRepository;
            _autoMapper = autoMapper;
            _responseHelper = responseHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacters()
        {
            try
            {
                var characters = await _characterRepository.GetAllAsync(includeProperties: "CharacterRelationships,Team");
                var mappedCharacters = _autoMapper.Map<IEnumerable<Character>, IEnumerable<CharacterDTO>>(characters);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacters, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        public async Task<IActionResult> GetCharacter(int id)
        {
            try
            {
                var character = await _characterRepository.GetAsync(x => x.Id == id, includeProperties: "CharacterRelationships");
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateCharacterCreateAndUpdateAttribute))]
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreateDTO character)
        {
            try
            {
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
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("{id:int}/allies")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharactersNotRelatedAttribute))]
        public async Task<IActionResult> AddAllies(int id, [FromBody] List<int> allyIds)
        {
            try
            {
                var character = await _characterRepository.AddAllyAsync(id, allyIds);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("{id:int}/enemies")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharactersNotRelatedAttribute))]
        public async Task<IActionResult> AddEnemies(int id, [FromBody] List<int> enemyIds)
        {
            try
            {
                var character = await _characterRepository.AddEnemyAsync(id, enemyIds);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("{id:int}/team")]
        public async Task<IActionResult> AddTeamToCharacter(int id, [FromBody] int teamId)
        {
            try
            {
                var character = await _characterRepository.AddTeamToCharacter(teamId, id);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharacterCreateAndUpdateAttribute))]
        public async Task<IActionResult> UpdateCharacter(int id, [FromBody] CharacterUpdateDTO character)
        {
            try
            {
                var updatedCharacter = await _characterRepository.UpdateAsync(character);
                var characterToReturn = _autoMapper.Map<CharacterDTO>(updatedCharacter);

                var response = _responseHelper.GetApiResponseSuccess(characterToReturn, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            try
            {
                var character = await _characterRepository.DeleteAsync(id);
                var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

                var response = _responseHelper.GetApiResponseSuccess(mappedCharacter, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = _responseHelper.GetApiReponseNotSuccess(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}