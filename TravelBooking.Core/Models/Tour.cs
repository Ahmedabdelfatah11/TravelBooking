using Org.BouncyCastle.Crypto.Signers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public enum TourCategory
    {
        Adventure,
        Relaxation,
        Cultural,
        Nature,
        Historical
    }
    public class Tour : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; } =DateTime.UtcNow;
        public DateTime EndDate { get; set; } 
        public string? Description { get; set; } 
        public string? Destination { get; set; } // Destination of the Tour (e.g., city or country)
        public int MaxGuests { get; set; } // Maximum allowed guests for the Tour
        public int MinGroupSize { get; set; } // Minimum allowed group size
        public int MaxGroupSize { get; set; }
        public decimal Price { get; set; } // Total price of the Tour 
        public string? ImageUrl { get; set; } // Primary image for quick access

        public TourCategory? Category { get; set; }
        public string Languages { get; set; }
        [ForeignKey("TourCompany")]
        public int? TourCompanyId { get; set; } // Foreign key for the associated tour company

        public TourCompany? TourCompany { get; set; } // Navigation property for the tour company

        public ICollection<TourImage>? TourImages { get; set; } = new List<TourImage>();
        public ICollection<TourTicket> TourTickets { get; set; } = new List<TourTicket>();
        public List<string> IncludedItems { get; set; } = new();
        public List<string> ExcludedItems { get; set; } = new();
        public List<TourQuestion> Questions { get; set; } = new();

        public decimal GetPrice(Dictionary<string, int> ticketSelections)
        {
            if (ticketSelections == null || !ticketSelections.Any())
                return 0;

            decimal totalPrice = 0;

            foreach (var selection in ticketSelections)
            {
                string ticketType = selection.Key;
                int quantity = selection.Value;

                if (quantity <= 0)
                    continue;

                var ticket = TourTickets.FirstOrDefault(t =>
                    t.Type.Equals(ticketType, StringComparison.OrdinalIgnoreCase) && t.IsActive);

                if (ticket == null)
                    throw new InvalidOperationException($"Ticket type '{ticketType}' is not available or inactive.");

                if (quantity > ticket.AvailableQuantity)
                    throw new InvalidOperationException($"Requested quantity for '{ticketType}' exceeds the maximum allowed ({ticket.AvailableQuantity}).");

                totalPrice += ticket.Price * quantity;
            }

            return totalPrice;
        }
    }
}
