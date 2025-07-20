using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Errors;
using TravelBooking.Repository;

namespace TravelBooking.Extensions
{
    public static class ApplicationServicesExtension
    {
        
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(configuration =>
            {
                configuration.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            /// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            ///builder.Services.AddOpenApi();
            services.Configure<ApiBehaviorOptions>(options =>
            {

              

                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            }
            );
            return services;
        }
    }
}
