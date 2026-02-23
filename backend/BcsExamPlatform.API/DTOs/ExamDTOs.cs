namespace BcsExamPlatform.API.DTOs;

public class ExamListDTO
{
    public Guid ExamId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public decimal TotalMarks { get; set; }
    public string LanguageMode { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public decimal? ExamFee { get; set; }
    public bool AllowGuestUsers { get; set; }
    public bool IsPublished { get; set; }
}

public class ExamDetailDTO
{
    public Guid ExamId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public decimal TotalMarks { get; set; }
    public string LanguageMode { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public decimal? ExamFee { get; set; }
    public bool AllowGuestUsers { get; set; }
    public List<SubjectDistributionDTO> SubjectDistributions { get; set; } = new();
}

public class SubjectDistributionDTO
{
    public string SubjectName { get; set; } = string.Empty;
    public int TotalQuestions { get; set; }
    public int EasyQuestions { get; set; }
    public int IntermediateQuestions { get; set; }
    public int HardQuestions { get; set; }
}

public class StartExamRequest
{
    public Guid ExamId { get; set; }
    public string SelectedLanguage { get; set; } = "Bangla";
    public string? GuestEmail { get; set; }
}

public class StartExamResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? AttemptId { get; set; }
    public DateTime? StartedAt { get; set; }
}

public class ExamQuestionDTO
{
    public Guid QuestionId { get; set; }
    public int QuestionNumber { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionImageUrl { get; set; }
    public bool HasMathContent { get; set; }
    public List<QuestionOptionDTO> Options { get; set; } = new();
    public string SubjectName { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public string DifficultyLevel { get; set; } = string.Empty;
    public decimal Marks { get; set; }
}

public class QuestionOptionDTO
{
    public Guid OptionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public int OptionOrder { get; set; }
}

public class SubmitAnswerRequest
{
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public int TimeSpentSeconds { get; set; }
    public bool IsMarkedForReview { get; set; }
}

public class SubmitExamRequest
{
    public Guid AttemptId { get; set; }
}

public class ExamResultDTO
{
    public Guid AttemptId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public decimal TotalMarksObtained { get; set; }
    public decimal TotalMarks { get; set; }
    public int TotalCorrectAnswers { get; set; }
    public int TotalWrongAnswers { get; set; }
    public int TotalUnanswered { get; set; }
    public decimal AccuracyPercentage { get; set; }
    public int TotalTimeSpentSeconds { get; set; }
    public int Rank { get; set; }
    public string BadgeName { get; set; } = string.Empty;
    public string BadgeColor { get; set; } = string.Empty;
    public bool IsPassed { get; set; }
}

public class QuestionReviewDTO
{
    public Guid QuestionId { get; set; }
    public int QuestionNumber { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionImageUrl { get; set; }
    public List<QuestionOptionReviewDTO> Options { get; set; } = new();
    public Guid? UserSelectedOptionId { get; set; }
    public Guid CorrectOptionId { get; set; }
    public bool IsCorrect { get; set; }
    public decimal MarksObtained { get; set; }
    public int TimeSpentSeconds { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public string DifficultyLevel { get; set; } = string.Empty;
    public string? SourceReference { get; set; }
}

public class QuestionOptionReviewDTO
{
    public Guid OptionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public int OptionOrder { get; set; }
    public bool IsCorrect { get; set; }
}
