using DigTionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DigTionary.Shared.Presentation.Pages.QuestionnaireComponents;

public class LoadingBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<DigTionaryTranslations> Localizer { get; set; } = default!;
}
