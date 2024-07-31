using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;

namespace MarvelApi_Mvc.Services.IServices
{
    public interface ICharacterService
    {
         Task<T> GetAllAsync<T>();
         Task<T> GetAsync<T>(int id);
         Task<T> CreateAsync<T>(CharacterCreateDTO characterCreateDTO);
         Task<T> UpdateAsync<T>(CharacterUpdateDTO characterUpdateDTO);
         Task<T> DeleteAsync<T>(int id);
         Task<T> SearchAsync<T>(string query);
    }
}