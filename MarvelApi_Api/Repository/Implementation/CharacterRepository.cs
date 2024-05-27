using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.Repository.Implementation
{
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CharacterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Character> AddAllyAsync(int characterId, List<int> allyIds)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingCharacter = await GetAsync(x => x.Id == characterId);

                    var existingAllies = await _dbContext.Characters
                        .Where(c => allyIds.Contains(c.Id))
                        .ToListAsync();

                    foreach (var character in existingAllies){
                        existingCharacter.Allies.Add(character);
                        character.Allies.Add(existingCharacter);
                    }                        

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return existingCharacter;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public  async Task<Character> AddEnemyAsync(int characterId, List<int> enemyIds)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingCharacter = await GetAsync(x => x.Id == characterId);

                    var existingEnemies = await _dbContext.Characters
                        .Where(c => enemyIds.Contains(c.Id))
                        .ToListAsync();

                    foreach (var character in existingEnemies){
                        existingCharacter.Enemies.Add(character);
                        character.Enemies.Add(existingCharacter);
                    }                        

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return existingCharacter;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task<Character> UpdateAsync(Character character)
        {
            character.DateUpdated = DateTime.UtcNow;
            _dbContext.Update(character);
            await _dbContext.SaveChangesAsync();
            return character;
        }
    }
}