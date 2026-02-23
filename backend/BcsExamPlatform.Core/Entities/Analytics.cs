namespace BcsExamPlatform.Core.Entities;

public class UserPerformanceAnalytics
{
    public Guid AnalyticsId { get; set; }
    public Guid UserId { get; set; }
    public Guid SubjectId { get; set; }

    public int TotalQuestionsAttempted { get; set; }
    public int TotalCorrectAnswers { get; set; }
    public int TotalWrongAnswers { get; set; }
    public decimal AccuracyPercentage { get; set; }
    public int AverageTimePerQuestion { get; set; }

    public DateTime LastUpdated { get; set; }

    public User User { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}

public class UserTopicWeakness
{
    public Guid WeaknessId { get; set; }
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    public decimal WeaknessScore { get; set; }
    public int TotalAttempts { get; set; }
    public int CorrectAttempts { get; set; }
    public DateTime LastUpdated { get; set; }

    public User User { get; set; } = null!;
    public Topic Topic { get; set; } = null!;
}

public class AIStudyRecommendation
{
    public Guid RecommendationId { get; set; }
    public Guid UserId { get; set; }
    public string RecommendationType { get; set; } = string.Empty;
    public string RecommendationText { get; set; } = string.Empty;
    public int Priority { get; set; } = 1;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}
