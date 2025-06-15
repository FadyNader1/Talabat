using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public static class BuildQuery<T> where T:BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> basequery,ISpecification<T> spec)
        {
            var query = basequery;
            if (spec.WHere is not null)
                query = query.Where(spec.WHere);

          query=spec.INclude.Aggregate(query, (current, ex) => current.Include(ex));
            return query;
        }
    }
}
