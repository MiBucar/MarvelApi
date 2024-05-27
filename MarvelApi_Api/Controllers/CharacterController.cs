using System.Net;
using AutoMapper;
using MarvelApi_Api.ActionFilters.Character;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Character;
using MarvelApi_Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Api.Controllers
{
    [Controller]
    [Route("controller")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _autoMapper;

        public CharacterController(ICharacterRepository characterRepository, IMapper autoMapper)
        {
            _characterRepository = characterRepository;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacters()
        {
            var characters = await _characterRepository.GetAllAsync(includeProperties: "CharacterRelationships");
            var mappedCharacters = _autoMapper.Map<IEnumerable<Character>, IEnumerable<CharacterDTO>>(characters);
            return Ok(mappedCharacters);
        }

        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        public async Task<IActionResult> GetCharacter(int id)
        {
            var character = await _characterRepository.GetAsync(x => x.Id == id, includeProperties: "CharacterRelationships");
            var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

            var response = new ApiResponse
            {
                Result = mappedCharacter,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            };

            return Ok(response);           
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateCharacterPropertiesAttribute))]
        // Validate entered allies and enemies exist
        // Override CreateCharacter method in repo
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreateDTO character)
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

            var response = new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = characterToReturn
            };

            return CreatedAtAction(nameof(GetCharacter), new { id = characterToReturn.Id }, response);
        }

        [HttpPost("{id:int}/allies")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharactersNotRelatedAttribute))]
        public async Task<IActionResult> AddAllies(int id, [FromBody] List<int> allyIds)
        {
            var character = await _characterRepository.AddAllyAsync(id, allyIds);
            var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

            var response = new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = mappedCharacter
            };

            return Ok(response);
        }

        [HttpPost("{id:int}/enemies")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharactersNotRelatedAttribute))]
        public async Task<IActionResult> AddEnemies(int id, [FromBody] List<int> enemyIds)
        {
            var character = await _characterRepository.AddEnemyAsync(id, enemyIds);
            var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

            var response = new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = mappedCharacter
            };

            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        [ServiceFilter(typeof(ValidateCharacterPropertiesAttribute))]
        public async Task<IActionResult> UpdateCharacter(int id, [FromBody] CharacterUpdateDTO character)
        {
            var updatedCharacter = await _characterRepository.UpdateAsync(character);
            var characterToReturn = _autoMapper.Map<CharacterDTO>(updatedCharacter);

            var response = new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = characterToReturn
            };

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidateCharacterExistsAttribute))]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var character = await _characterRepository.DeleteAsync(id);
            var mappedCharacter = _autoMapper.Map<CharacterDTO>(character);

            var response = new ApiResponse()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = mappedCharacter
            };

            return Ok(response);
        }
    }
}