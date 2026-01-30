using Dictionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Dictionary.Shared.Components.Pages;

public class NotFoundBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<DictionaryTranslations> Localizer { get; set; } = default!;
}
