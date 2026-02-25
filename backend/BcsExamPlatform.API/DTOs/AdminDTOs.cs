using BcsExamPlatform.Core.Enums;

namespace BcsExamPlatform.API.DTOs;

// Question DTOs
public class CreateQuestionDTO
{
    public Guid SubjectId { get; set; }
    public Guid TopicId { get; set; }
    public string QuestionTextBangla { get; set; } = string.Empty;
    public string QuestionTextEnglish { get; set; } = string.Empty;
    public string? QuestionImageUrl { get; set; }
    public bool HasMathContent { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Easy;
    public decimal Marks { get; set; } = 1.00m;
    public SourceType SourceType { get; set; } = SourceType.Manual;
    public int? SourceYear { get; set; }
    public string? SourceReference { get; set; }
    public bool IsAIGenerated { get; set; }
    public bool IsUnique { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Created;
}

public class UpdateQuestionDTO
{
    public Guid SubjectId { get; set; }
    public Guid TopicId { get; set; }
    public string QuestionTextBangla { get; set; } = string.Empty;
    public string QuestionTextEnglish { get; set; } = string.Empty;
    public string? QuestionImageUrl { get; set; }
    public bool HasMathContent { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Easy;
    public decimal Marks { get; set; } = 1.00m;
    public SourceType SourceType { get; set; } = SourceType.Manual;
    public int? SourceYear { get; set; }
    public string? SourceReference { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Created;
    public bool IsActive { get; set; } = true;
}

public class CreateQuestionOptionDTO
{
    public string OptionTextBangla { get; set; } = string.Empty;
    public string OptionTextEnglish { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

public class CreateExplanationDTO
{
    public string ExplanationBangla { get; set; } = string.Empty;
    public string ExplanationEnglish { get; set; } = string.Empty;
}

public class QuestionResponseDTO
{
    public Guid QuestionId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class QuestionListDTO
{
    public Guid QuestionId { get; set; }
    public string QuestionTextEnglish { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public int OptionsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class QuestionDetailDTO
{
    public Guid QuestionId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TopicId { get; set; }
    public string QuestionTextBangla { get; set; } = string.Empty;
    public string QuestionTextEnglish { get; set; } = string.Empty;
    public string? QuestionImageUrl { get; set; }
    public bool HasMathContent { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public decimal Marks { get; set; }
    public SourceType SourceType { get; set; }
    public int? SourceYear { get; set; }
    public string? SourceReference { get; set; }
    public bool IsAIGenerated { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public bool IsActive { get; set; }
    public List<QuestionOptionDetailDTO> Options { get; set; } = new();
    public string ExplanationBangla { get; set; } = string.Empty;
    public string ExplanationEnglish { get; set; } = string.Empty;
}

public class QuestionOptionDetailDTO
{
    public Guid OptionId { get; set; }
    public string OptionTextBangla { get; set; } = string.Empty;
    public string OptionTextEnglish { get; set; } = string.Empty;
    public int OptionOrder { get; set; }
    public bool IsCorrect { get; set; }
}

// Exam DTOs
public class CreateExamDTO
{
    public string ExamNameBangla { get; set; } = string.Empty;
    public string ExamNameEnglish { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int TotalQuestions { get; set; } = 200;
    public int DurationMinutes { get; set; } = 120;
    public decimal TotalMarks { get; set; } = 200;
    public LanguageMode LanguageMode { get; set; } = LanguageMode.Bilingual;
    public bool IsPaid { get; set; }
    public decimal ExamFee { get; set; }
    public bool AllowGuestUsers { get; set; } = true;
}

public class CreateSubjectDistributionDTO
{
    public Guid SubjectId { get; set; }
    public int TotalQuestions { get; set; }
    public int EasyQuestions { get; set; }
    public int IntermediateQuestions { get; set; }
    public int HardQuestions { get; set; }
}

public class ExamResponseDTO
{
    public Guid ExamId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ExamListAdminDTO
{
    public Guid ExamId { get; set; }
    public string ExamNameEnglish { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsPublished { get; set; }
    public bool IsPaid { get; set; }
    public decimal ExamFee { get; set; }
    public DateTime CreatedAt { get; set; }
}

// AI Question Generation DTO
public class GenerateAIQuestionsDTO
{
    public Guid SubjectId { get; set; }
    public Guid TopicId { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Easy;
    public int Count { get; set; } = 10;
    public LanguageMode Language { get; set; } = LanguageMode.English;
    public bool AutoApprove { get; set; } = true;
}

// Manual Question Management for Exams
public class AddManualQuestionDTO
{
    public Guid ExamId { get; set; }
    public Guid SubjectId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int CorrectOptionIndex { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Easy;
}

public class UpdateManualQuestionDTO
{
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int CorrectOptionIndex { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Easy;
}

public class ExamQuestionListDTO
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; }
    public int OptionsCount { get; set; }
    public int QuestionNumber { get; set; }
}
