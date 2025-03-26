using BankAPI.Core.Interfaces;
using BankAPI.Core.Interfaces.Repositories;
using BankAPI.Infrastructure.Persistence;
using BankAPI.Infrastructure.Persistence.Context;
using BankAPI.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankAPI.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddBankContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BankContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    }
                )
            );

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddResilientOperation(this IServiceCollection services)
        {
            services.AddScoped<IResilientOperation, ResilientOperation>();
            
            return services;
        }
    }
}
