namespace BcsExamPlatform.Core.Entities;

public class Subject
{
    public Guid SubjectId { get; set; }
    public string SubjectNameBangla { get; set; } = string.Empty;
    public string SubjectNameEnglish { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

public class Topic
{
    public Guid TopicId { get; set; }
    public Guid SubjectId { get; set; }
    public string TopicNameBangla { get; set; } = string.Empty;
    public string TopicNameEnglish { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Subject Subject { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
