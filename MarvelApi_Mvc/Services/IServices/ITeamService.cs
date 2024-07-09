using MarvelApi_Mvc.Models.DTOs.TeamDTOs;

namespace MarvelApi_Mvc.Services.IServices
{
    public interface ITeamService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(TeamCreateDTO teamCreateDTO);
        Task<T> UpdateAsync<T>(TeamUpdateDTO teamUpdateDTO);
        Task<T> DeleteAsync<T>(int id);
    }
}
