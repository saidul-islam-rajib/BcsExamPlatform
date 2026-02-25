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
        _model = configuration["Groq:Model"] ?? "llama-3.3-70b-versatile";
        
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
                    new { role = "system", content = "You are an expert BCS exam question generator. You MUST respond with valid JSON only. No markdown, no code blocks, no explanations outside the JSON." },
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

Return ONLY valid JSON in this exact format (no markdown, no code blocks):
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
            jsonContent = jsonContent.Trim();
            if (jsonContent.StartsWith("```json")) jsonContent = jsonContent.Substring(7);
            if (jsonContent.StartsWith("```")) jsonContent = jsonContent.Substring(3);
            if (jsonContent.EndsWith("```")) jsonContent = jsonContent.Substring(0, jsonContent.Length - 3);
            jsonContent = jsonContent.Trim();

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
