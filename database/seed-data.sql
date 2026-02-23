-- BCS Exam Platform - Seed Data
-- Initial data for development and testing

-- =============================================
-- 1. INSERT SUBJECTS (BCS Prelim Standard)
-- =============================================

INSERT INTO Subjects (SubjectId, SubjectNameBangla, SubjectNameEnglish, DisplayOrder) VALUES
(NEWID(), N'বাংলা', 'Bangla', 1),
(NEWID(), N'ইংরেজি', 'English', 2),
(NEWID(), N'গাণিতিক যুক্তি', 'Mathematical Reasoning', 3),
(NEWID(), N'বাংলাদেশ বিষয়াবলী', 'Bangladesh Affairs', 4),
(NEWID(), N'আন্তর্জাতিক বিষয়াবলী', 'International Affairs', 5),
(NEWID(), N'সাধারণ বিজ্ঞান', 'General Science', 6),
(NEWID(), N'মানসিক দক্ষতা', 'Mental Ability', 7),
(NEWID(), N'ভূগোল', 'Geography', 8);

-- =============================================
-- 2. INSERT BADGE CONFIGURATION (Default)
-- =============================================

INSERT INTO BadgeConfiguration (BadgeId, BadgeName, MinMarks, MaxMarks, BadgeColor, DisplayOrder) VALUES
(NEWID(), 'Fail', 0, 99.99, '#FF0000', 1),
(NEWID(), 'Pass', 100, 119.99, '#FFA500', 2),
(NEWID(), 'Good', 120, 149.99, '#FFFF00', 3),
(NEWID(), 'Excellent', 150, 179.99, '#00FF00', 4),
(NEWID(), 'Pro', 180, 200, '#0000FF', 5);

-- =============================================
-- 3. INSERT SYSTEM SETTINGS (Default)
-- =============================================

INSERT INTO SystemSettings (SettingKey, SettingValue, SettingType, Description) VALUES
('PassMarks', '100', 'Number', 'Minimum marks required to pass'),
('DefaultExamDuration', '120', 'Number', 'Default exam duration in minutes'),
('DefaultTotalQuestions', '200', 'Number', 'Default total questions per exam'),
('AllowGuestUsers', 'true', 'Boolean', 'Allow guest users to take exams'),
('EnableTabSwitchPenalty', 'false', 'Boolean', 'Enable penalty for tab switching'),
('MaxTabSwitchAllowed', '5', 'Number', 'Maximum tab switches allowed'),
('EnableScreenshotBlock', 'true', 'Boolean', 'Block screenshots during exam'),
('AIQuestionAutoApprove', 'false', 'Boolean', 'Auto-approve AI generated questions'),
('EmailNotificationsEnabled', 'true', 'Boolean', 'Enable email notifications'),
('PushNotificationsEnabled', 'true', 'Boolean', 'Enable push notifications');

-- =============================================
-- 4. INSERT DONATION SETTINGS
-- =============================================

INSERT INTO DonationSettings (PaymentMethod, AccountNumber, AccountName, DisplayOrder) VALUES
('Bkash', '01XXXXXXXXX', 'Admin Name', 1),
('Nagad', '01XXXXXXXXX', 'Admin Name', 2);

-- =============================================
-- 5. CREATE ADMIN USER (Default)
-- =============================================

-- Password: Admin@123 (hashed - you'll need to hash this properly in your app)
INSERT INTO Users (UserId, Email, PasswordHash, FullName, IsGuest, PreferredLanguage) VALUES
(NEWID(), 'admin@bcsexam.com', 'HASH_THIS_PASSWORD', 'System Administrator', 0, 'English');

-- =============================================
-- 6. SAMPLE TOPICS FOR EACH SUBJECT
-- =============================================

-- Get Subject IDs (you'll need to adjust these based on actual IDs)
DECLARE @BanglaId UNIQUEIDENTIFIER = (SELECT TOP 1 SubjectId FROM Subjects WHERE SubjectNameEnglish = 'Bangla');
DECLARE @EnglishId UNIQUEIDENTIFIER = (SELECT TOP 1 SubjectId FROM Subjects WHERE SubjectNameEnglish = 'English');
DECLARE @MathId UNIQUEIDENTIFIER = (SELECT TOP 1 SubjectId FROM Subjects WHERE SubjectNameEnglish = 'Mathematical Reasoning');

-- Bangla Topics
INSERT INTO Topics (SubjectId, TopicNameBangla, TopicNameEnglish) VALUES
(@BanglaId, N'ব্যাকরণ', 'Grammar'),
(@BanglaId, N'সাহিত্য', 'Literature'),
(@BanglaId, N'ভাষা', 'Language');

-- English Topics
INSERT INTO Topics (SubjectId, TopicNameBangla, TopicNameEnglish) VALUES
(@EnglishId, N'গ্রামার', 'Grammar'),
(@EnglishId, N'ভোকাবুলারি', 'Vocabulary'),
(@EnglishId, N'কম্প্রিহেনশন', 'Comprehension');

-- Math Topics
INSERT INTO Topics (SubjectId, TopicNameBangla, TopicNameEnglish) VALUES
(@MathId, N'পাটিগণিত', 'Arithmetic'),
(@MathId, N'বীজগণিত', 'Algebra'),
(@MathId, N'জ্যামিতি', 'Geometry');

PRINT 'Seed data inserted successfully!';
