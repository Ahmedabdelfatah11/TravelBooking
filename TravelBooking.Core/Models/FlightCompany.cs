

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Models;

namespace TravelBooking.Core.Models
{
    public class FlightCompany : BaseEntity
    {
        
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [Display(Name = "Company Name")]
        public string? Name { get; set; }

       
        //[StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters.")]
        //[DataType(DataType.MultilineText)]
        public string? Description { get; set; } 

       
        //[RegularExpression(@"^(https?:\/\/).*\.(jpg|jpeg|png|gif|bmp|webp)$",
        //    ErrorMessage = "Invalid image URL format. Must be a valid image URL ending with .jpg, .jpeg, .png, .gif, .bmp, or .webp.")]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

       
        //[StringLength(200, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 200 characters.")]
        public string? Location { get; set; } 
 
        public string? Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>(); // Navigation property for related flights


        public ICollection<Favoritet> favoritets { get; set; } = new List<Favoritet>();

        public ICollection<Review> reviews { get; set; } = new List<Review>();

        public string? AdminId { get; set; } // FK to AspNetUsers
        public ApplicationUser? Admin { get; set; } // Navigation Property
    }
}
