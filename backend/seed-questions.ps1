# Seed sample questions into the database
$apiBase = "http://localhost:2781/api"

Write-Host "Seeding 200 sample questions..." -ForegroundColor Cyan
Write-Host "This will create:" -ForegroundColor Yellow
Write-Host "  - 50 Bangla questions" -ForegroundColor White
Write-Host "  - 50 English questions" -ForegroundColor White
Write-Host "  - 100 Math questions" -ForegroundColor White
Write-Host ""

try {
    $response = Invoke-RestMethod -Uri "$apiBase/admin/seed-sample-data" -Method Post
    
    Write-Host "✅ SUCCESS!" -ForegroundColor Green
    Write-Host $response.message -ForegroundColor White
    
    Write-Host "`nNow you can:" -ForegroundColor Cyan
    Write-Host "1. Go to Exams page" -ForegroundColor White
    Write-Host "2. Click 'Generate Questions' for your exam" -ForegroundColor White
    Write-Host "3. Questions will be automatically selected" -ForegroundColor White
    
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "`nMake sure:" -ForegroundColor Yellow
    Write-Host "1. Backend server is running (dotnet run)" -ForegroundColor White
    Write-Host "2. API is accessible at $apiBase" -ForegroundColor White
}

Write-Host "`n"
