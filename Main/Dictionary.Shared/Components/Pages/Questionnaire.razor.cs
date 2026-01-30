using Dictionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Dictionary.Shared.Components.Pages;

public class QuestionnaireBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<DictionaryTranslations> Localizer { get; set; } = default!;

    [Inject]
    private QuestionnaireParams QuestionnaireParams { get; set; } = default!;

    protected bool IsLoading { get; private set; } = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionnaireParams.SetUpWordsTask;
            IsLoading = false;
            //SetUpNewCzechs();
            StateHasChanged();
        }
    }
}
