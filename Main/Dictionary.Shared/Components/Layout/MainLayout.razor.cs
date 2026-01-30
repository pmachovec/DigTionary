using Dictionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Dictionary.Shared.Components.Layout;

public class MainLayoutBase : LayoutComponentBase
{
    [Inject]
    protected IStringLocalizer<DictionaryTranslations> Localizer { get; set; } = default!;
}
