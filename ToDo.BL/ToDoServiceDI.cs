using Common.Domain;
using Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToDoBL.dto;
using ToDoBL.Mapper;
using ToDoBL.Validators;
using ToDoDomain;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ToDo.BL;
using UserServices;

namespace ToDoBL
{
    public static class ToDoServiceDI
    {
        public static IServiceCollection AddToDoServices(this IServiceCollection services)
        {

            services.AddScoped<IToDoService, ToDoService>();
            services.AddScoped<IRepository<TodoItem>, SqlServerBaseReository<TodoItem>>();
            services.AddScoped<IRepository<AppUser>, SqlServerBaseReository<AppUser>>();
            services.AddScoped<IRepository<AppUserAppRole>, SqlServerBaseReository<AppUserAppRole>>();
            services.AddTransient<IRepository<RefreshToken>, SqlServerBaseReository<RefreshToken>>();
            services.AddScoped<IRepository<AppUserRole>, SqlServerBaseReository<AppUserRole>>();
            services.AddScoped<IUserService, UserService>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly()}, includeInternalTypes: true);

            services.AddAutoMapper(typeof(Mapper.AutoMapperProfile));
            return services;
        }
    }
}
