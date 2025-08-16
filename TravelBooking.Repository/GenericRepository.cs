using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Repository.Data;

namespace TravelBooking.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenericRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        } 
         
        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();

        } 



        public async Task<IReadOnlyList<HotelCompany>> GetHotelsByAdminIdAsync(string adminId)
        {
            return await _dbContext.HotelCompanies
           .Include(h => h.Rooms)
           .ThenInclude(h => h.Images) // Eager load Rooms
           .Where(h => h.AdminId == adminId)
           .ToListAsync();
        }

        public async Task<IReadOnlyList<CarRentalCompany>> GetCarRentalByAdminIdAsync(string adminId)
        {
            return await _dbContext.CarRentalCompanies
           .Include(cr => cr.Cars)
            .Where(cr => cr.AdminId == adminId)
            .ToListAsync();
        }

        public async Task<IReadOnlyList<FlightCompany>> GetFlighByAdminIdAsync(string adminId)
        {
            return await _dbContext.FlightCompanies
           .Include(t => t.Flights)
            .Where(cr => cr.AdminId == adminId)
            .ToListAsync();
        }

        public async Task<IReadOnlyList<TourCompany>> GetTourByAdminIdAsync(string adminId)
        {
            return await _dbContext.TourCompanies
          .Include(t => t.Tours)
           .Where(cr => cr.AdminId == adminId)
           .ToListAsync();
        }



        public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().CountAsync(predicate);
        }
        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

    }
    }
