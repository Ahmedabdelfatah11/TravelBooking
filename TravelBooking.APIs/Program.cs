using AutoMapper;
using ContactUsAPI.Services;
using Jwt.Helper;
using Jwt.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using TravelBooking.Core.Interfaces_Or_Repository;
using TravelBooking.Core.Models.Services;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Services;
using TravelBooking.Core.Settings;
using TravelBooking.Extensions;
using TravelBooking.Helper;
using TravelBooking.Models;
using TravelBooking.Repository;
using TravelBooking.Repository.Data;

using TravelBooking.Repository.Data.Seeds; 
using TravelBooking.Service.Services; 

using TravelBooking.Service.Services.Dashboard;


namespace TravelBooking.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var webApplicationbuilder = WebApplication.CreateBuilder(args);


            webApplicationbuilder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            webApplicationbuilder.Services.AddOpenApi();
            webApplicationbuilder.Services.AddSwaggerServices();
            webApplicationbuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //inject the AuthService as IAuthService
            webApplicationbuilder.Services.AddScoped<IAuthService, AuthService>();

            // add EmailService as IEmailSender
            webApplicationbuilder.Services.AddScoped<IEmailSender, EmailService>();

            // Add the PaymentService as IPaymentService
            webApplicationbuilder.Services.AddScoped<IPaymentService, PaymentService>();

            webApplicationbuilder.Services.AddScoped<IRoomService, RoomService>();

            // add SmtpEmailService as IEmailService
            webApplicationbuilder.Services.AddScoped<IEmailService, SmtpEmailService>();
            // Enhanced ChatBot Services
            webApplicationbuilder.Services.AddScoped<GeminiService>();
            webApplicationbuilder.Services.AddScoped<ChatHistoryService>();
            webApplicationbuilder.Services.AddScoped<MultiRetrieverService>();
            webApplicationbuilder.Services.AddScoped<ChatService>();
            webApplicationbuilder.Services.AddScoped<DatabaseChatIntegrationService>();

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
                        ValidAudience = webApplicationbuilder.Configuration["JWT:Audience"],
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
            webApplicationbuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            webApplicationbuilder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            webApplicationbuilder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // or Preserve
                    options.JsonSerializerOptions.WriteIndented = true; // optional, for readability
                });
            //webApplicationbuilder.Services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = null; // or ReferenceHandler.IgnoreCycles
            //});

            // adding Email service
           webApplicationbuilder.Services.AddTransient<IEmailSender, EmailService>();

            webApplicationbuilder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:53517", "http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                     .AllowCredentials();
                    //policy.AllowAnyOrigin()
                    // .AllowAnyHeader()
                    // .AllowAnyMethod();
                });
            });
            // Caching
            webApplicationbuilder.Services.AddMemoryCache();
            webApplicationbuilder.Services.AddResponseCaching();

            

            // Dashboard
            webApplicationbuilder.Services.AddScoped<IDashboardService, DashboardService>();
            //Dasboard Hotel Admin
            webApplicationbuilder.Services.AddScoped<IHotelAdminDashboardService, HotelAdminDashboardService>();
            //Dasboard Tour Admin
            webApplicationbuilder.Services.AddScoped<ITourAdminDashboardService, TourAdminDashboardService>();
            //Dasboard Flight Admin
            webApplicationbuilder.Services.AddScoped<IFlightAdminDashboardService, FlightAdminDashboardService>();
            //Dasboard Car Admin
            webApplicationbuilder.Services.AddScoped<ICarRentalAdminDashboardService, CarRentalAdminDashboardService>();

            webApplicationbuilder.Services.AddApplicationServices();


            var app = webApplicationbuilder.Build();

            app.UseCors("AllowAngularApp");
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            using var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            var logger = Services.GetRequiredService<ILogger<Program>>();
            var _dbcontext = Services.GetRequiredService<AppDbContext>();

            try
            {
                await _dbcontext.Database.MigrateAsync();
                await RoleSeeder.SeedAsync(Services); // assigning roles to the database 
                await FlightContextSeed.SeedAsync(_dbcontext);
                await TravelContextSeed.SeedAsync(_dbcontext); 

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
            // Response Caching

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapStaticAssets();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseResponseCaching();
            app.MapControllers();
            app.MapFallbackToFile("index.html"); // <--- this makes /users load Angular

            app.Run();
        }
    }
}
