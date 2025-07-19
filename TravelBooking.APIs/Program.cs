
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TalabatAPIs.Extensions;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Extensions;
using TravelBooking.APIs.Helper;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Extensions;
using TravelBooking.Helper;
using TravelBooking.Repository;
using TravelBooking.Repository.Data;

namespace TravelBooking.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationbuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            webApplicationbuilder.Services.AddAutoMapper(typeof(MappingProfiles));

            webApplicationbuilder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            webApplicationbuilder.Services.AddOpenApi();
            webApplicationbuilder.Services.AddSwaggerServices();

            webApplicationbuilder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            webApplicationbuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            webApplicationbuilder.Services.AddAutoMapper(typeof(Program).Assembly);

            webApplicationbuilder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
                            //webApplicationbuilder.Services.AddControllers()
                //.AddJsonOptions(options =>
                //{
                //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                //});
            webApplicationbuilder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = null; // or ReferenceHandler.IgnoreCycles
            });


            webApplicationbuilder.Services.AddApplicationServices();
       

            var app = webApplicationbuilder.Build();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            using var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            var _dbcontext = Services.GetRequiredService<AppDbContext>();

            var logger = Services.GetRequiredService<ILogger<Program>>();
            try
            {
                await _dbcontext.Database.MigrateAsync();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
