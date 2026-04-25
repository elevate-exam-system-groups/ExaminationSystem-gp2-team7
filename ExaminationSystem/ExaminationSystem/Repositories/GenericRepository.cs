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
        public async Task<IEntity?> GetByIdAsync(Guid id)=> await _dbContext.Set<IEntity>().FindAsync(id);
        public void Remove(IEntity entity)=> _dbContext.Set<IEntity>().Remove(entity);
        public void Update(IEntity entity)=> _dbContext.Set<IEntity>().Update(entity);
    }
}