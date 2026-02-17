using Dictionary.Shared.Database.Entities;

// Database access is needed only inside Shared, therefore, services can be internal.
// They can be exposed publicly if needed in the future.
namespace Dictionary.Shared.Services;

internal interface ICategoryService
{
    Task<Category[]> GetCategoriesWithLessonsAsync(CancellationToken cancellationToken);
}
