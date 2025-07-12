using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

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
