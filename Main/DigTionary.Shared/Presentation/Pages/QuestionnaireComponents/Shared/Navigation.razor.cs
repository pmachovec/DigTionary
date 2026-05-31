using DigTionary.Shared.Presentation.Constants;
using DigTionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DigTionary.Shared.Presentation.Pages.QuestionnaireComponents.Shared;

public class NavigationBase : ComponentBase
{
    protected const string NAVIGATION_BUTTON_CLASSES = "border border-dark btn btn-lg btn-primary col-auto digtionary-button mb-2 me-2";

    [Inject]
    protected IStringLocalizer<DigTionaryTranslations> Localizer { get; set; } = default!;

    [Parameter, EditorRequired]
    public Action ClickPreviousWordButton { get; init; }

    [Parameter, EditorRequired]
    public Action ClickNextWordButton { get; init; }

    protected string Done { get; private set; } = string.Empty;

    public string NextWordButtonDisabled { protected get; set; } = CssClasses.DISABLED;

    public string PreviousWordButtonDisabled { protected get; set; } = CssClasses.DISABLED;

    public void SetDone() => Done = Localizer[DigTionaryTranslations.Done];
}
