using ExaminationSystem.Models;

namespace ExaminationSystem.Contracts
{
    public interface IGenericRepository<IEntity> where IEntity : BaseEntity
    {
        Task<IEnumerable<IEntity>> GetAllAsync();
        Task<IEntity?> GetByIdAsync(Guid id);
        Task AddAsync(IEntity entity);
        void Update(IEntity entity);
        void Remove(IEntity entity);
        IQueryable<IEntity> GetQueryable();
    }
}
