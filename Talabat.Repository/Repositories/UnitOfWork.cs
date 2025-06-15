using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Repository.Context;

namespace Talabat.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TalabatContext context;
        private Hashtable Repositories;

        public UnitOfWork(TalabatContext context)
        {
            Repositories = new Hashtable();
            this.context = context;
        }
        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;
            if (!Repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<T>(context);
                Repositories.Add(type, repository);
            }
            return Repositories[type] as IGenericRepository<T>;

        }
    }
}
