using BcsExamPlatform.Core.Enums;

namespace BcsExamPlatform.Core.Entities;

public class Question
{
    public Guid QuestionId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TopicId { get; set; }

    // Question Content
    public string QuestionTextBangla { get; set; } = string.Empty;
    public string QuestionTextEnglish { get; set; } = string.Empty;
    public string? QuestionImageUrl { get; set; }
    public bool HasMathContent { get; set; }

    // Metadata
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Easy;
    public decimal Marks { get; set; } = 1.00m;

    // Source Information
    public SourceType SourceType { get; set; } = SourceType.Manual;
    public int? SourceYear { get; set; }
    public string? SourceReference { get; set; }
    public bool IsAIGenerated { get; set; }
    public bool IsUnique { get; set; }

    // Status
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Created;
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Subject Subject { get; set; } = null!;
    public Topic Topic { get; set; } = null!;
    public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    public QuestionExplanation? Explanation { get; set; }
}

public class QuestionOption
{
    public Guid OptionId { get; set; }
    public Guid QuestionId { get; set; }
    public string OptionTextBangla { get; set; } = string.Empty;
    public string OptionTextEnglish { get; set; } = string.Empty;
    public int OptionOrder { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime CreatedAt { get; set; }

    public Question Question { get; set; } = null!;
}

public class QuestionExplanation
{
    public Guid ExplanationId { get; set; }
    public Guid QuestionId { get; set; }
    public string ExplanationBangla { get; set; } = string.Empty;
    public string ExplanationEnglish { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Question Question { get; set; } = null!;
}
