using DigTionary.Shared.Components.Pages;
using DigTionary.Shared.Database;
using DigTionary.Shared.Generators;
using DigTionary.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigTionary.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSharedInternalDependencies(this IServiceCollection services, string dbPath) =>
        services
            .AddDbContext<DigTionaryDbContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={dbPath}"))
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<ICzechService, CzechService>()
            .AddScoped<ICzechGenerator, CzechGenerator>()
            .AddScoped<QuestionnaireParams>();
}
