-- Add ApprovalStatus column to Questions table
ALTER TABLE Questions ADD ApprovalStatus NVARCHAR(20) NULL;

-- Migrate data from IsApproved to ApprovalStatus
UPDATE Questions 
SET ApprovalStatus = CASE 
    WHEN IsApproved = 1 THEN 'Approved'
    ELSE 'Created'
END;

-- Make ApprovalStatus NOT NULL after data migration
ALTER TABLE Questions ALTER COLUMN ApprovalStatus NVARCHAR(20) NOT NULL;

-- Drop the old IsApproved column
ALTER TABLE Questions DROP COLUMN IsApproved;

-- Ensure SourceType column exists and has proper values
UPDATE Questions 
SET SourceType = 'Manual'
WHERE SourceType IS NULL OR SourceType = '';

UPDATE Questions 
SET SourceType = 'AIGenerated'
WHERE SourceType = 'AI Generated';

-- Ensure DifficultyLevel values match enum names (they should already)
-- Easy, Intermediate, Hard are already correct

-- Ensure LanguageMode in Exams table matches enum names
UPDATE Exams
SET LanguageMode = 'Bilingual'
WHERE LanguageMode IS NULL OR LanguageMode = '';
