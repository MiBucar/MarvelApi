using System.Runtime.InteropServices;
using AutoMapper;
using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace MarvelApi_Api.Repository.Implementation
{
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _autoMapper;

        public CharacterRepository(ApplicationDbContext dbContext, IMapper autoMapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _autoMapper = autoMapper;
        }
        public async Task<Character> AddAllyAsync(int characterId, List<int> allyIds)
        {
            var character = await _dbContext.Characters.FindAsync(characterId);

            if (character != null)
            {
                await AddRelationshipAsync(character, allyIds, false);
                await SaveChangesAsync();
                return character;
            }

            return character;
        }

        public async Task<Character> AddEnemyAsync(int characterId, List<int> enemyIds)
        {
            var character = await _dbContext.Characters.FindAsync(characterId);

            if (character != null)
            {
                await AddRelationshipAsync(character, enemyIds, true);
                await SaveChangesAsync();
                return character;
            }

            return new Character();
        }

        public async Task<Character> UpdateAsync(CharacterUpdateDTO characterDto)
        {
            var existingCharacter = await _dbContext.Characters.Include(x => x.CharacterRelationships).ThenInclude(cr => cr.RelatedCharacter)
                                                                .FirstOrDefaultAsync(y => y.Id == characterDto.Id);

            await RemoveRelationshipAsync(existingCharacter);

            _autoMapper.Map(characterDto, existingCharacter);

            await AddRelationshipAsync(existingCharacter, characterDto.AllyIds, isEnemy: false);
            await AddRelationshipAsync(existingCharacter, characterDto.EnemyIds, isEnemy: true);

            existingCharacter.DateUpdated = DateTime.UtcNow;
            _dbContext.Update(existingCharacter);
            await _dbContext.SaveChangesAsync();

            return existingCharacter;
        }

        public override async Task<Character> DeleteAsync(int id)
        {
            var existingCharacter = await _dbContext.Characters.Include(x => x.CharacterRelationships).FirstOrDefaultAsync(y => y.Id == id);

            await RemoveRelationshipAsync(existingCharacter);

            _dbContext.Characters.Remove(existingCharacter);

            await SaveChangesAsync();
            return existingCharacter;
        }

        public async Task<Character> AddTeamToCharacter(int teamId, int characterId)
        {
            var existingCharacter = await _dbContext.Characters.FirstOrDefaultAsync(x => x.Id == characterId);

            if (existingCharacter != null)
            {
                existingCharacter.TeamId = teamId;
                await SaveChangesAsync();
                return existingCharacter;
            }

            return new Character();
        }

        public async Task AddTeamsToCharacter(int teamId, List<int> characterIds)
        {
            var characters = _dbContext.Characters.Where(x => characterIds.Contains(x.Id));

            foreach (var character in characters)
            {
                character.TeamId = teamId;
                character.DateUpdated = DateTime.UtcNow;
            }

            await SaveChangesAsync();
        }

        #region Helper Functions
        private async Task AddRelationshipAsync(Character character, List<int> relatedCharacterIds, bool isEnemy)
        {
            foreach (var id in relatedCharacterIds)
            {
                var relatedCharacter = await _dbContext.Characters.Include(x => x.CharacterRelationships).FirstOrDefaultAsync(y => y.Id == id);

                if (relatedCharacter != null)
                {
                    var relationship = new CharacterRelationship
                    {
                        Character = character,
                        CharacterId = character.Id,
                        RelatedCharacter = relatedCharacter,
                        RelatedCharacterId = id,
                        IsEnemy = isEnemy
                    };
                    character.CharacterRelationships.Add(relationship);

                    var reverseRelationship = new CharacterRelationship
                    {
                        Character = relatedCharacter,
                        CharacterId = id,
                        RelatedCharacter = character,
                        RelatedCharacterId = character.Id,
                        IsEnemy = isEnemy
                    };
                    relatedCharacter.CharacterRelationships.Add(reverseRelationship);

                    _dbContext.CharacterRelationships.Add(relationship);
                    _dbContext.CharacterRelationships.Add(reverseRelationship);
                }
            }
        }

        private async Task RemoveRelationshipAsync(Character character)
        {
            var currentRelationships = character.CharacterRelationships.ToList();

            foreach (var relationship in currentRelationships)
            {
                var relatedCharacter = await _dbContext.Characters
                    .Include(x => x.CharacterRelationships)
                    .FirstOrDefaultAsync(y => y.Id == relationship.RelatedCharacterId);

                if (relatedCharacter != null)
                {
                    var reverseRelationship = relatedCharacter.CharacterRelationships
                        .FirstOrDefault(x => x.RelatedCharacterId == character.Id);

                    if (reverseRelationship != null)
                    {
                        _dbContext.CharacterRelationships.Remove(reverseRelationship);
                    }
                }

                _dbContext.CharacterRelationships.Remove(relationship);
            }
        }
        #endregion
    }
}