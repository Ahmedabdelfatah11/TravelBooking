
using Microsoft.AspNetCore.Mvc; 
using TravelBooking.Errors;

namespace TravelBooking.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {

            /// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            ///builder.Services.AddOpenApi();
            Services.Configure<ApiBehaviorOptions>(options =>
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
            return Services;
        }
    }
}
