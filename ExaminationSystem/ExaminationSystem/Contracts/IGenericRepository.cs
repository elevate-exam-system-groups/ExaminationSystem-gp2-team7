using ExaminationSystem.Models;

namespace ExaminationSystem.Contracts
{
    public interface IGenericRepository<IEntity> where IEntity : BaseEntity
    {
        IQueryable<IEntity> AsQueryable();
        Task<IEnumerable<IEntity>> GetAllAsync();
        Task<IEntity?> GetByIdAsync(Guid id);
        void Add(IEntity entity);
        void Update(IEntity entity);
        void SoftDelete(IEntity entity);
        void HardDelete(IEntity entity);
        void Attach(IEntity entity);
    }
}