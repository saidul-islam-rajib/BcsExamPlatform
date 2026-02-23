@echo off
echo ========================================
echo BCS Exam Platform - Flutter Setup
echo ========================================
echo.

echo Step 1: Getting Flutter dependencies...
flutter pub get
if %errorlevel% neq 0 (
    echo ERROR: Failed to get dependencies
    pause
    exit /b 1
)
echo.

echo Step 2: Analyzing code for errors...
flutter analyze
if %errorlevel% neq 0 (
    echo WARNING: Code analysis found issues
    echo.
)

echo ========================================
echo Setup Complete!
echo ========================================
echo.
echo To run the app on Chrome:
echo   flutter run -d chrome
echo.
echo To run the app on Windows:
echo   flutter run -d windows
echo.
pause
