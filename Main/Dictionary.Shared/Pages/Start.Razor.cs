using Dictionary.Shared.Database.Entities;
using Dictionary.Shared.InternalServices;
using Microsoft.AspNetCore.Components;

namespace Dictionary.Shared.Pages;

public class StartBase : ComponentBase
{
    [Inject]
    private ICategoryService CategoryService { get; set; } = default!;

    protected Category[] Categories { get; private set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Categories = [.. await CategoryService.GetCategoriesWithLessonsAsync(CancellationToken.None)];
    }
}
