using ExaminationSystem.Models;

namespace ExaminationSystem.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellation = default);

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    }
}