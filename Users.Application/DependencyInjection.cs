using Application.Common.Domain;
using Application.Infrastructure.Common.Percistance;
using Application.User.Application;
using Application.User.Application.Comands.DeleteUser;
using Application.User.Application.Comands.UpdateUser;
using Application.User.Application.Comands.UpdateUserPassword;
using Application.User.Application.Query.GetById;
using Application.User.Application.Query.GetCount;
using Application.Users.Application.Comands.CreateUser;
using ApplicationUser.Application.Query.GetAll;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using Common.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
