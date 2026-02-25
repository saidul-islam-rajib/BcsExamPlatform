using BcsExamPlatform.Core.Enums;

namespace BcsExamPlatform.Core.Entities;

public class Exam
{
    public Guid ExamId { get; set; }
    public string ExamNameBangla { get; set; } = string.Empty;
    public string ExamNameEnglish { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }

    // Exam Configuration
    public int TotalQuestions { get; set; } = 200;
    public int DurationMinutes { get; set; } = 120;
    public decimal TotalMarks { get; set; } = 200.00m;

    // Language Settings
    public LanguageMode LanguageMode { get; set; } = LanguageMode.Bilingual;

    // Access Control
    public bool IsPaid { get; set; }
    public decimal? ExamFee { get; set; }
    public bool AllowGuestUsers { get; set; }

    // Status
    public bool IsPublished { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    // Navigation properties
    public ICollection<ExamSubjectDistribution> SubjectDistributions { get; set; } = new List<ExamSubjectDistribution>();
    public ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
}

public class ExamSubjectDistribution
{
    public Guid DistributionId { get; set; }
    public Guid ExamId { get; set; }
    public Guid SubjectId { get; set; }
    public int TotalQuestions { get; set; }
    public int EasyQuestions { get; set; }
    public int IntermediateQuestions { get; set; }
    public int HardQuestions { get; set; }

    public Exam Exam { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}

public class ExamQuestion
{
    public Guid ExamQuestionId { get; set; }
    public Guid ExamId { get; set; }
    public Guid QuestionId { get; set; }
    public int QuestionNumber { get; set; } // 1 to 200
    public int DisplayOrder { get; set; }

    public Exam Exam { get; set; } = null!;
    public Question Question { get; set; } = null!;
}
