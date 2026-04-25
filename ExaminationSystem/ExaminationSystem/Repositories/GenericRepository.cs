using System.Linq.Expressions;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Repositories
{
    public class GenericRepository<IEntity>(ApplicationDbContext _dbContext) : IGenericRepository<IEntity> where IEntity : BaseEntity
    {
        public IQueryable<IEntity> AsQueryable()
        {
          return  _dbContext.Set<IEntity>();
        }
        public async Task<IEnumerable<IEntity>> GetAllAsync() => await _dbContext.Set<IEntity>().ToListAsync();
        public async Task<IEntity?> GetByIdAsync(Guid id) => await _dbContext.Set<IEntity>().FindAsync(id);

        public void Add(IEntity entity) => _dbContext.Set<IEntity>().Add(entity);

        public void Update(IEntity entity) => _dbContext.Set<IEntity>().Update(entity);

        public void SoftDelete(IEntity entity)
        {
            entity.DeletedAt = DateTime.UtcNow;
        }
        public void HardDelete(IEntity entity) => _dbContext.Set<IEntity>().Remove(entity);

        public void Attach(IEntity entity) => _dbContext.Set<IEntity>().Attach(entity);

        // Added for SubmitQuiz task
        public async Task<IEntity?> FindAsync(Expression<Func<IEntity, bool>> predicate)
            => await _dbContext.Set<IEntity>().FirstOrDefaultAsync(predicate);

        public async Task<IEntity?> FindAsync(Expression<Func<IEntity, bool>> predicate, params Expression<Func<IEntity, object>>[] includes)
        {
            IQueryable<IEntity> query = _dbContext.Set<IEntity>();
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<IEntity, bool>> predicate)
            => await _dbContext.Set<IEntity>().CountAsync(predicate);
    }
}
