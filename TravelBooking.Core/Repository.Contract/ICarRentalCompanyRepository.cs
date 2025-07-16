using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Repository.Contract
{
    public interface ICarRentalCompanyRepository 
    {
        Task<CarRentalCompany> GetByIdAsync(int id);
        Task DeleteAsync(CarRentalCompany company);
    }
}
