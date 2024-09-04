using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Basket.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddCarter()
                .AddMediator()
                .AddMartenDatabase(config)
                .AddRepository()
                .AddRedis(config)
                .AddExceptionHandling()
                .AddGrpcServices(config)
                .AddHealthChecks(config)
                .AddMessageBroker(config);
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            app.UseExceptionHandler(_ => { });

            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            return app;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

        private static IServiceCollection AddMartenDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddMarten(cfg =>
            {
                cfg.Connection(config.GetConnectionString("Database")!);
                cfg.Schema.For<ShoppingCart>().Identity(x => x.UserName);
            }).UseLightweightSessions();

            return services;
        }

        private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config) =>
            services.AddStackExchangeRedisCache(cfg =>
            {
                cfg.Configuration = config.GetConnectionString("Redis")!;
            });

        private static IServiceCollection AddRepository(this IServiceCollection services) =>
            services
                .AddScoped<IBasketRepository, BasketRepository>()
                .Decorate<IBasketRepository, CachedBasketRepository>();
        {
        private static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(cfg =>
            {
                cfg.Address = new Uri(config.GetValue<string>("GrpcSettings:DiscountUrl")!);
            })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback
                        = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                    return handler;
                });
            return services;
        }

        private static IServiceCollection AddExceptionHandling(this IServiceCollection services) =>
            services.AddExceptionHandler<CustomExceptionHandler>();

        private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            services.AddHealthChecks()
                .AddNpgSql(config.GetConnectionString("Database")!)
                .AddRedis(config.GetConnectionString("Redis")!);

            return services;
        }
    }
}
