
using System.Linq.Expressions;
using MarvelApi_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
            await SaveChangesAsync();
        }

        public async Task<T> DeleteAsync(int id)
        {
            var itemToRemove = await _dbSet.FindAsync(id);
            if (itemToRemove != null)
            {
                _dbSet.Remove(itemToRemove);
                await SaveChangesAsync();
                return itemToRemove;
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null){
                query = query.Where(filter).AsQueryable();
            }
            if (includeProperties != null){
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)){
                    query = query.Include(includeProp);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool isTracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null){
                query = query.Where(filter).AsQueryable();
            }
            if (!isTracked){
                query = query.AsNoTracking();
            }
            if (includeProperties != null){
                foreach (var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)){
                    query = query.Include(includeProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}