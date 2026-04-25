using System.Linq.Expressions;
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

        // Added for SubmitQuiz task
        Task<IEntity?> FindAsync(Expression<Func<IEntity, bool>> predicate);
        Task<IEntity?> FindAsync(Expression<Func<IEntity, bool>> predicate, params Expression<Func<IEntity, object>>[] includes);
        Task<int> CountAsync(Expression<Func<IEntity, bool>> predicate);
    }
}