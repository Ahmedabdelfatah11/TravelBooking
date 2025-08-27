using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

public class BookingSpecification : BaseSpecifications<Booking>
{
    // Constructor for getting booking by ID
    public BookingSpecification(int id) : base(b => b.Id == id)
    {
        AddIncludes();
    }

    // Constructor for getting all bookings (no filter)
    public BookingSpecification() : base()
    {
        AddIncludes();
    }

    // Constructor for filtering by BookingType only
    public BookingSpecification(BookingType bookingType) : base(b => b.BookingType == bookingType)
    {
        AddIncludes();
    }

    // Constructor for filtering by BookingType and FlightCompanyId
    public BookingSpecification(BookingType bookingType, int flightCompanyId)
        : base(b => b.BookingType == bookingType &&
                   b.FlightId.HasValue &&
                   b.Flight.FlightCompanyId == flightCompanyId)
    {
        AddIncludes();
        // Include Flight to access FlightCompanyId
        Includes.Add(b => b.Flight);
        Includes.Add(b => b.Flight.FlightCompany);
    }

    // Constructor for filtering by userId and bookingType
    public BookingSpecification(string userId, BookingType bookingType)
        : base(b => b.UserId == userId && b.BookingType == bookingType)
    {
        AddIncludes();
    }

    // Constructor for filtering by userId, bookingType, and flightCompanyId
    public BookingSpecification(string userId, BookingType bookingType, int flightCompanyId)
        : base(b => b.UserId == userId &&
                   b.BookingType == bookingType &&
                   b.FlightId.HasValue &&
                   b.Flight.FlightCompanyId == flightCompanyId)
    {
        AddIncludes();
        Includes.Add(b => b.Flight);
        Includes.Add(b => b.Flight.FlightCompany);
    }

    // Private method to add common includes
    private void AddIncludes()
    {
        Includes.Add(b => b.User);
        Includes.Add(b => b.Payment);
    }

    // Static factory methods to avoid constructor conflicts and improve clarity
    public static BookingSpecification ById(int id)
    {
        return new BookingSpecification(id);
    }

    public static BookingSpecification ByBookingType(BookingType bookingType)
    {
        return new BookingSpecification(bookingType);
    }

    // Factory method for flight company filtering - this replaces the conflicting constructor
    public static BookingSpecification ByFlightCompany(int flightCompanyId)
    {
        var spec = new BookingSpecification(BookingType.Flight, flightCompanyId);
        return spec;
    }

    public static BookingSpecification ByBookingTypeAndFlightCompany(BookingType bookingType, int flightCompanyId)
    {
        return new BookingSpecification(bookingType, flightCompanyId);
    }

    public static BookingSpecification ByUserAndBookingType(string userId, BookingType bookingType)
    {
        return new BookingSpecification(userId, bookingType);
    }

    public static BookingSpecification ByUserBookingTypeAndFlightCompany(string userId, BookingType bookingType, int flightCompanyId)
    {
        return new BookingSpecification(userId, bookingType, flightCompanyId);
    }
}