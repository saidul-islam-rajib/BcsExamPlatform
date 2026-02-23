-- BCS Exam Platform Database Schema
-- SQL Server 2019+

-- =============================================
-- 1. USERS & AUTHENTICATION
-- =============================================

CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NULL, -- NULL for guest users
    FullName NVARCHAR(200) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    IsGuest BIT NOT NULL DEFAULT 0,
    PreferredLanguage NVARCHAR(10) NOT NULL DEFAULT 'Bangla', -- 'Bangla' or 'English'
    ProfileImageUrl NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    LastLoginAt DATETIME2 NULL
);

CREATE TABLE UserSocialLinks (
    SocialLinkId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(UserId) ON DELETE CASCADE,
    Platform NVARCHAR(50) NOT NULL, -- 'Facebook', 'LinkedIn', 'Twitter', etc.
    ProfileUrl NVARCHAR(500) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- 2. SUBJECTS & TOPICS
-- =============================================

CREATE TABLE Subjects (
    SubjectId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SubjectNameBangla NVARCHAR(200) NOT NULL,
    SubjectNameEnglish NVARCHAR(200) NOT NULL,
    DisplayOrder INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Topics (
    TopicId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SubjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectId) ON DELETE CASCADE,
    TopicNameBangla NVARCHAR(300) NOT NULL,
    TopicNameEnglish NVARCHAR(300) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- 3. QUESTIONS
-- =============================================

CREATE TABLE Questions (
    QuestionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SubjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectId),
    TopicId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Topics(TopicId),
    
    -- Question Content
    QuestionTextBangla NVARCHAR(MAX) NOT NULL,
    QuestionTextEnglish NVARCHAR(MAX) NOT NULL,
    QuestionImageUrl NVARCHAR(500) NULL,
    HasMathContent BIT NOT NULL DEFAULT 0,
    
    -- Metadata
    DifficultyLevel NVARCHAR(20) NOT NULL CHECK (DifficultyLevel IN ('Easy', 'Intermediate', 'Hard')),
    Marks DECIMAL(5,2) NOT NULL DEFAULT 1.00,
    
    -- Source Information
    SourceType NVARCHAR(50) NOT NULL, -- 'BCS_Preliminary', 'Model_Test', 'Book', 'Website', 'AI_Generated'
    SourceYear INT NULL,
    SourceReference NVARCHAR(500) NULL,
    IsAIGenerated BIT NOT NULL DEFAULT 0,
    IsUnique BIT NOT NULL DEFAULT 0,
    
    -- Status
    IsApproved BIT NOT NULL DEFAULT 0,
    ApprovedBy UNIQUEIDENTIFIER NULL,
    ApprovedAt DATETIME2 NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE QuestionOptions (
    OptionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    QuestionId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Questions(QuestionId) ON DELETE CASCADE,
    OptionTextBangla NVARCHAR(MAX) NOT NULL,
    OptionTextEnglish NVARCHAR(MAX) NOT NULL,
    OptionOrder INT NOT NULL, -- 1, 2, 3, 4
    IsCorrect BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE QuestionExplanations (
    ExplanationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    QuestionId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Questions(QuestionId) ON DELETE CASCADE,
    ExplanationBangla NVARCHAR(MAX) NOT NULL,
    ExplanationEnglish NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- 4. EXAMS
-- =============================================

CREATE TABLE Exams (
    ExamId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ExamNameBangla NVARCHAR(300) NOT NULL,
    ExamNameEnglish NVARCHAR(300) NOT NULL,
    ExamDate DATETIME2 NOT NULL,
    
    -- Exam Configuration
    TotalQuestions INT NOT NULL DEFAULT 200,
    DurationMinutes INT NOT NULL DEFAULT 120,
    TotalMarks DECIMAL(7,2) NOT NULL DEFAULT 200.00,
    
    -- Language Settings
    LanguageMode NVARCHAR(20) NOT NULL CHECK (LanguageMode IN ('Bangla', 'English', 'Bilingual')),
    
    -- Access Control
    IsPaid BIT NOT NULL DEFAULT 0,
    ExamFee DECIMAL(10,2) NULL,
    AllowGuestUsers BIT NOT NULL DEFAULT 1,
    
    -- Status
    IsPublished BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL
);

CREATE TABLE ExamSubjectDistribution (
    DistributionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ExamId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Exams(ExamId) ON DELETE CASCADE,
    SubjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectId),
    TotalQuestions INT NOT NULL,
    EasyQuestions INT NOT NULL DEFAULT 0,
    IntermediateQuestions INT NOT NULL DEFAULT 0,
    HardQuestions INT NOT NULL DEFAULT 0
);

CREATE TABLE ExamQuestions (
    ExamQuestionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ExamId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Exams(ExamId) ON DELETE CASCADE,
    QuestionId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Questions(QuestionId),
    QuestionNumber INT NOT NULL, -- 1 to 200
    DisplayOrder INT NOT NULL, -- Can be randomized
    UNIQUE(ExamId, QuestionNumber)
);

-- =============================================
-- 5. EXAM ATTEMPTS & RESULTS
-- =============================================

CREATE TABLE ExamAttempts (
    AttemptId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ExamId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Exams(ExamId),
    UserId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Users(UserId), -- NULL for guest
    GuestEmail NVARCHAR(255) NULL, -- For guest users
    
    -- Attempt Details
    SelectedLanguage NVARCHAR(10) NOT NULL, -- 'Bangla' or 'English'
    StartedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SubmittedAt DATETIME2 NULL,
    TotalTimeSpentSeconds INT NULL,
    
    -- Results
    TotalMarksObtained DECIMAL(7,2) NULL,
    TotalCorrectAnswers INT NULL,
    TotalWrongAnswers INT NULL,
    TotalUnanswered INT NULL,
    AccuracyPercentage DECIMAL(5,2) NULL,
    
    -- Status
    IsCompleted BIT NOT NULL DEFAULT 0,
    Rank INT NULL,
    
    -- Anti-cheating
    TabSwitchCount INT NOT NULL DEFAULT 0,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE ExamAnswers (
    AnswerId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AttemptId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES ExamAttempts(AttemptId) ON DELETE CASCADE,
    QuestionId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Questions(QuestionId),
    SelectedOptionId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES QuestionOptions(OptionId),
    IsCorrect BIT NULL,
    MarksObtained DECIMAL(5,2) NULL,
    TimeSpentSeconds INT NOT NULL DEFAULT 0,
    IsMarkedForReview BIT NOT NULL DEFAULT 0,
    AnsweredAt DATETIME2 NULL
);

-- =============================================
-- 6. BADGES & RANKINGS
-- =============================================

CREATE TABLE BadgeConfiguration (
    BadgeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    BadgeName NVARCHAR(100) NOT NULL,
    MinMarks DECIMAL(7,2) NOT NULL,
    MaxMarks DECIMAL(7,2) NOT NULL,
    BadgeColor NVARCHAR(50) NULL,
    BadgeIcon NVARCHAR(100) NULL,
    DisplayOrder INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- =============================================
-- 7. ANALYTICS
-- =============================================

CREATE TABLE UserPerformanceAnalytics (
    AnalyticsId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(UserId) ON DELETE CASCADE,
    SubjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectId),
    
    TotalQuestionsAttempted INT NOT NULL DEFAULT 0,
    TotalCorrectAnswers INT NOT NULL DEFAULT 0,
    TotalWrongAnswers INT NOT NULL DEFAULT 0,
    AccuracyPercentage DECIMAL(5,2) NOT NULL DEFAULT 0,
    AverageTimePerQuestion INT NOT NULL DEFAULT 0,
    
    LastUpdated DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE UserTopicWeakness (
    WeaknessId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(UserId) ON DELETE CASCADE,
    TopicId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Topics(TopicId),
    WeaknessScore DECIMAL(5,2) NOT NULL, -- 0-100, higher = weaker
    TotalAttempts INT NOT NULL DEFAULT 0,
    CorrectAttempts INT NOT NULL DEFAULT 0,
    LastUpdated DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE AIStudyRecommendations (
    RecommendationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(UserId) ON DELETE CASCADE,
    RecommendationType NVARCHAR(50) NOT NULL, -- 'WeakSubject', 'WeakTopic', 'TimeManagement', etc.
    RecommendationText NVARCHAR(MAX) NOT NULL,
    Priority INT NOT NULL DEFAULT 1, -- 1=High, 2=Medium, 3=Low
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- 8. PAYMENTS
-- =============================================

CREATE TABLE Payments (
    PaymentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Users(UserId),
    ExamId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Exams(ExamId),
    
    Amount DECIMAL(10,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL, -- 'Bkash', 'Nagad', 'Card', etc.
    TransactionId NVARCHAR(200) NULL,
    PaymentStatus NVARCHAR(20) NOT NULL, -- 'Pending', 'Completed', 'Failed', 'Refunded'
    
    PaymentDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- 9. NOTIFICATIONS
-- =============================================

CREATE TABLE Notifications (
    NotificationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Users(UserId), -- NULL = broadcast to all
    
    NotificationType NVARCHAR(50) NOT NULL, -- 'ExamAnnouncement', 'ExamReminder', 'ResultPublished'
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    
    IsSent BIT NOT NULL DEFAULT 0,
    SentAt DATETIME2 NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    ReadAt DATETIME2 NULL,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- 10. PRACTICE SECTION (1000 Questions)
-- =============================================

CREATE TABLE PracticeQuestions (
    PracticeQuestionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    QuestionId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Questions(QuestionId),
    SubjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectId),
    DisplayOrder INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- 11. ADMIN SETTINGS
-- =============================================

CREATE TABLE SystemSettings (
    SettingId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(MAX) NOT NULL,
    SettingType NVARCHAR(50) NOT NULL, -- 'String', 'Number', 'Boolean', 'JSON'
    Description NVARCHAR(500) NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE DonationSettings (
    DonationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PaymentMethod NVARCHAR(50) NOT NULL, -- 'Bkash', 'Nagad'
    AccountNumber NVARCHAR(50) NOT NULL,
    AccountName NVARCHAR(200) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- =============================================
-- INDEXES FOR PERFORMANCE
-- =============================================

CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_IsGuest ON Users(IsGuest);
CREATE INDEX IX_Questions_SubjectId ON Questions(SubjectId);
CREATE INDEX IX_Questions_TopicId ON Questions(TopicId);
CREATE INDEX IX_Questions_DifficultyLevel ON Questions(DifficultyLevel);
CREATE INDEX IX_Questions_IsApproved ON Questions(IsApproved);
CREATE INDEX IX_ExamAttempts_ExamId ON ExamAttempts(ExamId);
CREATE INDEX IX_ExamAttempts_UserId ON ExamAttempts(UserId);
CREATE INDEX IX_ExamAttempts_GuestEmail ON ExamAttempts(GuestEmail);
CREATE INDEX IX_ExamAnswers_AttemptId ON ExamAnswers(AttemptId);
CREATE INDEX IX_Exams_ExamDate ON Exams(ExamDate);
CREATE INDEX IX_Exams_IsPublished ON Exams(IsPublished);
