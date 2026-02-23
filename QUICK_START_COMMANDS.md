# Quick Start Commands

All commands you need to get started!

---

## 🗄️ Database Setup

### Create Database

```bash
cd backend

# Create migration
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Apply migration (creates database + seeds data)
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

✅ **Database created at:** `RAJIB\MSSQLSERVER01`

---

## 🚀 Run Backend API

```bash
cd backend/BcsExamPlatform.API
dotnet run
```

✅ **API running at:**
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000
- Swagger: https://localhost:5001/swagger

---

## 📱 Run Mobile App

```bash
cd mobile

# Get dependencies
flutter pub get

# Run on Chrome
flutter run -d chrome

# Or run on Android
flutter run -d android
```

✅ **App running!**

---

## 🧪 Test with Postman

1. Import: `postman/BCS_Exam_Platform.postman_collection.json`
2. Import: `postman/BCS_Exam_Platform.postman_environment.json`
3. Select environment: "BCS Exam Platform - Local"
4. Test endpoints!

---

## 📊 Create Sample Exam

In SQL Server Management Studio:

```sql
-- Connect to: RAJIB\MSSQLSERVER01
-- Open file: database/sample-exam-with-questions.sql
-- Press F5 to execute
```

✅ **Sample exam created with 200 questions!**

---

## 🔧 Useful Commands

### Backend

```bash
# Build
dotnet build

# Clean
dotnet clean

# Restore packages
dotnet restore

# Run
dotnet run

# Create migration
dotnet ef migrations add MigrationName --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Apply migration
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Drop database
dotnet ef database drop --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API --force
```

### Mobile

```bash
# Get dependencies
flutter pub get

# Clean
flutter clean

# Run on Chrome
flutter run -d chrome

# Run on Android
flutter run -d android

# List devices
flutter devices

# Check Flutter
flutter doctor
```

### Database (SQL)

```sql
-- Check databases
SELECT name FROM sys.databases;

-- Use database
USE BcsExamPlatform;

-- Check tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES;

-- Check subjects
SELECT * FROM Subjects;

-- Check admin user
SELECT * FROM Users;

-- Check exams
SELECT * FROM Exams;
```

---

## ✅ Complete Setup Flow

```bash
# 1. Create database
cd backend
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# 2. Create sample exam (in SSMS)
# Run: database/sample-exam-with-questions.sql

# 3. Run backend
cd BcsExamPlatform.API
dotnet run

# 4. Test API
# Open: https://localhost:5001/swagger

# 5. Run mobile app
cd ../../mobile
flutter pub get
flutter run -d chrome
```

---

## 🎯 Default Credentials

**Admin:**
- Email: `admin@bcsexam.com`
- Password: `Admin@123`

**Test User:**
- Email: `student@example.com`
- Password: `Student@123`

---

## 📁 Project Structure

```
bcs-exam-platform/
├── backend/
│   ├── BcsExamPlatform.API/          ← Run: dotnet run
│   ├── BcsExamPlatform.Core/
│   └── BcsExamPlatform.Infrastructure/
├── mobile/                            ← Run: flutter run
├── database/
│   └── sample-exam-with-questions.sql ← Run in SSMS
└── postman/                           ← Import to Postman
```

---

## 🔗 Quick Links

- **API Swagger:** https://localhost:5001/swagger
- **SQL Server:** RAJIB\MSSQLSERVER01
- **Database:** BcsExamPlatform

---

## 🆘 Quick Fixes

**Backend won't start:**
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

**Database error:**
```bash
dotnet ef database drop --force
dotnet ef database update
```

**Mobile app error:**
```bash
flutter clean
flutter pub get
flutter run -d chrome
```

---

**Copy and paste these commands as needed! 🚀**

---

*Last Updated: February 23, 2026*
