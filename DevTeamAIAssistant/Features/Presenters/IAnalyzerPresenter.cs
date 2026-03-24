using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features.Presenters;

public interface IAnalyzerPresenter<T> where T : IAnalyzerResponse
{
    void Display(T response);
}
