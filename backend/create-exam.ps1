# Quick script to create a complete exam with 200 questions

$API_BASE = "http://localhost:2781/api"

Write-Host "Creating BCS Preliminary Exam 2026..." -ForegroundColor Green

# Step 1: Get subjects
$subjects = Invoke-RestMethod -Uri "$API_BASE/admin/subjects" -Method GET
$banglaSubject = $subjects | Where-Object { $_.subjectNameEnglish -eq "Bangla" }
$englishSubject = $subjects | Where-Object { $_.subjectNameEnglish -eq "English" }
$mathSubject = $subjects | Where-Object { $_.subjectNameEnglish -eq "Mathematical Reasoning" }

Write-Host "Found subjects: Bangla, English, Math" -ForegroundColor Cyan

# Step 2: Create exam
$examData = @{
    examNameBangla = "বিসিএস প্রিলিমিনারি পরীক্ষা ২০২৬"
    examNameEnglish = "BCS Preliminary Exam 2026"
    examDate = (Get-Date).AddDays(7).ToString("yyyy-MM-ddTHH:mm:ss")
    totalQuestions = 200
    durationMinutes = 120
    totalMarks = 200
    languageMode = "Bilingual"
    isPaid = $false
    examFee = 0
    allowGuestUsers = $true
} | ConvertTo-Json

$exam = Invoke-RestMethod -Uri "$API_BASE/admin/exams" -Method POST -Body $examData -ContentType "application/json"
$examId = $exam.examId

Write-Host "Created exam with ID: $examId" -ForegroundColor Green

# Step 3: Add subject distribution
$distribution = @(
    @{
        subjectId = $banglaSubject.subjectId
        totalQuestions = 50
        easyQuestions = 20
        intermediateQuestions = 20
        hardQuestions = 10
    },
    @{
        subjectId = $englishSubject.subjectId
        totalQuestions = 50
        easyQuestions = 20
        intermediateQuestions = 20
        hardQuestions = 10
    },
    @{
        subjectId = $mathSubject.subjectId
        totalQuestions = 100
        easyQuestions = 40
        intermediateQuestions = 40
        hardQuestions = 20
    }
) | ConvertTo-Json

Invoke-RestMethod -Uri "$API_BASE/admin/exams/$examId/subject-distribution" -Method POST -Body $distribution -ContentType "application/json"

Write-Host "Added subject distribution" -ForegroundColor Green

# Step 4: Generate questions
Invoke-RestMethod -Uri "$API_BASE/admin/exams/$examId/generate-questions" -Method POST

Write-Host "Generated 200 questions for the exam" -ForegroundColor Green

# Step 5: Publish exam
Invoke-RestMethod -Uri "$API_BASE/admin/exams/$examId/publish" -Method PUT

Write-Host "Published exam!" -ForegroundColor Green
Write-Host ""
Write-Host "✅ Exam created successfully!" -ForegroundColor Green
Write-Host "Students can now see and take this exam in the mobile app" -ForegroundColor Cyan
Write-Host "Exam ID: $examId" -ForegroundColor Yellow
