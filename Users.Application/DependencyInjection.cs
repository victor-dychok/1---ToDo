using Application.Common.Domain;
using Application.User.Application;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using User.Application;

namespace Application.Users.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
           
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddSingleton<UserMemoryCache>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);



            return services;
        }
    }
}
