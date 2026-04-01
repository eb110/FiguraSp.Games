using FiguraSp.Games.Model.Data;
using FiguraSp.Games.Service.Services;
using FiguraSp.SharedLibrary.DependencyInjection;

namespace FiguraSp.Teams.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSharedDbConnection(this IServiceCollection services, IConfiguration config)
        {
            //add db
            SharedService.AddSharedSqlService<GamesDbContext>(services, config);

            return services;
        }

        public static IServiceCollection AddSharedJwtScheme(this IServiceCollection services, IConfiguration config)
        {
            //add db
            SharedService.AddJwtSharedService(services, config);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();

            return services;
        }

        public static IApplicationBuilder UserSharedGatewayMiddleware(this IApplicationBuilder app)
        {
            //register middleware such as:
            //global exception -> handle external errors
            //listen to api gateway only -> block all outside calls
            SharedService.UseSharedGatewary(app);

            return app;
        }
    }
}
