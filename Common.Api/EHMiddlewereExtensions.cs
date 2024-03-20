using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api
{
    public static class EHMiddlewereExtensions
    {
        public static IApplicationBuilder UseExeptionHendler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExeptionHendlerMiddleware>();
            return app;
        }
    }
}
