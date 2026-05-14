namespace DigTionary.Shared.Presentation.Pages;

internal sealed class QuestionnaireParams
{
    public bool CzToLang { get; set; } = true;

    public Task SetUpGeneratorTask { get; set; } = Task.CompletedTask;
}
