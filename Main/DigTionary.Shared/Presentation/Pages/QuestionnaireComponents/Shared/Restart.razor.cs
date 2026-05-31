using DigTionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DigTionary.Shared.Presentation.Pages.QuestionnaireComponents.Shared;

public class RestartBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<DigTionaryTranslations> Localizer { get; set; } = default!;
}
