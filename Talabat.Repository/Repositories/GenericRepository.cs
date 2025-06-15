using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Context;
using Talabat.Repository.Specifications;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : BaseEntity
    {
        private readonly TalabatContext context;

        public GenericRepository(TalabatContext context)
        {
            this.context = context;
        }

        private IQueryable<T> ApplyQuery(ISpecification<T> spec)
        {
          return  BuildQuery<T>.GetQuery(context.Set<T>(), spec);
        }

        public async Task AddAsync(T item)
        {
            await context.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            context.Set<T>().Remove(item);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
          return  await context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplyQuery(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplyQuery(spec).FirstOrDefaultAsync();
        }

        public void Update(T item)
        {
            context.Set<T>().Update(item);
        }
    }
}
