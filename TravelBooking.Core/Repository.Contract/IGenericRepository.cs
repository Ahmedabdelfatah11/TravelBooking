using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

namespace TravelBooking.Core.Repository.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Get operations
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
        Task<int> GetCountAsync(ISpecifications<T> spec);
        // using Dabashboard
        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task SaveChangesAsync();

        public Task<IReadOnlyList<HotelCompany>> GetHotelsByAdminIdAsync(string adminId);

        public Task<IReadOnlyList<CarRentalCompany>> GetCarRentalByAdminIdAsync(string adminId);
        public Task<IReadOnlyList<FlightCompany>> GetFlighByAdminIdAsync(string adminId);
        public Task<IReadOnlyList<TourCompany>> GetTourByAdminIdAsync(string adminId);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetQueryable();
    }

}
