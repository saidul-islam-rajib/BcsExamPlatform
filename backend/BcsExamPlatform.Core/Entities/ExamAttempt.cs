namespace BcsExamPlatform.Core.Entities;

public class ExamAttempt
{
    public Guid AttemptId { get; set; }
    public Guid ExamId { get; set; }
    public Guid? UserId { get; set; }
    public string? GuestEmail { get; set; }

    // Attempt Details
    public string SelectedLanguage { get; set; } = "Bangla";
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int? TotalTimeSpentSeconds { get; set; }

    // Results
    public decimal? TotalMarksObtained { get; set; }
    public int? TotalCorrectAnswers { get; set; }
    public int? TotalWrongAnswers { get; set; }
    public int? TotalUnanswered { get; set; }
    public decimal? AccuracyPercentage { get; set; }

    // Status
    public bool IsCompleted { get; set; }
    public int? Rank { get; set; }

    // Anti-cheating
    public int TabSwitchCount { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Exam Exam { get; set; } = null!;
    public User? User { get; set; }
    public ICollection<ExamAnswer> ExamAnswers { get; set; } = new List<ExamAnswer>();
}

public class ExamAnswer
{
    public Guid AnswerId { get; set; }
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public bool? IsCorrect { get; set; }
    public decimal? MarksObtained { get; set; }
    public int TimeSpentSeconds { get; set; }
    public bool IsMarkedForReview { get; set; }
    public DateTime? AnsweredAt { get; set; }

    public ExamAttempt ExamAttempt { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public QuestionOption? SelectedOption { get; set; }
}
