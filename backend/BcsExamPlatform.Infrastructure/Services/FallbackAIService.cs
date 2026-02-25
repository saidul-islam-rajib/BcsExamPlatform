using BcsExamPlatform.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BcsExamPlatform.Infrastructure.Services;

public class FallbackAIService : IOpenAIService
{
    private readonly List<(string Name, IOpenAIService Service)> _services;
    private readonly ILogger<FallbackAIService>? _logger;

    public FallbackAIService(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<FallbackAIService>? logger = null)
    {
        _logger = logger;
        _services = new List<(string, IOpenAIService)>();

        // Initialize all available services
        try
        {
            var geminiKey = configuration["Gemini:ApiKey"];
            if (!string.IsNullOrEmpty(geminiKey) && geminiKey != "YOUR_GEMINI_API_KEY_HERE")
            {
                _services.Add(("Gemini", new GeminiService(configuration, httpClientFactory.CreateClient())));
                _logger?.LogInformation("Gemini service initialized");
            }
        }
        catch { }

        try
        {
            var groqKey = configuration["Groq:ApiKey"];
            if (!string.IsNullOrEmpty(groqKey) && groqKey != "YOUR_GROQ_API_KEY_HERE")
            {
                _services.Add(("Groq", new GroqService(configuration, httpClientFactory.CreateClient())));
                _logger?.LogInformation("Groq service initialized");
            }
        }
        catch { }

        try
        {
            var hfKey = configuration["HuggingFace:ApiKey"];
            if (!string.IsNullOrEmpty(hfKey) && hfKey != "YOUR_HUGGINGFACE_API_KEY_HERE")
            {
                _services.Add(("HuggingFace", new HuggingFaceService(configuration, httpClientFactory.CreateClient())));
                _logger?.LogInformation("HuggingFace service initialized");
            }
        }
        catch { }

        try
        {
            var openaiKey = configuration["OpenAI:ApiKey"];
            if (!string.IsNullOrEmpty(openaiKey) && openaiKey != "YOUR_OPENAI_API_KEY_HERE")
            {
                _services.Add(("OpenAI", new OpenAIService(configuration, httpClientFactory.CreateClient())));
                _logger?.LogInformation("OpenAI service initialized");
            }
        }
        catch { }

        if (_services.Count == 0)
        {
            throw new Exception("No AI services configured. Please add at least one API key.");
        }

        _logger?.LogInformation($"FallbackAIService initialized with {_services.Count} provider(s): {string.Join(", ", _services.Select(s => s.Name))}");
    }

    public async Task<List<GeneratedQuestion>> GenerateQuestionsAsync(
        string subject,
        string topic,
        string difficulty,
        int count,
        string language = "English")
    {
        var errors = new List<string>();

        foreach (var (name, service) in _services)
        {
            try
            {
                _logger?.LogInformation($"Trying {name} API...");
                
                var questions = await service.GenerateQuestionsAsync(subject, topic, difficulty, count, language);
                
                if (questions != null && questions.Count > 0)
                {
                    _logger?.LogInformation($"✅ {name} API succeeded! Generated {questions.Count} questions.");
                    return questions;
                }
                
                errors.Add($"{name}: Returned empty result");
                _logger?.LogWarning($"❌ {name} API returned empty result, trying next provider...");
            }
            catch (Exception ex)
            {
                errors.Add($"{name}: {ex.Message}");
                _logger?.LogWarning($"❌ {name} API failed: {ex.Message}, trying next provider...");
            }
        }

        // All services failed
        var errorMessage = $"All AI providers failed:\n" + string.Join("\n", errors.Select((e, i) => $"{i + 1}. {e}"));
        _logger?.LogError(errorMessage);
        throw new Exception(errorMessage);
    }
}
