using Microsoft.EntityFrameworkCore;
using System.Linq;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications.TourSpecs;

namespace TravelBooking.APIs.Extensions
{
    public static class TourQueryExtensions
    {
        public static IQueryable<Tour> ApplyFiltering(this IQueryable<Tour> query, TourSpecParams specParams)
        {
            if (!string.IsNullOrEmpty(specParams.Search))
                query = query.Where(t => t.Name.ToLower().Contains(specParams.Search));

            if (!string.IsNullOrEmpty(specParams.Destination))
                query = query.Where(t => t.Destination.ToLower() == specParams.Destination);

            if (specParams.Category != null && specParams.Category.Count > 0)
                query = query.Where(t => specParams.Category.Contains(t.Category.Value));

            return query;
        }

        public static async Task<(decimal MinPrice, decimal MaxPrice)> GetPriceBoundsAsync(this IQueryable<Tour> query)
        {
            var min = await query.MinAsync(t => t.Price);
            var max = await query.MaxAsync(t => t.Price);
            return (min, max);
        }
    }
}
