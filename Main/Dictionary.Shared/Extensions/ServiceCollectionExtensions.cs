using Dictionary.Shared.Database;
using Dictionary.Shared.InternalServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDictionaryDb(this IServiceCollection services, string dbPath) =>
        services
            .AddDbContext<DictionaryDbContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={dbPath}"))
            .AddScoped<ICategoryService, CategoryService>();
}
