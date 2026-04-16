using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Domain.Contracts;

using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;

namespace E_Commerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var EntityType = typeof(TEntity); 
            if(_repositories.TryGetValue(EntityType,out object? repository))
                return (IGenericRepository<TEntity>)repository;

            var newRepo = new GenericRepository<TEntity>(_dbContext); 

            _repositories[EntityType] = newRepo;
            return newRepo;

        }


        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
