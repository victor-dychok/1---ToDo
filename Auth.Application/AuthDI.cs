using Application.Common.Domain;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserServices;
using Auth.Application.Mapping;

namespace Auth.Api.Services
{
    public static class AuthDI
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
            object value = services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
