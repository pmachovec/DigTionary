using DigTionary.Shared.Database.Entities;

// Database access is needed only inside Shared, therefore, repositories can be internal.
// They can be exposed publicly if needed in the future.
namespace DigTionary.Shared.Repositories;

internal interface ICategoryRepository
{
    Task<Category[]> GetCategoriesWithLessonsAsync(CancellationToken cancellationToken);
}
