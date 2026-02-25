# Test generating questions for an exam
param(
    [string]$examId = ""
)

$apiBase = "http://localhost:2781/api"

if ($examId -eq "") {
    Write-Host "Getting list of exams..." -ForegroundColor Cyan
    try {
        $exams = Invoke-RestMethod -Uri "$apiBase/admin/exams" -Method Get
        
        Write-Host "`n=== Available Exams ===" -ForegroundColor Green
        foreach ($exam in $exams) {
            Write-Host "$($exam.examNameEnglish) - ID: $($exam.examId)" -ForegroundColor White
        }
        
        if ($exams.Count -gt 0) {
            $examId = $exams[0].examId
            Write-Host "`nUsing first exam: $($exams[0].examNameEnglish)" -ForegroundColor Yellow
        } else {
            Write-Host "`n❌ No exams found!" -ForegroundColor Red
            exit
        }
    } catch {
        Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
        exit
    }
}

Write-Host "`nGenerating questions for exam: $examId" -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri "$apiBase/admin/exams/$examId/generate-questions" -Method Post
    
    Write-Host "✅ SUCCESS!" -ForegroundColor Green
    Write-Host "Generated $($response.totalQuestions) questions" -ForegroundColor White
    
    # Now check the questions
    Write-Host "`nVerifying questions..." -ForegroundColor Cyan
    $questionsData = Invoke-RestMethod -Uri "$apiBase/admin/exams/$examId/questions" -Method Get
    
    Write-Host "Questions in exam: $($questionsData.questions.Count)" -ForegroundColor White
    
    if ($questionsData.questions.Count -gt 0) {
        Write-Host "`n=== First 3 Questions ===" -ForegroundColor Green
        for ($i = 0; $i -lt [Math]::Min(3, $questionsData.questions.Count); $i++) {
            $q = $questionsData.questions[$i]
            Write-Host "`nQ$($q.questionNumber): $($q.questionTextEnglish.Substring(0, [Math]::Min(80, $q.questionTextEnglish.Length)))..." -ForegroundColor White
            Write-Host "  Subject: $($q.subjectName) | Difficulty: $($q.difficultyLevel)" -ForegroundColor Gray
        }
    }
    
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "`nAPI Response:" -ForegroundColor Yellow
        Write-Host $responseBody -ForegroundColor White
    }
    
    Write-Host "`nPossible reasons:" -ForegroundColor Yellow
    Write-Host "1. No questions in question bank - run .\seed-questions.ps1" -ForegroundColor White
    Write-Host "2. Subject distribution doesn't match available questions" -ForegroundColor White
    Write-Host "3. Backend server not running" -ForegroundColor White
}

Write-Host "`n"
