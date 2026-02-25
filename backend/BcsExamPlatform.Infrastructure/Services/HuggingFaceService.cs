using System.Text;
using System.Text.Json;
using BcsExamPlatform.Core.Services;
using Microsoft.Extensions.Configuration;

namespace BcsExamPlatform.Infrastructure.Services;

public class HuggingFaceService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public HuggingFaceService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = configuration["HuggingFace:ApiKey"] ?? throw new Exception("HuggingFace API Key not configured");
        _model = configuration["HuggingFace:Model"] ?? "mistralai/Mixtral-8x7B-Instruct-v0.1";
        
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
                inputs = prompt,
                parameters = new
                {
                    max_new_tokens = 2000,
                    temperature = 0.7,
                    return_full_text = false
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var url = $"https://router.huggingface.co/models/{_model}";
            var response = await _httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"HuggingFace API Error ({response.StatusCode}): {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<HuggingFaceResponse>>(responseBody);

            var jsonContent = result?[0]?.GeneratedText ?? "{}";
            var questions = ParseQuestions(jsonContent);
            
            if (questions.Count == 0)
            {
                throw new Exception("HuggingFace returned no questions. Response may be in wrong format.");
            }

            return questions;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Network error calling HuggingFace API: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex.Message.Contains("API Key"))
        {
            throw new Exception("Invalid HuggingFace API Key", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"HuggingFace error: {ex.Message}", ex);
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

    private class HuggingFaceResponse
    {
        public string? GeneratedText { get; set; }
    }
}
