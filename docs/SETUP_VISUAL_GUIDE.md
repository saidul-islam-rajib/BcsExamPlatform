# Visual Setup Guide - BCS Exam Platform

## 🎯 Complete Setup in 3 Steps

```
┌─────────────────────────────────────────────────────────────┐
│                    STEP 1: BACKEND                          │
│  Install Tools → Create Migration → Apply Migration → Run  │
│                     ⏱️ 2 minutes                            │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                    STEP 2: DATABASE                         │
│        Automatically Created & Seeded by EF Core            │
│                     ⏱️ 30 seconds                           │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                    STEP 3: MOBILE APP                       │
│           Get Dependencies → Run Flutter App                │
│                     ⏱️ 1 minute                             │
└─────────────────────────────────────────────────────────────┘
```

## 📋 Step-by-Step with Screenshots

### STEP 1: Backend Setup

#### 1.1 Open Terminal in Backend Folder
```
📁 bcs-exam-platform/
  └── 📁 backend/  ← Open terminal here
```

#### 1.2 Install EF Core Tools (One-time only)
```bash
dotnet tool install --global dotnet-ef
```

**Expected Output:**
```
✅ Tool 'dotnet-ef' (version '8.0.0') was successfully installed.
```

#### 1.3 Create Migration
```bash
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

**What happens:**
```
📁 BcsExamPlatform.Infrastructure/
  └── 📁 Migrations/
      ├── 📄 20240223120000_InitialCreate.cs
      ├── 📄 20240223120000_InitialCreate.Designer.cs
      └── 📄 ApplicationDbContextModelSnapshot.cs
```

**Expected Output:**
```
Build started...
Build succeeded.
✅ Done. To undo this action, use 'ef migrations remove'
```

#### 1.4 Apply Migration (Creates Database)
```bash
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

**What happens:**
```
1. Creates database: BcsExamPlatform
2. Creates all tables
3. Applies indexes
4. Runs DbInitializer (seeds data)
```

**Expected Output:**
```
Build started...
Build succeeded.
Applying migration '20240223120000_InitialCreate'.
✅ Done.
```

#### 1.5 Run the API
```bash
cd BcsExamPlatform.API
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

#### 1.6 Test with Swagger
Open browser: `https://localhost:5001/swagger`

**You should see:**
```
┌─────────────────────────────────────────┐
│         Swagger UI                      │
├─────────────────────────────────────────┤
│  Auth                                   │
│    POST /api/auth/register              │
│    POST /api/auth/login                 │
│                                         │
│  Exam                                   │
│    GET  /api/exam/list                  │
│    GET  /api/exam/{examId}              │
│    POST /api/exam/start                 │
│    GET  /api/exam/questions/{attemptId} │
│    POST /api/exam/submit-answer         │
│    POST /api/exam/submit                │
│    GET  /api/exam/result/{attemptId}    │
│    GET  /api/exam/review/{attemptId}    │
└─────────────────────────────────────────┘
```

---

### STEP 2: Verify Database

#### 2.1 Open SQL Server Management Studio (SSMS)

**Connect to:**
- Server: `localhost` (or `localhost\SQLEXPRESS`)
- Authentication: Windows Authentication

#### 2.2 Check Database
```
📁 Databases/
  └── 📁 BcsExamPlatform/  ← Should exist
      └── 📁 Tables/
          ├── 📄 dbo.Users (1 row - admin)
          ├── 📄 dbo.Subjects (8 rows)
          ├── 📄 dbo.Topics (9 rows)
          ├── 📄 dbo.Questions
          ├── 📄 dbo.QuestionOptions
          ├── 📄 dbo.Exams
          ├── 📄 dbo.ExamAttempts
          ├── 📄 dbo.BadgeConfigurations (5 rows)
          ├── 📄 dbo.SystemSettings (10 rows)
          └── 📄 dbo.DonationSettings (2 rows)
```

#### 2.3 Verify Seeded Data

**Query:**
```sql
-- Check subjects
SELECT * FROM Subjects;
```

**Expected Result:**
```
SubjectId | SubjectNameBangla | SubjectNameEnglish
----------|-------------------|-------------------
...       | বাংলা             | Bangla
...       | ইংরেজি            | English
...       | গাণিতিক যুক্তি    | Mathematical Reasoning
...       | বাংলাদেশ বিষয়াবলী | Bangladesh Affairs
...       | আন্তর্জাতিক বিষয়াবলী | International Affairs
...       | সাধারণ বিজ্ঞান     | General Science
...       | মানসিক দক্ষতা      | Mental Ability
...       | ভূগোল             | Geography
```

**Query:**
```sql
-- Check admin user
SELECT Email, FullName FROM Users;
```

**Expected Result:**
```
Email                | FullName
---------------------|---------------------
admin@bcsexam.com    | System Administrator
```

---

### STEP 3: Mobile App Setup

#### 3.1 Open New Terminal in Mobile Folder
```
📁 bcs-exam-platform/
  └── 📁 mobile/  ← Open terminal here
```

#### 3.2 Get Dependencies
```bash
flutter pub get
```

**Expected Output:**
```
Running "flutter pub get" in mobile...
Resolving dependencies...
✅ Got dependencies!
```

#### 3.3 Update API URL (if needed)

Edit `mobile/.env`:
```env
API_BASE_URL=http://localhost:5000/api  ← Change if needed
```

**For Android Emulator:**
```env
API_BASE_URL=http://10.0.2.2:5000/api
```

**For Physical Device:**
```env
API_BASE_URL=http://YOUR_COMPUTER_IP:5000/api
```

#### 3.4 Run Flutter App
```bash
flutter run
```

**Expected Output:**
```
Launching lib/main.dart on Chrome in debug mode...
✅ Application finished.
```

---

## 🎉 Success! What You Have Now

### ✅ Backend API
```
🌐 Running at: https://localhost:5001
📚 Swagger UI: https://localhost:5001/swagger
🔐 Admin Login: admin@bcsexam.com / Admin@123
```

### ✅ Database
```
💾 Database: BcsExamPlatform
📊 Tables: 15+ tables created
🌱 Seeded: Subjects, Topics, Badges, Settings, Admin
```

### ✅ Mobile App
```
📱 Running on: Your device/emulator
🎨 UI: Login, Register, Exam List, etc.
🔗 Connected to: Backend API
```

---

## 🧪 Test the Complete Flow

### Test 1: Register New User

**In Mobile App:**
1. Click "Register"
2. Fill in:
   - Email: `test@example.com`
   - Password: `Test@123`
   - Full Name: `Test User`
   - Preferred Language: `Bangla`
3. Click "Register"

**Expected:**
- ✅ User created
- ✅ Automatically logged in
- ✅ Redirected to home screen

**Verify in Database:**
```sql
SELECT * FROM Users WHERE Email = 'test@example.com';
```

### Test 2: Login

**In Mobile App:**
1. Click "Login"
2. Enter:
   - Email: `admin@bcsexam.com`
   - Password: `Admin@123`
3. Click "Login"

**Expected:**
- ✅ Login successful
- ✅ JWT token received
- ✅ Redirected to home screen

### Test 3: View Exams (When created)

**In Mobile App:**
1. Navigate to "Exams"
2. See list of available exams

**Expected:**
- ✅ Exam list displayed
- ✅ Can see exam details
- ✅ Can start exam

---

## 🔧 Troubleshooting

### Issue: "dotnet ef not found"

**Solution:**
```bash
dotnet tool install --global dotnet-ef
# Or update
dotnet tool update --global dotnet-ef
```

### Issue: "Cannot connect to SQL Server"

**Check 1: Is SQL Server running?**
```bash
# Windows: Check Services
services.msc → SQL Server (MSSQLSERVER)
```

**Check 2: Connection string**
```json
// appsettings.json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;..."
```

**Check 3: Test connection**
- Open SSMS
- Try to connect
- If fails, check SQL Server Configuration Manager

### Issue: "Build failed"

**Solution:**
```bash
cd backend
dotnet clean
dotnet restore
dotnet build
```

### Issue: "Migration already applied"

**Solution:**
```bash
# Drop database and recreate
dotnet ef database drop --force
dotnet ef database update
```

### Issue: Flutter app can't connect to API

**Android Emulator:**
```env
API_BASE_URL=http://10.0.2.2:5000/api
```

**iOS Simulator:**
```env
API_BASE_URL=http://localhost:5000/api
```

**Physical Device:**
```env
API_BASE_URL=http://192.168.1.100:5000/api  ← Your computer's IP
```

**Check firewall:**
```bash
# Windows: Allow port 5000 in firewall
# Or run as administrator
```

---

## 📊 Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                    MOBILE APP (Flutter)                 │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐             │
│  │  Login   │  │  Exams   │  │ Results  │             │
│  └──────────┘  └──────────┘  └──────────┘             │
└────────────────────┬────────────────────────────────────┘
                     │ HTTP/REST API
                     ↓
┌─────────────────────────────────────────────────────────┐
│              BACKEND API (ASP.NET Core)                 │
│  ┌──────────────┐  ┌──────────────┐                    │
│  │ Auth         │  │ Exam         │                    │
│  │ Controller   │  │ Controller   │                    │
│  └──────────────┘  └──────────────┘                    │
└────────────────────┬────────────────────────────────────┘
                     │ Entity Framework Core
                     ↓
┌─────────────────────────────────────────────────────────┐
│              DATABASE (SQL Server)                      │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐             │
│  │  Users   │  │  Exams   │  │Questions │             │
│  └──────────┘  └──────────┘  └──────────┘             │
└─────────────────────────────────────────────────────────┘
```

---

## 🎯 Next Steps

1. ✅ Backend is running
2. ✅ Database is created and seeded
3. ✅ Mobile app is running
4. 📝 Create your first exam (via API)
5. 📝 Add questions
6. 📝 Take an exam!

**Ready to develop! 🚀**
