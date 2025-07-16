
using Jwt.Helper;
using TravelBooking.Models;
using Microsoft.IdentityModel.Tokens;
using Jwt.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TravelBooking.Core.Services;
using TravelBooking.Extensions;
using TravelBooking.Repository.Data;
using TravelBooking.Core.Models.Services;
using TravelBooking.Core.Settings;

namespace TravelBooking.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationbuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            webApplicationbuilder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            webApplicationbuilder.Services.AddOpenApi();
            webApplicationbuilder.Services.AddSwaggerServices();

            //inject the AuthService as IAuthService
            webApplicationbuilder.Services.AddScoped<IAuthService, AuthService>();

            // add EmailService as IEmailSender
            webApplicationbuilder.Services.AddScoped<IEmailSender, EmailService>();


            //add Accessor for User
            webApplicationbuilder.Services.AddHttpContextAccessor();

            webApplicationbuilder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("DefaultConnection")));

            // Register JWT configuration and map it from appsettings.json
           webApplicationbuilder.Services.Configure<JWT>(webApplicationbuilder.Configuration.GetSection("JWT"));

            // Register Identity services
           webApplicationbuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            //adding Email service
           webApplicationbuilder.Services.Configure<MailSettings>(webApplicationbuilder.Configuration.GetSection(nameof(MailSettings)));

            // adding AutoMapper 
           webApplicationbuilder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

            // Configure JWT authentication
           webApplicationbuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = webApplicationbuilder.Configuration["JWT:Issuer"],
                        ValidAudience =webApplicationbuilder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationbuilder.Configuration["JWT:Key"]))
                    };

                });
           webApplicationbuilder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings  
                options.Password.RequiredLength = 8;

                //options.SignIn.RequireConfirmedEmail= true; // Optional: Require confirmed email for sign-in
                options.User.RequireUniqueEmail = true; // Ensure unique email addresses
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            });
            // adding Email service
           //webApplicationbuilder.Services.AddTransient<IEmailSender, EmailService>();

            webApplicationbuilder.Services.AddApplicationServices();
            var app = webApplicationbuilder.Build();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            using var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            var _dbcontext = Services.GetRequiredService<AppDbContext>();

            var logger = Services.GetRequiredService<ILogger<Program>>();
            try
            {
                //await StoredContextSeed.SeedAsync(_dbcontext);
                await _dbcontext.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration");
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization(); 
            app.MapControllers();

            app.Run();
        }
    }
}
