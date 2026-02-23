namespace BcsExamPlatform.Core.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsGuest { get; set; }
    public string PreferredLanguage { get; set; } = "Bangla";
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public ICollection<UserSocialLink> SocialLinks { get; set; } = new List<UserSocialLink>();
    public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
}

public class UserSocialLink
{
    public Guid SocialLinkId { get; set; }
    public Guid UserId { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string ProfileUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}
