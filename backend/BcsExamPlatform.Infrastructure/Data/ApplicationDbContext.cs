using BcsExamPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BcsExamPlatform.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Users
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserSocialLink> UserSocialLinks { get; set; } = null!;

    // Subjects & Topics
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;

    // Questions
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;
    public DbSet<QuestionExplanation> QuestionExplanations { get; set; } = null!;

    // Exams
    public DbSet<Exam> Exams { get; set; } = null!;
    public DbSet<ExamSubjectDistribution> ExamSubjectDistributions { get; set; } = null!;
    public DbSet<ExamQuestion> ExamQuestions { get; set; } = null!;

    // Exam Attempts
    public DbSet<ExamAttempt> ExamAttempts { get; set; } = null!;
    public DbSet<ExamAnswer> ExamAnswers { get; set; } = null!;

    // Badges
    public DbSet<BadgeConfiguration> BadgeConfigurations { get; set; } = null!;

    // Analytics
    public DbSet<UserPerformanceAnalytics> UserPerformanceAnalytics { get; set; } = null!;
    public DbSet<UserTopicWeakness> UserTopicWeaknesses { get; set; } = null!;
    public DbSet<AIStudyRecommendation> AIStudyRecommendations { get; set; } = null!;

    // Payments
    public DbSet<Payment> Payments { get; set; } = null!;

    // Notifications
    public DbSet<Notification> Notifications { get; set; } = null!;

    // System Settings
    public DbSet<SystemSettings> SystemSettings { get; set; } = null!;
    public DbSet<DonationSettings> DonationSettings { get; set; } = null!;
    public DbSet<PracticeQuestion> PracticeQuestions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships and constraints
        ConfigureUserEntities(modelBuilder);
        ConfigureSubjectEntities(modelBuilder);
        ConfigureQuestionEntities(modelBuilder);
        ConfigureExamEntities(modelBuilder);
        ConfigureAnalyticsEntities(modelBuilder);
        ConfigureSystemSettingsEntities(modelBuilder);
        ConfigurePaymentEntities(modelBuilder);
        ConfigureNotificationEntities(modelBuilder);
    }

    private void ConfigureUserEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PreferredLanguage).HasMaxLength(10);
        });

        modelBuilder.Entity<UserSocialLink>(entity =>
        {
            entity.HasKey(e => e.SocialLinkId);
            entity.HasOne(e => e.User)
                .WithMany(u => u.SocialLinks)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureSubjectEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId);
            entity.Property(e => e.SubjectNameBangla).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SubjectNameEnglish).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopicId);
            entity.HasOne(e => e.Subject)
                .WithMany(s => s.Topics)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureQuestionEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId);
            
            // Configure enums to be stored as strings
            entity.Property(e => e.DifficultyLevel)
                .HasConversion<string>()
                .HasMaxLength(20);
            
            entity.Property(e => e.SourceType)
                .HasConversion<string>()
                .HasMaxLength(50);
            
            entity.Property(e => e.ApprovalStatus)
                .HasConversion<string>()
                .HasMaxLength(20);
            
            entity.Property(e => e.Marks).HasColumnType("decimal(5,2)");

            entity.HasOne(e => e.Subject)
                .WithMany(s => s.Questions)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Topic)
                .WithMany(t => t.Questions)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(e => e.OptionId);
            entity.HasOne(e => e.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<QuestionExplanation>(entity =>
        {
            entity.HasKey(e => e.ExplanationId);
            entity.HasOne(e => e.Question)
                .WithOne(q => q.Explanation)
                .HasForeignKey<QuestionExplanation>(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureExamEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId);
            entity.Property(e => e.TotalMarks).HasColumnType("decimal(7,2)");
            entity.Property(e => e.ExamFee).HasColumnType("decimal(10,2)");
            
            // Configure enum to be stored as string
            entity.Property(e => e.LanguageMode)
                .HasConversion<string>()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<ExamSubjectDistribution>(entity =>
        {
            entity.HasKey(e => e.DistributionId);
            entity.HasOne(e => e.Exam)
                .WithMany(ex => ex.SubjectDistributions)
                .HasForeignKey(e => e.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => e.ExamQuestionId);
            entity.HasIndex(e => new { e.ExamId, e.QuestionNumber }).IsUnique();
        });

        modelBuilder.Entity<ExamAttempt>(entity =>
        {
            entity.HasKey(e => e.AttemptId);
            entity.Property(e => e.TotalMarksObtained).HasColumnType("decimal(7,2)");
            entity.Property(e => e.AccuracyPercentage).HasColumnType("decimal(5,2)");

            entity.HasOne(e => e.User)
                .WithMany(u => u.ExamAttempts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ExamAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId);
            entity.Property(e => e.MarksObtained).HasColumnType("decimal(5,2)");
        });
    }

    private void ConfigureAnalyticsEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserPerformanceAnalytics>(entity =>
        {
            entity.HasKey(e => e.AnalyticsId);
            entity.Property(e => e.AccuracyPercentage).HasColumnType("decimal(5,2)");
        });

        modelBuilder.Entity<UserTopicWeakness>(entity =>
        {
            entity.HasKey(e => e.WeaknessId);
            entity.Property(e => e.WeaknessScore).HasColumnType("decimal(5,2)");
        });

        modelBuilder.Entity<AIStudyRecommendation>(entity =>
        {
            entity.HasKey(e => e.RecommendationId);
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BadgeConfiguration>(entity =>
        {
            entity.HasKey(e => e.BadgeId);
            entity.Property(e => e.MinMarks).HasColumnType("decimal(7,2)");
            entity.Property(e => e.MaxMarks).HasColumnType("decimal(7,2)");
        });
    }

    private void ConfigureSystemSettingsEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemSettings>(entity =>
        {
            entity.HasKey(e => e.SettingId);
            entity.Property(e => e.SettingKey).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.SettingKey).IsUnique();
        });

        modelBuilder.Entity<DonationSettings>(entity =>
        {
            entity.HasKey(e => e.DonationId);
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<PracticeQuestion>(entity =>
        {
            entity.HasKey(e => e.PracticeQuestionId);
            entity.HasOne(e => e.Question)
                .WithMany()
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Subject)
                .WithMany()
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigurePaymentEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);
            entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasMaxLength(200);
        });
    }

    private void ConfigureNotificationEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);
            entity.Property(e => e.NotificationType).HasMaxLength(50);
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
