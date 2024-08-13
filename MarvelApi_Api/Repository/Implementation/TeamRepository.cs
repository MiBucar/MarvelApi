using System.Reflection;
using AutoMapper;
using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using MarvelApi_Api.Models.DTOs.Team;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;

namespace MarvelApi_Api.Repository.Implementation
{
	public class TeamRepository : Repository<Team>, ITeamRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _autoMapper;
        public TeamRepository(ApplicationDbContext dbContext, IMapper autoMapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _autoMapper = autoMapper;
        }

        public async Task<Team> UpdateAsync(TeamUpdateDTO teamUpdateDTO)
        {
            var existingTeam = _dbContext.Teams.Include(x => x.Members).FirstOrDefault(y => y.Id == teamUpdateDTO.Id);
            if (existingTeam != null)
            {
                existingTeam.DateUpdated = DateTime.UtcNow;
                _autoMapper.Map(teamUpdateDTO, existingTeam);

                var existingMembers = existingTeam.Members.Select(x => x.Id).ToList();
                var membersToAdd = teamUpdateDTO.MemberIds.Except(existingMembers).ToList();
                var membersToRemove = existingMembers.Except(teamUpdateDTO.MemberIds).ToList();

                var charactersToRemove = await _dbContext.Characters.Where(x => membersToRemove.Contains(x.Id)).ToListAsync();
                RemoveCharactersFromTeam(charactersToRemove);

                var charactersToAdd = await _dbContext.Characters.Where(x => membersToAdd.Contains(x.Id)).ToListAsync();
                foreach (var character in charactersToAdd)
                    if (character != null)
                    {
                        character.TeamId = existingTeam.Id;
                        character.DateUpdated = DateTime.UtcNow;
                    }

                await SaveChangesAsync();
                return existingTeam;
            }
            return null;
        }

        public override async Task<Team> DeleteAsync(int id)
        {
            var charactersToRemove = await _dbContext.Characters.Where(x => x.TeamId == id).ToListAsync();
            RemoveCharactersFromTeam(charactersToRemove);

            return await base.DeleteAsync(id);
        }
    
        private void RemoveCharactersFromTeam(List<Character> characters){
            foreach (var character in characters)
                    if (character != null)
                    {
                        character.TeamId = null;
                        character.DateUpdated = DateTime.UtcNow;
                    }
        }

		public async Task<IEnumerable<Character>> GetMembersAsync(int id)
		{
            var existingTeam = await _dbContext.Teams.Include(x => x.Members).FirstOrDefaultAsync(y => y.Id == id);
            if (existingTeam != null)
                return existingTeam.Members.ToList();

            return new List<Character>();
		}
	}
}