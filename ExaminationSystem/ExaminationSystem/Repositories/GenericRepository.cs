using System.Linq.Expressions;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Repositories
{
    public class GenericRepository<IEntity>(ApplicationDbContext _dbContext) : IGenericRepository<IEntity> where IEntity : BaseEntity
    {
        public async void Add(IEntity entity) => await _dbContext.Set<IEntity>().AddAsync(entity);

        public IQueryable<IEntity> AsQueryable()
        {
            return _dbContext.Set<IEntity>();
        }
        public IQueryable<IEntity> GetAll() => _dbContext.Set<IEntity>();
        public async Task<IEntity?> GetByIdAsync(Guid id)
            => await _dbContext.Set<IEntity>()
                               .AsNoTracking()
                               .FirstOrDefaultAsync(e => e.Id == id);
        public void Remove(IEntity entity) => _dbContext.Set<IEntity>().Remove(entity);
        public void Update(IEntity entity) => _dbContext.Set<IEntity>().Update(entity);

        public void SoftDelete(IEntity entity)
        {
            entity.DeletedAt = DateTime.UtcNow;
        }
        public void HardDelete(IEntity entity) => _dbContext.Set<IEntity>().Remove(entity);

        public void Attach(IEntity entity) => _dbContext.Set<IEntity>().Attach(entity);


        public async Task<int> CountAsync(Expression<Func<IEntity, bool>> predicate)
            => await _dbContext.Set<IEntity>().CountAsync(predicate);

        Task<IQueryable<IEntity>> IGenericRepository<IEntity>.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}



