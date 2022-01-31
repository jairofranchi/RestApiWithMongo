using Catalog.Repositories;
using Catalog.Settings;
using MongoDB.Driver;

namespace Catalog.Extensions
{
    public static class RepositoryExtensions
    {
        public static void ConfigureRepository(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<IMongoClient>(serviceprovider =>
            {
                var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                return new MongoClient(settings.ConnectionString);
            });
            services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
        }
    }
}