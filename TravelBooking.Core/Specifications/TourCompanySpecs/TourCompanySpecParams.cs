using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Specifications
{
    public class TourCompanySpecParams
    {
        private const int MaxPageSize = 20; // or any suitable max size
        private int pageSize = 10;
        private int pageIndex = 1; // default page index

        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int PageIndex
        {
            get => pageIndex < 1 ? 1 : pageIndex;
            set => pageIndex = value;
        }
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

        public string? Sort { get; set; } // sorting, e.g. "nameAsc", "ratingDesc"

        private string? location;

        public string? Location
        {
            get { return location; }
            set { location = value?.ToLower(); }
        }
        public int? Rating { get; set; }
    }
}
