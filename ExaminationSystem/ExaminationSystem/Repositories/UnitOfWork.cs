using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;

namespace ExaminationSystem.Repositories
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
            if (_repositories.TryGetValue(EntityType, out object? repository))
                return (IGenericRepository<TEntity>)repository;

            var newRepo = new GenericRepository<TEntity>(_dbContext);

            _repositories[EntityType] = newRepo;
            return newRepo;

        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}