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
        IQueryable<IEntity> AsQueryable();
        Task<IEnumerable<IEntity>> GetAllAsync();
        Task<IEntity?> GetByIdAsync(Guid id);
        void Add(IEntity entity);
        void Update(IEntity entity);
        void SoftDelete(IEntity entity);
        void HardDelete(IEntity entity);
    }
}