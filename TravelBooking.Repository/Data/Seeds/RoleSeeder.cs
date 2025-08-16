using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Models;

namespace TravelBooking.Repository.Data.Seeds
{
 
    public static class RoleSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        { 


            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roles = {
                "User",
                "SuperAdmin",
                "HotelAdmin",
                "FlightAdmin",
                "CarRentalAdmin",
                "TourAdmin"
            };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }


            await CreateDefaultSuperAdmin(userManager);
        }

        private static async Task CreateDefaultSuperAdmin(UserManager<ApplicationUser> userManager)
        {
            string superAdminEmail = "superadmin@travelbooking.com";
            string superAdminPassword = "SuperAdmin@123";

            var existingSuperAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (existingSuperAdmin == null)
            {
                var superAdmin = new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    FirstName = "Super",
                    LastName = "Admin",
                    Address = "Default Address", 
                    DateOfBirth = new DateTime(1990, 1, 1)
                };

                var result = await userManager.CreateAsync(superAdmin, superAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }

            }
        }
    }
}