using Common.Api.Service;
using Common.Domain;
using Common.Repository;
using FluentValidation;
using System.Reflection;
using UserServices;

namespace Auth.Api.Services
{
    public static class AuthDI
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAutnService, AuthService>();
            services.AddTransient<IRepository<AppUserRole>, SqlServerBaseReository<AppUserRole>>();
            services.AddTransient<IRepository<RefreshToken>, SqlServerBaseReository<RefreshToken>>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IRepository<AppUserAppRole>, SqlServerBaseReository<AppUserAppRole>>();
            services.AddTransient<IRepository<AppUser>, SqlServerBaseReository<AppUser>>();
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
            object value = services.AddAutoMapper(typeof(AutoMapperProfile));
            return services;
        }
    }
}
