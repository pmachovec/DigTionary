using Dictionary.Shared.Database.Entities;
using Dictionary.Shared.InternalServices;
using Dictionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Dictionary.Shared.Components.Pages;

public class StartBase : ComponentBase
{
    [Inject]
    private ICategoryService CategoryService { get; set; } = default!;

    [Inject]
    protected IStringLocalizer<DictionaryTranslations> Localizer { get; set; } = default!;

    protected Category[] Categories { get; private set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Categories = [.. await CategoryService.GetCategoriesWithLessonsAsync(CancellationToken.None)];
    }
}
