using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<T> Repository<T>() where T : BaseEntity;
        public Task<int> Complete();    }
}
