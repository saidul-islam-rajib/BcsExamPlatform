using BcsExamPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace BcsExamPlatform.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        // Ensure database is created
        await context.Database.MigrateAsync();

        // Check if data already exists
        if (await context.Subjects.AnyAsync())
        {
            return; // Database has been seeded
        }

        // Seed Subjects
        var subjects = new List<Subject>
        {
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "বাংলা",
                SubjectNameEnglish = "Bangla",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "ইংরেজি",
                SubjectNameEnglish = "English",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "গাণিতিক যুক্তি",
                SubjectNameEnglish = "Mathematical Reasoning",
                DisplayOrder = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "বাংলাদেশ বিষয়াবলী",
                SubjectNameEnglish = "Bangladesh Affairs",
                DisplayOrder = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "আন্তর্জাতিক বিষয়াবলী",
                SubjectNameEnglish = "International Affairs",
                DisplayOrder = 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "সাধারণ বিজ্ঞান",
                SubjectNameEnglish = "General Science",
                DisplayOrder = 6,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "মানসিক দক্ষতা",
                SubjectNameEnglish = "Mental Ability",
                DisplayOrder = 7,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                SubjectNameBangla = "ভূগোল",
                SubjectNameEnglish = "Geography",
                DisplayOrder = 8,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Subjects.AddRangeAsync(subjects);
        await context.SaveChangesAsync();

        // Seed Topics for each subject
        var banglaSubject = subjects.First(s => s.SubjectNameEnglish == "Bangla");
        var englishSubject = subjects.First(s => s.SubjectNameEnglish == "English");
        var mathSubject = subjects.First(s => s.SubjectNameEnglish == "Mathematical Reasoning");

        var topics = new List<Topic>
        {
            // Bangla Topics
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = banglaSubject.SubjectId,
                TopicNameBangla = "ব্যাকরণ",
                TopicNameEnglish = "Grammar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = banglaSubject.SubjectId,
                TopicNameBangla = "সাহিত্য",
                TopicNameEnglish = "Literature",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = banglaSubject.SubjectId,
                TopicNameBangla = "ভাষা",
                TopicNameEnglish = "Language",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // English Topics
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = englishSubject.SubjectId,
                TopicNameBangla = "গ্রামার",
                TopicNameEnglish = "Grammar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = englishSubject.SubjectId,
                TopicNameBangla = "ভোকাবুলারি",
                TopicNameEnglish = "Vocabulary",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = englishSubject.SubjectId,
                TopicNameBangla = "কম্প্রিহেনশন",
                TopicNameEnglish = "Comprehension",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            // Math Topics
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = mathSubject.SubjectId,
                TopicNameBangla = "পাটিগণিত",
                TopicNameEnglish = "Arithmetic",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = mathSubject.SubjectId,
                TopicNameBangla = "বীজগণিত",
                TopicNameEnglish = "Algebra",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Topic
            {
                TopicId = Guid.NewGuid(),
                SubjectId = mathSubject.SubjectId,
                TopicNameBangla = "জ্যামিতি",
                TopicNameEnglish = "Geometry",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Topics.AddRangeAsync(topics);
        await context.SaveChangesAsync();

        // Seed Badge Configuration
        var badges = new List<BadgeConfiguration>
        {
            new BadgeConfiguration
            {
                BadgeId = Guid.NewGuid(),
                BadgeName = "Fail",
                MinMarks = 0,
                MaxMarks = 99.99m,
                BadgeColor = "#FF0000",
                DisplayOrder = 1,
                IsActive = true
            },
            new BadgeConfiguration
            {
                BadgeId = Guid.NewGuid(),
                BadgeName = "Pass",
                MinMarks = 100,
                MaxMarks = 119.99m,
                BadgeColor = "#FFA500",
                DisplayOrder = 2,
                IsActive = true
            },
            new BadgeConfiguration
            {
                BadgeId = Guid.NewGuid(),
                BadgeName = "Good",
                MinMarks = 120,
                MaxMarks = 149.99m,
                BadgeColor = "#FFFF00",
                DisplayOrder = 3,
                IsActive = true
            },
            new BadgeConfiguration
            {
                BadgeId = Guid.NewGuid(),
                BadgeName = "Excellent",
                MinMarks = 150,
                MaxMarks = 179.99m,
                BadgeColor = "#00FF00",
                DisplayOrder = 4,
                IsActive = true
            },
            new BadgeConfiguration
            {
                BadgeId = Guid.NewGuid(),
                BadgeName = "Pro",
                MinMarks = 180,
                MaxMarks = 200,
                BadgeColor = "#0000FF",
                DisplayOrder = 5,
                IsActive = true
            }
        };

        await context.BadgeConfigurations.AddRangeAsync(badges);
        await context.SaveChangesAsync();

        // Seed System Settings
        var settings = new List<SystemSettings>
        {
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "PassMarks",
                SettingValue = "100",
                SettingType = "Number",
                Description = "Minimum marks required to pass",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "DefaultExamDuration",
                SettingValue = "120",
                SettingType = "Number",
                Description = "Default exam duration in minutes",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "DefaultTotalQuestions",
                SettingValue = "200",
                SettingType = "Number",
                Description = "Default total questions per exam",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "AllowGuestUsers",
                SettingValue = "true",
                SettingType = "Boolean",
                Description = "Allow guest users to take exams",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "EnableTabSwitchPenalty",
                SettingValue = "false",
                SettingType = "Boolean",
                Description = "Enable penalty for tab switching",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "MaxTabSwitchAllowed",
                SettingValue = "5",
                SettingType = "Number",
                Description = "Maximum tab switches allowed",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "EnableScreenshotBlock",
                SettingValue = "true",
                SettingType = "Boolean",
                Description = "Block screenshots during exam",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "AIQuestionAutoApprove",
                SettingValue = "false",
                SettingType = "Boolean",
                Description = "Auto-approve AI generated questions",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "EmailNotificationsEnabled",
                SettingValue = "true",
                SettingType = "Boolean",
                Description = "Enable email notifications",
                UpdatedAt = DateTime.UtcNow
            },
            new SystemSettings
            {
                SettingId = Guid.NewGuid(),
                SettingKey = "PushNotificationsEnabled",
                SettingValue = "true",
                SettingType = "Boolean",
                Description = "Enable push notifications",
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.SystemSettings.AddRangeAsync(settings);
        await context.SaveChangesAsync();

        // Seed Donation Settings
        var donations = new List<DonationSettings>
        {
            new DonationSettings
            {
                DonationId = Guid.NewGuid(),
                PaymentMethod = "Bkash",
                AccountNumber = "01XXXXXXXXX",
                AccountName = "Admin Name",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new DonationSettings
            {
                DonationId = Guid.NewGuid(),
                PaymentMethod = "Nagad",
                AccountNumber = "01XXXXXXXXX",
                AccountName = "Admin Name",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.DonationSettings.AddRangeAsync(donations);
        await context.SaveChangesAsync();

        // Create default admin user
        var adminUser = new User
        {
            UserId = Guid.NewGuid(),
            Email = "admin@bcsexam.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            FullName = "System Administrator",
            IsGuest = false,
            PreferredLanguage = "English",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }
}
