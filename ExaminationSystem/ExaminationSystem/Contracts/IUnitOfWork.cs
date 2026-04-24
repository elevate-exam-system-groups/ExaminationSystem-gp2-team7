using ExaminationSystem.Models;

namespace ExaminationSystem.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    }
}
