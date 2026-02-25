# Check if questions exist in the database
$apiBase = "http://localhost:2781/api"

Write-Host "Checking question bank..." -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri "$apiBase/admin/questions?pageSize=1" -Method Get
    
    Write-Host "`n=== Question Bank Status ===" -ForegroundColor Green
    Write-Host "Total Questions: $($response.totalCount)" -ForegroundColor Yellow
    
    if ($response.totalCount -eq 0) {
        Write-Host "`n❌ NO QUESTIONS FOUND!" -ForegroundColor Red
        Write-Host "`nYou need to add questions first:" -ForegroundColor Yellow
        Write-Host "1. Go to Dashboard" -ForegroundColor White
        Write-Host "2. Click 'Quick Setup' tab" -ForegroundColor White
        Write-Host "3. Click 'Seed 200 Sample Questions'" -ForegroundColor White
        Write-Host "`nOR run: .\seed-questions.ps1" -ForegroundColor Cyan
    } else {
        Write-Host "✅ Questions available!" -ForegroundColor Green
        
        # Check subjects
        Write-Host "`n=== Checking Subjects ===" -ForegroundColor Green
        $subjects = Invoke-RestMethod -Uri "$apiBase/admin/subjects" -Method Get
        
        foreach ($subject in $subjects) {
            $subjectQuestions = Invoke-RestMethod -Uri "$apiBase/admin/questions?subjectId=$($subject.subjectId)&pageSize=1" -Method Get
            Write-Host "$($subject.subjectNameEnglish): $($subjectQuestions.totalCount) questions" -ForegroundColor White
        }
    }
    
} catch {
    Write-Host "`n❌ Error checking questions: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Make sure the backend server is running!" -ForegroundColor Yellow
}

Write-Host "`n"
