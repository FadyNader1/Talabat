using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductParams
    {
        public int? Brand_Id { get; set; }
        public int? Type_Id { get; set; }
        public string? SearchByName { get; set; }
        public const int MaxPageSize = 8;
        private int pageSize = 5;
        public int PageSze
        {
            get {  return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageIndex { get; set; } = 1;


    }
}
