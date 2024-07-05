namespace Catalog.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCatalogApi(
            this IServiceCollection services, ConfigurationManager config)
        {
            return services
                .AddMediator()
                .AddValidation()
                .AddCarter()
                .AddMartenDatabase(config)
                .AddHealthChecks(config)
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

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        }

        private static IServiceCollection AddMartenDatabase(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddMarten(cfg =>
            {
                cfg.Connection(config.GetConnectionString("Database")!);
            }).UseLightweightSessions();

            return services;
        }

        private static IServiceCollection AddHealthChecks(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddHealthChecks()
               .AddNpgSql(config.GetConnectionString("Database")!);

            return services;
        }

        private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            return services.AddExceptionHandler<CustomExceptionHandler>();
        }
    }
}