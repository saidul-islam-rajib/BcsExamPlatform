-- =============================================
-- Sample Exam with 200 Questions
-- BCS 45th Preliminary Mock Test
-- =============================================

USE BcsExamPlatform;
GO

-- =============================================
-- 1. CREATE SAMPLE EXAM
-- =============================================

DECLARE @ExamId UNIQUEIDENTIFIER = NEWID();
DECLARE @AdminUserId UNIQUEIDENTIFIER = (SELECT TOP 1 UserId FROM Users WHERE Email = 'admin@bcsexam.com');

INSERT INTO Exams (
    ExamId, 
    ExamNameBangla, 
    ExamNameEnglish, 
    ExamDate, 
    TotalQuestions, 
    DurationMinutes, 
    TotalMarks, 
    LanguageMode, 
    IsPaid, 
    ExamFee,
    AllowGuestUsers, 
    IsPublished, 
    IsActive, 
    CreatedAt, 
    UpdatedAt, 
    CreatedBy
)
VALUES (
    @ExamId,
    N'BCS ৪৫তম প্রিলিমিনারি মডেল টেস্ট',
    'BCS 45th Preliminary Model Test',
    DATEADD(DAY, 7, GETUTCDATE()), -- Exam in 7 days
    200,
    120,
    200.00,
    'Bilingual',
    0, -- Free exam
    NULL,
    1, -- Allow guests
    1, -- Published
    1, -- Active
    GETUTCDATE(),
    GETUTCDATE(),
    @AdminUserId
);

PRINT 'Exam created with ID: ' + CAST(@ExamId AS NVARCHAR(50));

-- =============================================
-- 2. CREATE SUBJECT DISTRIBUTION (25 questions each)
-- =============================================

INSERT INTO ExamSubjectDistributions (DistributionId, ExamId, SubjectId, TotalQuestions, EasyQuestions, IntermediateQuestions, HardQuestions)
SELECT 
    NEWID(),
    @ExamId,
    SubjectId,
    25, -- 25 questions per subject (8 subjects × 25 = 200)
    10, -- 10 easy
    10, -- 10 intermediate
    5   -- 5 hard
FROM Subjects
WHERE IsActive = 1;

PRINT 'Subject distribution created for 8 subjects';

-- =============================================
-- 3. CREATE 200 SAMPLE QUESTIONS
-- =============================================

-- Get subject IDs
DECLARE @BanglaId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'Bangla');
DECLARE @EnglishId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'English');
DECLARE @MathId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'Mathematical Reasoning');
DECLARE @BangladeshId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'Bangladesh Affairs');
DECLARE @InternationalId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'International Affairs');
DECLARE @ScienceId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'General Science');
DECLARE @MentalId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'Mental Ability');
DECLARE @GeographyId UNIQUEIDENTIFIER = (SELECT SubjectId FROM Subjects WHERE SubjectNameEnglish = 'Geography');

-- Get topic IDs
DECLARE @BanglaGrammarId UNIQUEIDENTIFIER = (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @BanglaId);
DECLARE @EnglishGrammarId UNIQUEIDENTIFIER = (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @EnglishId);
DECLARE @ArithmeticId UNIQUEIDENTIFIER = (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @MathId);

-- Counter for question numbers
DECLARE @QuestionCounter INT = 1;
DECLARE @CurrentQuestionId UNIQUEIDENTIFIER;

-- =============================================
-- BANGLA QUESTIONS (25 questions)
-- =============================================

WHILE @QuestionCounter <= 25
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    -- Insert Question
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, DifficultyLevel, Marks, SourceType, SourceYear, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (
        @CurrentQuestionId,
        @BanglaId,
        @BanglaGrammarId,
        N'বাংলা ভাষার উৎপত্তি কোন ভাষা থেকে? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')',
        'From which language did Bengali originate? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')',
        CASE WHEN @QuestionCounter <= 10 THEN 'Easy' WHEN @QuestionCounter <= 20 THEN 'Intermediate' ELSE 'Hard' END,
        1.00,
        'BCS_Preliminary',
        2023,
        0,
        1,
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    
    -- Insert Options
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'সংস্কৃত', 'Sanskrit', 1, 1),
        (NEWID(), @CurrentQuestionId, N'পালি', 'Pali', 2, 0),
        (NEWID(), @CurrentQuestionId, N'প্রাকৃত', 'Prakrit', 3, 0),
        (NEWID(), @CurrentQuestionId, N'অপভ্রংশ', 'Apabhramsa', 4, 0);
    
    -- Insert Explanation
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        @CurrentQuestionId,
        N'বাংলা ভাষার উৎপত্তি সংস্কৃত ভাষা থেকে। এটি ইন্দো-আর্য ভাষা পরিবারের অন্তর্গত।',
        'Bengali language originated from Sanskrit. It belongs to the Indo-Aryan language family.',
        GETUTCDATE(),
        GETUTCDATE()
    );
    
    -- Link to Exam
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 Bangla questions';

-- =============================================
-- ENGLISH QUESTIONS (25 questions)
-- =============================================

WHILE @QuestionCounter <= 50
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, DifficultyLevel, Marks, SourceType, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (
        @CurrentQuestionId,
        @EnglishId,
        @EnglishGrammarId,
        N'What is the past tense of "go"? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')',
        'What is the past tense of "go"? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')',
        CASE WHEN @QuestionCounter <= 35 THEN 'Easy' WHEN @QuestionCounter <= 45 THEN 'Intermediate' ELSE 'Hard' END,
        1.00,
        'Model_Test',
        0,
        1,
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'went', 'went', 1, 1),
        (NEWID(), @CurrentQuestionId, N'gone', 'gone', 2, 0),
        (NEWID(), @CurrentQuestionId, N'going', 'going', 3, 0),
        (NEWID(), @CurrentQuestionId, N'goes', 'goes', 4, 0);
    
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        @CurrentQuestionId,
        N'Go এর past tense হলো went। এটি একটি irregular verb।',
        'The past tense of "go" is "went". It is an irregular verb.',
        GETUTCDATE(),
        GETUTCDATE()
    );
    
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 English questions';

-- =============================================
-- MATH QUESTIONS (25 questions)
-- =============================================

WHILE @QuestionCounter <= 75
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, HasMathContent, DifficultyLevel, Marks, SourceType, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (
        @CurrentQuestionId,
        @MathId,
        @ArithmeticId,
        N'২ + ২ = কত? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')',
        'What is 2 + 2? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')',
        1,
        CASE WHEN @QuestionCounter <= 60 THEN 'Easy' WHEN @QuestionCounter <= 70 THEN 'Intermediate' ELSE 'Hard' END,
        1.00,
        'BCS_Preliminary',
        0,
        1,
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'৪', '4', 1, 1),
        (NEWID(), @CurrentQuestionId, N'৩', '3', 2, 0),
        (NEWID(), @CurrentQuestionId, N'৫', '5', 3, 0),
        (NEWID(), @CurrentQuestionId, N'৬', '6', 4, 0);
    
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        @CurrentQuestionId,
        N'২ + ২ = ৪। এটি একটি সাধারণ যোগ।',
        '2 + 2 = 4. This is a simple addition.',
        GETUTCDATE(),
        GETUTCDATE()
    );
    
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 Math questions';

-- =============================================
-- REMAINING SUBJECTS (175 more questions)
-- =============================================

-- Bangladesh Affairs (25)
WHILE @QuestionCounter <= 100
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, DifficultyLevel, Marks, SourceType, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (@CurrentQuestionId, @BangladeshId, (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @BangladeshId), 
            N'বাংলাদেশের রাজধানী কোথায়? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'What is the capital of Bangladesh? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Easy', 1.00, 'BCS_Preliminary', 0, 1, 1, GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'ঢাকা', 'Dhaka', 1, 1),
        (NEWID(), @CurrentQuestionId, N'চট্টগ্রাম', 'Chittagong', 2, 0),
        (NEWID(), @CurrentQuestionId, N'সিলেট', 'Sylhet', 3, 0),
        (NEWID(), @CurrentQuestionId, N'রাজশাহী', 'Rajshahi', 4, 0);
    
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (NEWID(), @CurrentQuestionId, N'বাংলাদেশের রাজধানী ঢাকা।', 'The capital of Bangladesh is Dhaka.', GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 Bangladesh Affairs questions';

-- International Affairs (25)
WHILE @QuestionCounter <= 125
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, DifficultyLevel, Marks, SourceType, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (@CurrentQuestionId, @InternationalId, (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @InternationalId), 
            N'জাতিসংঘের সদর দপ্তর কোথায়? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Where is the UN headquarters? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Easy', 1.00, 'Model_Test', 0, 1, 1, GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'নিউইয়র্ক', 'New York', 1, 1),
        (NEWID(), @CurrentQuestionId, N'জেনেভা', 'Geneva', 2, 0),
        (NEWID(), @CurrentQuestionId, N'প্যারিস', 'Paris', 3, 0),
        (NEWID(), @CurrentQuestionId, N'লন্ডন', 'London', 4, 0);
    
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (NEWID(), @CurrentQuestionId, N'জাতিসংঘের সদর দপ্তর নিউইয়র্কে অবস্থিত।', 'The UN headquarters is located in New York.', GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 International Affairs questions';

-- General Science (25)
WHILE @QuestionCounter <= 150
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, DifficultyLevel, Marks, SourceType, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (@CurrentQuestionId, @ScienceId, (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @ScienceId), 
            N'পানির রাসায়নিক সংকেত কী? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'What is the chemical formula of water? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Easy', 1.00, 'BCS_Preliminary', 0, 1, 1, GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'H2O', 'H2O', 1, 1),
        (NEWID(), @CurrentQuestionId, N'CO2', 'CO2', 2, 0),
        (NEWID(), @CurrentQuestionId, N'O2', 'O2', 3, 0),
        (NEWID(), @CurrentQuestionId, N'H2', 'H2', 4, 0);
    
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (NEWID(), @CurrentQuestionId, N'পানির রাসায়নিক সংকেত H2O।', 'The chemical formula of water is H2O.', GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 General Science questions';

-- Mental Ability (25)
WHILE @QuestionCounter <= 175
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, DifficultyLevel, Marks, SourceType, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (@CurrentQuestionId, @MentalId, (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @MentalId), 
            N'ধারাবাহিকতা: ২, ৪, ৬, ৮, ? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Series: 2, 4, 6, 8, ? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Easy', 1.00, 'Model_Test', 0, 1, 1, GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'১০', '10', 1, 1),
        (NEWID(), @CurrentQuestionId, N'৯', '9', 2, 0),
        (NEWID(), @CurrentQuestionId, N'১১', '11', 3, 0),
        (NEWID(), @CurrentQuestionId, N'১২', '12', 4, 0);
    
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (NEWID(), @CurrentQuestionId, N'এটি একটি জোড় সংখ্যার ধারা। পরবর্তী সংখ্যা ১০।', 'This is a series of even numbers. The next number is 10.', GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 Mental Ability questions';

-- Geography (25)
WHILE @QuestionCounter <= 200
BEGIN
    SET @CurrentQuestionId = NEWID();
    
    INSERT INTO Questions (QuestionId, SubjectId, TopicId, QuestionTextBangla, QuestionTextEnglish, DifficultyLevel, Marks, SourceType, IsAIGenerated, IsApproved, IsActive, CreatedAt, UpdatedAt)
    VALUES (@CurrentQuestionId, @GeographyId, (SELECT TOP 1 TopicId FROM Topics WHERE SubjectId = @GeographyId), 
            N'পৃথিবীর বৃহত্তম মহাদেশ কোনটি? (প্রশ্ন ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Which is the largest continent? (Question ' + CAST(@QuestionCounter AS NVARCHAR(10)) + ')', 
            'Easy', 1.00, 'BCS_Preliminary', 0, 1, 1, GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO QuestionOptions (OptionId, QuestionId, OptionTextBangla, OptionTextEnglish, OptionOrder, IsCorrect)
    VALUES 
        (NEWID(), @CurrentQuestionId, N'এশিয়া', 'Asia', 1, 1),
        (NEWID(), @CurrentQuestionId, N'আফ্রিকা', 'Africa', 2, 0),
        (NEWID(), @CurrentQuestionId, N'ইউরোপ', 'Europe', 3, 0),
        (NEWID(), @CurrentQuestionId, N'আমেরিকা', 'America', 4, 0);
    
    INSERT INTO QuestionExplanations (ExplanationId, QuestionId, ExplanationBangla, ExplanationEnglish, CreatedAt, UpdatedAt)
    VALUES (NEWID(), @CurrentQuestionId, N'এশিয়া পৃথিবীর বৃহত্তম মহাদেশ।', 'Asia is the largest continent in the world.', GETUTCDATE(), GETUTCDATE());
    
    INSERT INTO ExamQuestions (ExamQuestionId, ExamId, QuestionId, QuestionNumber, DisplayOrder)
    VALUES (NEWID(), @ExamId, @CurrentQuestionId, @QuestionCounter, @QuestionCounter);
    
    SET @QuestionCounter = @QuestionCounter + 1;
END

PRINT 'Created 25 Geography questions';

-- =============================================
-- SUMMARY
-- =============================================

PRINT '';
PRINT '========================================';
PRINT 'SAMPLE EXAM CREATED SUCCESSFULLY!';
PRINT '========================================';
PRINT 'Exam ID: ' + CAST(@ExamId AS NVARCHAR(50));
PRINT 'Total Questions: 200';
PRINT 'Subjects: 8 (25 questions each)';
PRINT 'Status: Published and Active';
PRINT '';
PRINT 'You can now test the API with this exam!';
PRINT 'Use Postman collection to:';
PRINT '1. Get exam list';
PRINT '2. Start exam';
PRINT '3. Answer questions';
PRINT '4. Submit and get results';
PRINT '========================================';

-- Display exam details
SELECT 
    ExamId,
    ExamNameEnglish,
    ExamDate,
    TotalQuestions,
    DurationMinutes,
    IsPublished,
    AllowGuestUsers
FROM Exams
WHERE ExamId = @ExamId;

-- Display question count by subject
SELECT 
    s.SubjectNameEnglish,
    COUNT(eq.QuestionId) as QuestionCount
FROM ExamQuestions eq
INNER JOIN Questions q ON eq.QuestionId = q.QuestionId
INNER JOIN Subjects s ON q.SubjectId = s.SubjectId
WHERE eq.ExamId = @ExamId
GROUP BY s.SubjectNameEnglish
ORDER BY s.DisplayOrder;
