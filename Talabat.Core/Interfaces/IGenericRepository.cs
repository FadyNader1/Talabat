using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Interfaces
{
    public interface IGenericRepository<T> 
    {
         Task<IReadOnlyList<T>> GetAllAsync();
         Task<IReadOnlyList<T>> GetAllSpecificationAsync(ISpecification<T> spec);
         Task<T> GetByIdAsync(int id);
         Task<T> GetByIdSpecificationAsync(ISpecification<T> spec);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);

    }
}
