using Dictionary.Shared.Database.Entities;
using Dictionary.Shared.Generators;
using Dictionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Dictionary.Shared.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    private Czech? _actualCzech;

    [Inject]
    protected IStringLocalizer<DictionaryTranslations> Localizer { get; set; } = default!;

    [Inject]
    private ICzechGenerator CzechGenerator { get; set; } = default!;

    [Inject]
    private QuestionnaireParams QuestionnaireParams { get; set; } = default!;

    protected bool IsLoading { get; private set; } = true;

    protected bool IsResultHidden { get; private set; } = true;

    protected string Text { get; private set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionnaireParams.SetUpCzechsTask;
            IsLoading = false;
            SetUpNewCzech();
            StateHasChanged();
        }
    }

    protected void ShowResult() => IsResultHidden = false;

    protected void HideResult() => IsResultHidden = true;

    private void SetUpNewCzech()
    {
        _actualCzech = CzechGenerator.GetNextCzech();
        SetUpCzech(_actualCzech);
    }

    private void SetUpCzech(Czech czech) => Text = czech.Text;
}
