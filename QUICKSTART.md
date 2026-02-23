# Quick Start Guide - BCS Exam Platform

Get the app running in 5 minutes!

## Prerequisites Check

```bash
# Check .NET version (need 8.0+)
dotnet --version

# Check Flutter version (need 3.0+)
flutter --version

# Check SQL Server (should be running)
# Windows: Check Services for "SQL Server"
# Or use SQL Server Management Studio to connect
```

## Step 1: Setup Backend (2 minutes)

```bash
# Navigate to backend
cd backend

# Install EF Core tools (one-time only)
dotnet tool install --global dotnet-ef

# Restore packages
dotnet restore

# Create database migration
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Create database and seed data (automatic!)
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Run the API
cd BcsExamPlatform.API
dotnet run
```

✅ Backend is now running at `https://localhost:5001`

**Test it**: Open `https://localhost:5001/swagger` in your browser

## Step 2: Setup Mobile App (2 minutes)

Open a new terminal:

```bash
# Navigate to mobile
cd mobile

# Get dependencies
flutter pub get

# Run the app
flutter run
```

✅ Mobile app is now running!

## Step 3: Test the App (1 minute)

### Option 1: Login as Admin
- Email: `admin@bcsexam.com`
- Password: `Admin@123`

### Option 2: Register New User
- Click "Register"
- Fill in your details
- Start taking exams!

### Option 3: Continue as Guest
- Click "Continue as Guest"
- Take exams without registration

## What's Included (Auto-Seeded)

✅ **8 Subjects**: Bangla, English, Math, Bangladesh Affairs, etc.
✅ **9 Topics**: Grammar, Literature, Arithmetic, etc.
✅ **5 Badge Levels**: Fail, Pass, Good, Excellent, Pro
✅ **System Settings**: All configured
✅ **Admin Account**: Ready to use

## Common Issues & Solutions

### Issue: "dotnet ef not found"
```bash
dotnet tool install --global dotnet-ef
```

### Issue: "Cannot connect to SQL Server"
- Check if SQL Server is running
- Update connection string in `backend/BcsExamPlatform.API/appsettings.json`
- For SQL Express: Use `Server=localhost\\SQLEXPRESS`

### Issue: "Build failed"
```bash
cd backend
dotnet clean
dotnet restore
dotnet build
```

### Issue: Flutter dependencies error
```bash
cd mobile
flutter clean
flutter pub get
```

## Project Structure

```
bcs-exam-platform/
├── backend/              # ASP.NET Core API
│   ├── BcsExamPlatform.API/
│   ├── BcsExamPlatform.Core/
│   └── BcsExamPlatform.Infrastructure/
├── mobile/               # Flutter App
│   └── lib/
├── database/             # SQL scripts (optional)
└── docs/                 # Documentation
```

## Next Steps

1. ✅ Backend running
2. ✅ Database created and seeded
3. ✅ Mobile app running
4. 📝 Create your first exam (via API or admin panel)
5. 📝 Add questions
6. 📝 Take an exam!

## API Endpoints (Swagger)

Visit `https://localhost:5001/swagger` to see all available endpoints:

- **Auth**: `/api/auth/register`, `/api/auth/login`
- **Exams**: `/api/exam/list`, `/api/exam/{id}`, `/api/exam/start`
- **Questions**: `/api/exam/questions/{attemptId}`
- **Results**: `/api/exam/result/{attemptId}`

## Development Workflow

### Backend Changes
```bash
# Make changes to entities
# Create new migration
dotnet ef migrations add YourMigrationName --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Apply migration
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

### Mobile Changes
```bash
# Hot reload is enabled - just save your files!
# Or press 'r' in terminal to reload
# Press 'R' for full restart
```

## Useful Commands

### Backend
```bash
# Run in watch mode (auto-restart on changes)
dotnet watch run

# Run tests
dotnet test

# Generate migration SQL script
dotnet ef migrations script --output migration.sql
```

### Mobile
```bash
# Run on specific device
flutter run -d <device-id>

# Build APK
flutter build apk

# Build iOS
flutter build ios
```

## Need Help?

- Check `backend/SETUP.md` for detailed backend setup
- Check `backend/README.md` for API documentation
- Check `docs/DEVELOPMENT_PROGRESS.md` for project status

## What's Working Now

✅ User registration and login
✅ JWT authentication
✅ Exam listing
✅ Exam details
✅ Start exam
✅ Answer questions
✅ Submit exam
✅ View results
✅ Question review with explanations
✅ Bilingual support (Bangla/English)
✅ Guest user support

## What's Next

⏳ Admin panel for exam creation
⏳ Question bank management
⏳ AI integration for question generation
⏳ Analytics dashboard
⏳ Payment integration
⏳ Email notifications

---

**You're all set! Happy coding! 🚀**
