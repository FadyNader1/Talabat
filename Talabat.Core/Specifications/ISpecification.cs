using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>> WHere { get; set; }
        public List<Expression<Func<T, object>>> INclude { get; set; }

    }
}
