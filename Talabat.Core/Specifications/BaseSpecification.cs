using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> WHere { get; set; }
        public List<Expression<Func<T, object>>> INclude { get; set; }
        public int Skip { get ; set ; }
        public int Take { get; set; }
        public bool EnablePaggination { get ; set; }

        public BaseSpecification()
        {
            INclude = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecification(Expression<Func<T, bool>> wh)
        {
            WHere = wh;
            INclude = new List<Expression<Func<T, object>>>();
        }
        public void ApplyPaggination(int Skip,int Take)
        {
            EnablePaggination = true;
            this.Skip = Skip;
            this.Take = Take;
        }

    }
}
