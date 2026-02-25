namespace BcsExamPlatform.Core.Enums;

public enum ApprovalStatus
{
    Created = 0,
    Approved = 1,
    Rejected = 2
}

public enum DifficultyLevel
{
    Easy = 0,
    Intermediate = 1,
    Hard = 2
}

public enum LanguageMode
{
    English = 0,
    Bangla = 1,
    Bilingual = 2
}

public enum SourceType
{
    Manual = 0,
    AIGenerated = 1,
    PreviousExam = 2,
    Sample = 3
}
