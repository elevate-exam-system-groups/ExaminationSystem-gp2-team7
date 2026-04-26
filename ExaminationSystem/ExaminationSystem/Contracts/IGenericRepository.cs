using ExaminationSystem.Models;

namespace ExaminationSystem.Contracts
{
    public interface IGenericRepository<IEntity> where IEntity : BaseEntity
    {
        IQueryable<IEntity> AsQueryable();
        Task<IQueryable<IEntity>> GetAllAsync();
        Task<IEntity?> GetByIdAsync(Guid id);
        void Add(IEntity entity);
        void Update(IEntity entity);
        void Remove(IEntity entity);
    }
}