# 🚀 Quick Reference - BCS Exam Platform

## Start Backend (One Command!)

```bash
cd backend/BcsExamPlatform.API
dotnet run
```

**That's it!** Database creates automatically.

---

## 🌐 Access Points

- **Swagger UI**: https://localhost:2780/swagger
- **API Base**: https://localhost:2780/api
- **HTTP**: http://localhost:2781

---

## 🔐 Default Login

```
Email: admin@bcsexam.com
Password: Admin@123
```

---

## 📊 Database Info

- **Server**: RAJIB\MSSQLSERVER01
- **Database**: BcsExamPlatform
- **Auto-Created**: ✅ Yes
- **Auto-Seeded**: ✅ Yes

---

## 🧪 Test with Postman

1. Import: `postman/BCS_Exam_Platform.postman_collection.json`
2. Import: `postman/BCS_Exam_Platform.postman_environment.json`
3. Follow: `POSTMAN_TESTING_GUIDE.md`

---

## 📱 Mobile App

**Status**: Code complete, waiting for Flutter SDK

**Install Flutter**:
1. Read: `FLUTTER_INSTALLATION_GUIDE.md`
2. Or: `FLUTTER_QUICK_SETUP.md` (web only)

---

## 📚 Documentation

- `AUTO_DATABASE_SETUP_SUCCESS.md` - Database setup guide
- `BUILD_STATUS.md` - Current build status
- `BACKEND_REQUIREMENTS_CHECKLIST.md` - What's done vs pending
- `API_TESTING_COMPLETE.md` - API testing info
- `DATABASE_CONNECTION_GUIDE.md` - Connection details

---

## 🔧 Common Commands

### Run Backend:
```bash
cd backend/BcsExamPlatform.API
dotnet run
```

### Build Backend:
```bash
cd backend
dotnet build
```

### View Logs:
Check console output when running `dotnet run`

### Stop Backend:
Press `Ctrl+C` in terminal

---

## ✅ What's Working

- User registration/login
- JWT authentication
- Exam listing
- Start exam
- Submit answers
- View results
- Review answers
- Badge assignment
- Bilingual support

---

## ⏳ What's Pending

- Admin panel APIs
- Question management APIs
- Sample exam data (use SQL script)
- Flutter SDK installation
- Mobile app testing

---

## 🆘 Quick Troubleshooting

**Backend won't start?**
- Check SQL Server is running
- Check connection string in `appsettings.json`

**Port already in use?**
- Kill process or change port in `launchSettings.json`

**Database error?**
- Check SQL Server service
- Verify Windows Authentication works

---

## 📞 Need Help?

Check these files:
1. `AUTO_DATABASE_SETUP_SUCCESS.md` - Full setup guide
2. `BUILD_STATUS.md` - Current status
3. `BACKEND_REQUIREMENTS_CHECKLIST.md` - Feature list

---

*Quick Reference Card - Keep this handy!*
