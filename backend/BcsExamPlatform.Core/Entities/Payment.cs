namespace BcsExamPlatform.Core.Entities;

public class Payment
{
    public Guid PaymentId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ExamId { get; set; }

    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string PaymentStatus { get; set; } = "Pending";

    public DateTime PaymentDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
    public Exam? Exam { get; set; }
}
