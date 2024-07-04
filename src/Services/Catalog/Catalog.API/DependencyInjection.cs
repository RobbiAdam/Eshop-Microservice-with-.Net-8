namespace Catalog.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCatalogApi(
            this IServiceCollection services, ConfigurationManager config)
        {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            services.AddCarter();

            services.AddMarten(cfg =>
            {
                cfg.Connection(config.GetConnectionString("Database")!);
            }).UseLightweightSessions();

            services.AddExceptionHandler<CustomExceptionHandler>();

            return services;
        }
    }
}
