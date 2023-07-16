using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IGenericRepository<T> where T :BaseEntity
    {
        Task<T>GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T>GetEntityWithSpecification(ISpecification<T> specifications);
        Task<IReadOnlyList<T>>ListAsync(ISpecification<T> specification);
        Task<int> CountAsync(ISpecification<T> spec);
        Task Add(T entity);
       
        Task DeleteWhere(Expression<Func<T, bool>> predicate = null);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
        void SaveEntities(CancellationToken cancellationToken = default(CancellationToken));
    }
}
