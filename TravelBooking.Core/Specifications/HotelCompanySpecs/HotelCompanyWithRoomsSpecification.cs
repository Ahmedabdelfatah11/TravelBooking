using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.HotelCompanySpecs
{
    public class HotelCompanyWithRoomsSpecification : BaseSpecifications<HotelCompany>
    {
        //Get All + Search + Sort + Paging
        public HotelCompanyWithRoomsSpecification(HotelCompanySpecParams specParams)
            : base(h =>
                string.IsNullOrEmpty(specParams.Search) ||
                h.Name.ToLower().Contains(specParams.Search.ToLower()) ||
                h.Location.ToLower().Contains(specParams.Search.ToLower()) ||
                h.Description.ToLower().Contains(specParams.Search.ToLower()))
        {
            // Replace these lines in the "Get by Id" constructor:
            // Includes.Add("Rooms");
            // Includes.Add("Rooms.RoomImages");

            // With these lines:
            Includes.Add(h => h.Rooms);
            AddInclude("Rooms.Images");
          



            // Pagination
            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            // Sorting
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(h => h.Name);
                        break;
                    case "namedesc":
                        AddOrderByDesc(h => h.Name);
                        break;
                    case "locationasc":
                        AddOrderBy(h => h.Location);
                        break;
                    case "locationdesc":
                        AddOrderByDesc(h => h.Location);
                        break;
                    case "ratingasc":
                        AddOrderBy(h => h.Rating);
                        break;
                    case "ratingdesc":
                        AddOrderByDesc(h => h.Rating);
                        break;
                    default:
                        AddOrderBy(h => h.Id);
                        break;
                }
            }
        }

        // Get by Id
        public HotelCompanyWithRoomsSpecification(int id)
            : base(h => h.Id == id)
        {
            Includes.Add(h => h.Rooms);
            AddInclude("Rooms.Images");

        }
    }
}


