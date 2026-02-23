namespace BcsExamPlatform.Core.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    public Guid? UserId { get; set; }

    public string NotificationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public bool IsSent { get; set; }
    public DateTime? SentAt { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
}
