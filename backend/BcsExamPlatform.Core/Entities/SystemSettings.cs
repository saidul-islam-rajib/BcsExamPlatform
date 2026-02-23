namespace BcsExamPlatform.Core.Entities;

public class SystemSettings
{
    public Guid SettingId { get; set; }
    public string SettingKey { get; set; } = string.Empty;
    public string SettingValue { get; set; } = string.Empty;
    public string SettingType { get; set; } = "String";
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class DonationSettings
{
    public Guid DonationId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PracticeQuestion
{
    public Guid PracticeQuestionId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid SubjectId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Question Question { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}
