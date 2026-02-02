using Dictionary.Shared.Components.Pages;
using Dictionary.Shared.Database;
using Dictionary.Shared.Generators;
using Dictionary.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSharedInternalDependencies(this IServiceCollection services, string dbPath) =>
        services
            .AddDbContext<DictionaryDbContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={dbPath}"))
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<IWordService, WordService>()
            .AddScoped<IWordGenerator, WordGenerator>()
            .AddScoped<QuestionnaireParams>();
}
