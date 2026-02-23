# Database Connection Guide

Your SQL Server connection string has been updated!

---

## ✅ Updated Connection String

**File:** `backend/BcsExamPlatform.API/appsettings.json`

**Connection String:**
```
Server=RAJIB\MSSQLSERVER01;
Database=BcsExamPlatform;
Integrated Security=True;
TrustServerCertificate=True;
MultipleActiveResultSets=True;
Encrypt=True
```

**Details:**
- **Server:** RAJIB\MSSQLSERVER01
- **Database:** BcsExamPlatform (will be created automatically)
- **Authentication:** Windows Authentication (Integrated Security)
- **Encryption:** Enabled with trusted certificate

---

## 🚀 Next Steps

### Step 1: Verify SQL Server is Running

1. Open **SQL Server Management Studio (SSMS)**
2. Connect to: `RAJIB\MSSQLSERVER01`
3. Use **Windows Authentication**
4. Click **Connect**

✅ **If connected successfully, SQL Server is running!**

---

### Step 2: Create Database with Migrations

Open terminal in backend folder:

```bash
cd backend

# Create migration
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Apply migration (creates database automatically)
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

**Expected Output:**
```
Build started...
Build succeeded.
Applying migration '20240223120000_InitialCreate'.
Done.
```

✅ **Database `BcsExamPlatform` is now created!**

---

### Step 3: Verify Database Creation

In **SQL Server Management Studio:**

1. Expand **Databases** folder
2. You should see **BcsExamPlatform**
3. Expand it to see tables:
   - Users
   - Subjects
   - Topics
   - Questions
   - Exams
   - ExamAttempts
   - And more...

✅ **All tables created with seed data!**

---

### Step 4: Check Seed Data

Run this query in SSMS:

```sql
USE BcsExamPlatform;

-- Check subjects (should have 8)
SELECT * FROM Subjects;

-- Check admin user (should have 1)
SELECT Email, FullName FROM Users;

-- Check badges (should have 5)
SELECT * FROM BadgeConfigurations;

-- Check system settings (should have 10)
SELECT SettingKey, SettingValue FROM SystemSettings;
```

**Expected Results:**
- ✅ 8 Subjects (Bangla, English, Math, etc.)
- ✅ 1 Admin user (admin@bcsexam.com)
- ✅ 5 Badges (Fail, Pass, Good, Excellent, Pro)
- ✅ 10 System settings

---

### Step 5: Create Sample Exam (Optional)

Run the SQL script to create a test exam with 200 questions:

**File:** `database/sample-exam-with-questions.sql`

In SSMS:
1. Open the file
2. Click **Execute** or press F5
3. Wait for completion (~30 seconds)

✅ **Sample exam created with 200 questions!**

---

### Step 6: Run the API

```bash
cd backend/BcsExamPlatform.API
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

✅ **API is running!**

---

### Step 7: Test API

Open browser: `https://localhost:5001/swagger`

You should see Swagger UI with all endpoints.

---

## 🔧 Troubleshooting

### Issue: "Cannot connect to SQL Server"

**Check 1: SQL Server is running**
```
Services → SQL Server (MSSQLSERVER01) → Status: Running
```

**Check 2: Connection string is correct**
```
Server name in SSMS: RAJIB\MSSQLSERVER01
Should match appsettings.json
```

**Check 3: Windows Authentication works**
```
Try connecting in SSMS first
If SSMS works, API should work too
```

---

### Issue: "Login failed for user"

**Solution:**
Your Windows user needs permission on SQL Server.

In SSMS:
1. Expand **Security** → **Logins**
2. Right-click → **New Login**
3. Enter your Windows username
4. Grant **sysadmin** role
5. Click OK

---

### Issue: "Database already exists"

**Solution:**
If you want to recreate:

```bash
# Drop database
dotnet ef database drop --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API --force

# Recreate
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

Or in SSMS:
```sql
DROP DATABASE BcsExamPlatform;
```

Then run migrations again.

---

### Issue: "Migration already exists"

**Solution:**
```bash
# Remove migrations folder
rm -r BcsExamPlatform.Infrastructure/Migrations

# Create new migration
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

---

### Issue: "Build failed"

**Solution:**
```bash
cd backend
dotnet clean
dotnet restore
dotnet build
```

---

## 📊 Connection String Breakdown

Your connection string explained:

```
Server=RAJIB\MSSQLSERVER01
```
- Your SQL Server instance name
- Format: ComputerName\InstanceName

```
Database=BcsExamPlatform
```
- Database name (will be created by migrations)

```
Integrated Security=True
```
- Use Windows Authentication
- No username/password needed
- Uses your Windows login

```
TrustServerCertificate=True
```
- Trust the SQL Server certificate
- Required for local development

```
MultipleActiveResultSets=True
```
- Allow multiple queries at once
- Required by Entity Framework

```
Encrypt=True
```
- Encrypt connection
- Security best practice

---

## ✅ Verification Checklist

- [ ] SQL Server is running
- [ ] Can connect via SSMS
- [ ] Connection string updated in appsettings.json
- [ ] Migrations created
- [ ] Database created
- [ ] Tables exist
- [ ] Seed data loaded
- [ ] API runs successfully
- [ ] Swagger UI accessible
- [ ] Can test endpoints

---

## 🎯 Quick Test

After everything is set up:

```bash
# 1. Run API
cd backend/BcsExamPlatform.API
dotnet run

# 2. Open Swagger
# Browser: https://localhost:5001/swagger

# 3. Test Register endpoint
POST /api/auth/register
{
  "email": "test@example.com",
  "password": "Test@123",
  "fullName": "Test User",
  "preferredLanguage": "Bangla"
}

# 4. Should return success with token
```

✅ **If this works, everything is connected properly!**

---

## 📝 Summary

**What Changed:**
- ✅ Connection string updated to use your SQL Server instance
- ✅ Using Windows Authentication
- ✅ Encryption enabled
- ✅ Ready for migrations

**What to Do:**
1. ✅ Run migrations to create database
2. ✅ Run sample exam SQL script (optional)
3. ✅ Start API
4. ✅ Test with Swagger or Postman

---

## 🆘 Still Having Issues?

### Check SQL Server Configuration

```sql
-- Check SQL Server version
SELECT @@VERSION;

-- Check database list
SELECT name FROM sys.databases;

-- Check your login
SELECT SYSTEM_USER;
```

### Check Connection from Code

The API will show detailed error messages if connection fails.
Look for errors like:
- "Login failed"
- "Cannot open database"
- "Network-related error"

---

**Your database is now configured! Run the migrations and start developing! 🚀**

---

*Last Updated: February 23, 2026*
*SQL Server Instance: RAJIB\MSSQLSERVER01*
