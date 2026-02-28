using Microsoft.AspNetCore.Components;

namespace DigTionary.Shared.Presentation.Pages;

public class QuestionnaireBase : ComponentBase
{
    [Inject]
    private QuestionnaireParams QuestionnaireParams { get; set; } = default!;

    protected bool IsLoading { get; private set; } = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QuestionnaireParams.SetUpGeneratorTask;
            IsLoading = false;
            StateHasChanged();
        }
    }
}
