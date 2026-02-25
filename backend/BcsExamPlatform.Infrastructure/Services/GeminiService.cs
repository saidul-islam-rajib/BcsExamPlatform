using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BcsExamPlatform.Core.Services;
using Microsoft.Extensions.Configuration;

namespace BcsExamPlatform.Infrastructure.Services;

public class GeminiService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public GeminiService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"] ?? throw new Exception("Gemini API Key not configured");
        _model = configuration["Gemini:Model"] ?? "gemini-1.5-flash-latest";
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
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 2000
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var url = $"https://generativelanguage.googleapis.com/v1/models/{_model}:generateContent?key={_apiKey}";
            var response = await _httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error ({response.StatusCode}): {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GeminiResponse>(responseBody);

            var jsonContent = result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "{}";
            var questions = ParseQuestions(jsonContent);
            
            if (questions.Count == 0)
            {
                throw new Exception($"Gemini returned no questions. Raw response: {jsonContent.Substring(0, Math.Min(500, jsonContent.Length))}");
            }

            return questions;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Network error calling Gemini API: {ex.Message}. Check your internet connection.", ex);
        }
        catch (Exception ex) when (ex.Message.Contains("API Key"))
        {
            throw new Exception("Invalid Gemini API Key. Please check your configuration.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating questions: {ex.Message}", ex);
        }
    }

    private string BuildPrompt(string subject, string topic, string difficulty, int count, string language)
    {
        return $@"Generate {count} {difficulty} level multiple-choice questions for BCS Preliminary Exam.

Subject: {subject}
Topic: {topic}
Language: {language}
Difficulty: {difficulty}

Requirements:
1. Each question must have exactly 4 options (A, B, C, D)
2. Only one correct answer per question
3. Include a brief explanation for the correct answer
4. Questions should be relevant to BCS Preliminary Exam syllabus
5. Use clear, professional language

Return ONLY a valid JSON response in this exact format (no markdown, no code blocks):
{{
  ""questions"": [
    {{
      ""questionText"": ""Question text here"",
      ""options"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
      ""correctOptionIndex"": 0,
      ""explanation"": ""Explanation here""
    }}
  ]
}}";
    }

    private List<GeneratedQuestion> ParseQuestions(string jsonContent)
    {
        try
        {
            // Remove markdown code blocks if present
            jsonContent = jsonContent.Trim();
            if (jsonContent.StartsWith("```json"))
            {
                jsonContent = jsonContent.Substring(7);
            }
            if (jsonContent.StartsWith("```"))
            {
                jsonContent = jsonContent.Substring(3);
            }
            if (jsonContent.EndsWith("```"))
            {
                jsonContent = jsonContent.Substring(0, jsonContent.Length - 3);
            }
            jsonContent = jsonContent.Trim();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
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

    private class GeminiResponse
    {
        public List<Candidate>? Candidates { get; set; }
    }

    private class Candidate
    {
        public Content? Content { get; set; }
    }

    private class Content
    {
        public List<Part>? Parts { get; set; }
    }

    private class Part
    {
        public string? Text { get; set; }
    }
}
