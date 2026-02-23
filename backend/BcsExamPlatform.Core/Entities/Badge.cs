namespace BcsExamPlatform.Core.Entities;

public class BadgeConfiguration
{
    public Guid BadgeId { get; set; }
    public string BadgeName { get; set; } = string.Empty;
    public decimal MinMarks { get; set; }
    public decimal MaxMarks { get; set; }
    public string? BadgeColor { get; set; }
    public string? BadgeIcon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}
