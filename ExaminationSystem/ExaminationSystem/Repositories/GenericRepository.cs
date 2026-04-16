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
    public class GenericRepository<IEntity> : IGenericRepository<IEntity> where IEntity : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(IEntity entity)=> await _dbContext.Set<IEntity>().AddAsync(entity);
        public async Task<IEnumerable<IEntity>> GetAllAsync() => await _dbContext.Set<IEntity>().ToListAsync();
        public async Task<IEntity?> GetByIdAsync(Guid id)=> await _dbContext.Set<IEntity>().FindAsync(id);
        public void Remove(IEntity entity)=> _dbContext.Set<IEntity>().Remove(entity);
        public void Update(IEntity entity)=> _dbContext.Set<IEntity>().Update(entity);
    }
}
