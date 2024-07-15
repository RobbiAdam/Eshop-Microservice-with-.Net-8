using BuildingBlocks.Exceptions.Handler;

namespace Basket.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBasketAPI(this IServiceCollection services, ConfigurationManager config)
        {
            return services
                .AddCarter()
                .AddMediator()
                .AddMartenDatabase(config)
                .AddRepository()
                .AddExceptionHandling();
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            return services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
        }

        private static IServiceCollection AddMartenDatabase(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddMarten(cfg =>
            {
                cfg.Connection(config.GetConnectionString("Database")!);
                cfg.Schema.For<ShoppingCart>().Identity(x => x.UserName);
            }).UseLightweightSessions();

            return services;
        }
        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            return services
                .AddScoped<IBasketRepository, BasketRepository>();
        }
        private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            return services.AddExceptionHandler<CustomExceptionHandler>();
        }
    }
}
