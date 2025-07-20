using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.DTOS
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } 
        public int count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pageindex , int pagesize,int count,IReadOnlyList<T>data)
        {
            this.PageIndex = pageindex;
            this.PageSize = pagesize;
            this.count = count;
            Data = data;
            
        }
    }
    
}
