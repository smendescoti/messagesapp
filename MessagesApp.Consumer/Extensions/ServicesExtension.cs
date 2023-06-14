using MessagesApp.Consumer.Logs.Collections;
using MessagesApp.Consumer.Logs.Contexts;
using MessagesApp.Consumer.Logs.Persistence;
using MessagesApp.Consumer.Services;
using MessagesApp.Consumer.Settings;
using Microsoft.Extensions.Configuration;

namespace MessagesApp.Consumer.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailMessageApiSettings>(configuration.GetSection("EmailMessageApiSettings"));
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDBSettings"));
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));

            services.AddTransient<AuthService>();
            services.AddTransient<MessagesService>();

            services.AddTransient<LogMensagensPersistence>();
            services.AddTransient<MongoDBContext>();

            return services;
        }
    }
}
