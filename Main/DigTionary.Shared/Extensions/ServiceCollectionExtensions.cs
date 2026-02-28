using DigTionary.Shared.Application;
using DigTionary.Shared.Database;
using DigTionary.Shared.Presentation.Pages;
using DigTionary.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigTionary.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSharedInternalDependencies(this IServiceCollection services, string dbPath) =>
        services
            .AddDbContext<DigTionaryDbContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={dbPath}"))
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<ICzechRepository, CzechRepository>()
            .AddScoped<ICzechGenerator, CzechGenerator>()
            .AddScoped<QuestionnaireParams>();
}
