using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

        // Added for SubmitQuiz task
        Task<IEntity?> FindAsync(Expression<Func<IEntity, bool>> predicate);
        Task<IEntity?> FindAsync(Expression<Func<IEntity, bool>> predicate, params Expression<Func<IEntity, object>>[] includes);
        Task<int> CountAsync(Expression<Func<IEntity, bool>> predicate);
    }
}
