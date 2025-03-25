using BankAPI.Application.Interfaces.Services;
using BankAPI.Application.Services;
using BankAPI.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankAPI.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAddBankAccountService, AddBankAccountService>();
            services.AddScoped<IGetAllBankAccountService, GetAllBankAccountService>();
            services.AddScoped<IDeactivateBankAccountService, DeactivateBankAccountService>();
            services.AddScoped<ITransferBankAccountService, TransferBankAccountService>(); 

            return services;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(typeof(AddBankAccountInputModelValidator));

            return services;
        }
    }
}
