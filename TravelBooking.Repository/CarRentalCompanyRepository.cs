using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Repository.Data;

namespace TravelBooking.Repository
{
    public class CarRentalCompanyRepository : ICarRentalCompanyRepository
    {
        private readonly AppDbContext _dbContext;

        public CarRentalCompanyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CarRentalCompany> GetByIdAsync(int id)
        {
            return await _dbContext.CarRentalCompanies.FindAsync(id);
        }

        public async Task DeleteAsync(CarRentalCompany company)
        {
            _dbContext.CarRentalCompanies.Remove(company);
            await _dbContext.SaveChangesAsync();
        }
    }
}
