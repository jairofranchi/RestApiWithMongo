using Catalog.Repositories;
using Catalog.Settings;
using MongoDB.Driver;

namespace Catalog.Extensions
{
    public static class RepositoryExtensions
    {
        public static void ConfigureRepository(this IServiceCollection services, ConfigurationManager configuration)
        {
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            services.AddSingleton<IMongoClient>(serviceprovider =>
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            });

            services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
            services.AddHealthChecks()
                .AddMongoDb(
                    mongoDbSettings.ConnectionString, 
                    name: "mongodb", 
                    timeout: TimeSpan.FromSeconds(3),
                    tags: new[] { "ready" });
        }
    }
}