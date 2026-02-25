using System.Text;
using System.Text.Json;
using BcsExamPlatform.Core.Services;
using Microsoft.Extensions.Configuration;

namespace BcsExamPlatform.Infrastructure.Services;

public class GroqService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public GroqService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Groq:ApiKey"] ?? throw new Exception("Groq API Key not configured");
        _model = configuration["Groq:Model"] ?? "llama-3.1-8b-instant";
        
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<List<GeneratedQuestion>> GenerateQuestionsAsync(
        string subject,
        string topic,
        string difficulty,
        int count,
        string language = "English")
    {
        try
        {
            var prompt = BuildPrompt(subject, topic, difficulty, count, language);
            
            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                max_tokens = 2000
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Groq API Error ({response.StatusCode}): {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GroqResponse>(responseBody);

            var jsonContent = result?.Choices?[0]?.Message?.Content ?? "{}";
            
            // Log the raw response for debugging
            Console.WriteLine($"[Groq Debug] Raw response length: {jsonContent.Length}");
            Console.WriteLine($"[Groq Debug] First 200 chars: {jsonContent.Substring(0, Math.Min(200, jsonContent.Length))}");
            
            var questions = ParseQuestions(jsonContent);
            
            if (questions.Count == 0)
            {
                throw new Exception($"Groq returned no questions. Raw response: {jsonContent.Substring(0, Math.Min(500, jsonContent.Length))}");
            }

            return questions;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Network error calling Groq API: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex.Message.Contains("API Key"))
        {
            throw new Exception("Invalid Groq API Key", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Groq error: {ex.Message}", ex);
        }
    }

    private string BuildPrompt(string subject, string topic, string difficulty, int count, string language)
    {
        return $@"Generate exactly {count} multiple-choice questions for BCS Preliminary Exam.

Subject: {subject}
Topic: {topic}  
Difficulty: {difficulty}
Language: {language}

IMPORTANT: Return ONLY a JSON object in this EXACT format with NO additional text, NO markdown, NO explanations:

{{
  ""questions"": [
    {{
      ""questionText"": ""Your question here?"",
      ""options"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
      ""correctOptionIndex"": 1,
      ""explanation"": ""Brief explanation""
    }}
  ]
}}

Generate {count} questions now:";
    }

    private List<GeneratedQuestion> ParseQuestions(string jsonContent)
    {
        try
        {
            // Clean up the response - remove markdown, explanatory text, etc.
            jsonContent = jsonContent.Trim();
            
            // Remove everything before the first {
            int firstBrace = jsonContent.IndexOf('{');
            if (firstBrace > 0)
            {
                jsonContent = jsonContent.Substring(firstBrace);
            }
            
            // Remove everything after the last }
            int lastBrace = jsonContent.LastIndexOf('}');
            if (lastBrace >= 0 && lastBrace < jsonContent.Length - 1)
            {
                jsonContent = jsonContent.Substring(0, lastBrace + 1);
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var wrapper = JsonSerializer.Deserialize<QuestionWrapper>(jsonContent, options);
            return wrapper?.Questions ?? new List<GeneratedQuestion>();
        }
        catch
        {
            return new List<GeneratedQuestion>();
        }
    }

    private class QuestionWrapper
    {
        public List<GeneratedQuestion> Questions { get; set; } = new();
    }

    private class GroqResponse
    {
        public List<Choice>? Choices { get; set; }
    }

    private class Choice
    {
        public Message? Message { get; set; }
    }

    private class Message
    {
        public string? Content { get; set; }
    }
}
