using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BcsExamPlatform.Core.Services;
using Microsoft.Extensions.Configuration;

namespace BcsExamPlatform.Infrastructure.Services;

public class OpenAIService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public OpenAIService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"] ?? throw new Exception("OpenAI API Key not configured");
        _model = configuration["OpenAI:Model"] ?? "gpt-4o-mini";
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
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
                    new { role = "system", content = "You are an expert BCS exam question generator. Generate high-quality multiple-choice questions with 4 options each." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                response_format = new { type = "json_object" }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API Error ({response.StatusCode}): {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OpenAIResponse>(responseBody);

            var questions = ParseQuestions(result?.Choices?[0]?.Message?.Content ?? "{}");
            
            if (questions.Count == 0)
            {
                throw new Exception("OpenAI returned no questions. Response may be in wrong format.");
            }

            return questions;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Network error calling OpenAI API: {ex.Message}. Check your internet connection.", ex);
        }
        catch (Exception ex) when (ex.Message.Contains("API Key"))
        {
            throw new Exception("Invalid OpenAI API Key. Please check your configuration.", ex);
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

Return the response in this exact JSON format:
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

    private class OpenAIResponse
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
