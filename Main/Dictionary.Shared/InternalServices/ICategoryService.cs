using Dictionary.Shared.Database.Entities;

// Database access is needed only inside Shared, therefore, services can be internal.
// They can be exposed publicly if needed in the future.
namespace Dictionary.Shared.InternalServices;

internal interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesWithLessonsAsync(CancellationToken cancellationToken);
}
