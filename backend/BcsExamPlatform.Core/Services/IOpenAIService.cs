namespace BcsExamPlatform.Core.Services;

public interface IOpenAIService
{
    Task<List<GeneratedQuestion>> GenerateQuestionsAsync(
        string subject,
        string topic,
        string difficulty,
        int count,
        string language = "English"
    );
}

public class GeneratedQuestion
{
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int CorrectOptionIndex { get; set; }
    public string Explanation { get; set; } = string.Empty;
}
