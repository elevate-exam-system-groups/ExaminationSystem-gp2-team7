using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExaminationSystem.Models;

namespace E_Commerce.Domain.Contracts
{
    public interface IGenericRepository<IEntity> where IEntity : BaseEntity
    {
        Task<IEnumerable<IEntity>> GetAllAsync();
        Task<IEntity?> GetByIdAsync(Guid id);
        Task AddAsync(IEntity entity);
        void Update(IEntity entity);
        void Remove(IEntity entity);
    }
}
