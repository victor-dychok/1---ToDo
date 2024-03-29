using Application.Common.Domain;
using AutoMapper;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToDoDomain;

namespace ToDo.Application
{
    public static class DependencyInqection
    {
        public static IServiceCollection AddToDoServices(this IServiceCollection services)
        {
            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly()}, includeInternalTypes: true);

            services.AddAutoMapper(typeof(AutoMapperProfile));
            return services;
        }
    }
}
