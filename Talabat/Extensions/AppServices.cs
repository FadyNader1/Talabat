using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Interfaces;
using Talabat.Errors;
using Talabat.Helper;
using Talabat.Repository.Context;
using Talabat.Repository.Repositories;
using Talabat.Services.Services;

namespace Talabat.Extensions
{
    public static class AppServices
    {
        public static IServiceCollection ApplyServices(this IServiceCollection Services)
        {
            Services.AddControllers();
            //auto mapper
            Services.AddAutoMapper(typeof(MappingProfiles));
            // DI UnitOfWork
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            //change behavior api
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(x => x.Value.Errors.Count() > 0).SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray();

                    var validationerror = new ApiValidationError()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationerror);
                };
            });
            //DI BasketRepository
            Services.AddScoped<IBasketRepository, BasketRepository>();
            //identity
            Services.AddIdentity<UserApp, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<TalabatContextIdentity>()
            .AddDefaultTokenProviders();
            //DI TokenService
            Services.AddScoped<ITokenService, TokenService>();
            //DI Email
            Services.AddScoped<IEmailService,EmailService>();
            //DI OrderService
            Services.AddScoped<IOrderService, OrderService>();
            //DI  payment intent service        
            Services.AddScoped<IPaymentIntentService, PaymentIntentService>();

            return Services;
        }
    }
}
