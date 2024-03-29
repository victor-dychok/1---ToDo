using Application.Common.Domain;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDomain;

namespace Application.Infrastructure.Common.Percistance
{
    public static class DbContextDi
    {
        public static IServiceCollection AddTodoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, AppDBContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }
            );

            services.AddTransient<IRepository<AppUserRole>, SqlServerBaseReository<AppUserRole>>();
            services.AddTransient<IRepository<RefreshToken>, SqlServerBaseReository<RefreshToken>>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IRepository<AppUserAppRole>, SqlServerBaseReository<AppUserAppRole>>();
            services.AddTransient<IRepository<AppUser>, SqlServerBaseReository<AppUser>>();
            services.AddTransient<IRepository<TodoItem>, SqlServerBaseReository<TodoItem>>();
            return services;
        }
    }
}
