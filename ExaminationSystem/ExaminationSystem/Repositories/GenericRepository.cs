using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Domain.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Repositories
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

    }
}