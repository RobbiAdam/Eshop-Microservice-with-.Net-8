using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc;

namespace Basket.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBasketAPI(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddCarter()
                .AddMediator()
                .AddMartenDatabase(config)
                .AddRepository()
                .AddRedis(config)
                .AddExceptionHandling()
                .AddGrpc(config)
                .AddHealthChecks(config);
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

        private static IServiceCollection AddMartenDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddMarten(cfg =>
            {
                cfg.Connection(config.GetConnectionString("Database")!);
                cfg.Schema.For<ShoppingCart>().Identity(x => x.UserName);
            }).UseLightweightSessions();

            return services;
        }

        private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
        {
            return services.AddStackExchangeRedisCache(cfg =>
            {
                cfg.Configuration = config.GetConnectionString("Redis")!;
            });
        }

        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            return services
                .AddScoped<IBasketRepository, BasketRepository>()
                .Decorate<IBasketRepository, CachedBasketRepository>();
        }

        private static IServiceCollection AddGrpc(this IServiceCollection services, IConfiguration config)
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

        private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            return services.AddExceptionHandler<CustomExceptionHandler>();
        }

        private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            services.AddHealthChecks()
                .AddNpgSql(config.GetConnectionString("Database")!)
                .AddRedis(config.GetConnectionString("Redis")!);

            return services;

        }
    }
}
