using System.Linq.Expressions;

namespace MarvelApi_Api.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool isTracked = true, string? includeProperties = null);
        Task<T> CreateAsync(T obj);
        Task<T> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}