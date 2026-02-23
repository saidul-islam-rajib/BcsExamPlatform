# 🎓 BCS Exam Platform

A comprehensive exam platform for BCS (Bangladesh Civil Service) exam preparation with bilingual support (Bangla/English). Built with ASP.NET Core 8.0 backend and Flutter mobile app.

![Status](https://img.shields.io/badge/Backend-Working-success)
![Status](https://img.shields.io/badge/Database-Auto--Setup-success)
![Status](https://img.shields.io/badge/Mobile-Code%20Complete-success)
![License](https://img.shields.io/badge/License-MIT-blue)

## 🌟 Features

### For Students
- ✅ User registration and login with JWT authentication
- ✅ Guest mode (take exams without registration)
- ✅ 200 MCQ questions per exam
- ✅ 8 subjects with topic-wise questions
- ✅ Real-time exam timer
- ✅ Mark questions for review
- ✅ Instant results with detailed analysis
- ✅ Badge system (Fail, Pass, Good, Excellent, Pro)
- ✅ Ranking system
- ✅ Review answers with explanations
- ✅ Bilingual support (Bangla/English)
- ✅ Subject-wise performance tracking

### For Admins (Pending)
- ⏳ Create and manage exams
- ⏳ Add and manage questions
- ⏳ User management
- ⏳ Analytics dashboard
- ⏳ Notification system

## 🏗️ Architecture

### Backend
- **Framework**: ASP.NET Core 8.0
- **Architecture**: Clean Architecture (3 layers)
- **Database**: SQL Server with EF Core Code-First
- **Authentication**: JWT Bearer Token
- **API Documentation**: Swagger/OpenAPI

### Mobile App
- **Framework**: Flutter
- **State Management**: BLoC Pattern
- **Architecture**: Clean Architecture
- **Supported Platforms**: Android, iOS, Web

## 📊 Project Structure

```
BCS/
├── backend/
│   ├── BcsExamPlatform.API/          # API Layer (Controllers, DTOs)
│   ├── BcsExamPlatform.Core/         # Domain Layer (Entities)
│   └── BcsExamPlatform.Infrastructure/ # Data Layer (DbContext, Migrations)
│
├── mobile/
│   └── lib/
│       ├── core/                     # Core services (API, Storage)
│       ├── data/                     # Models
│       ├── blocs/                    # State management
│       └── screens/                  # UI screens
│
├── postman/                          # API testing collection
├── database/                         # SQL scripts
└── [Documentation files]
```

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server (Windows Authentication)
- Flutter SDK (for mobile app)
- Git

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/saidul-islam-rajib/bcs-exam-platform.git
   cd bcs-exam-platform
   ```

2. **Update connection string** (if needed)
   
   Edit `backend/BcsExamPlatform.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=BcsExamPlatform;Integrated Security=True;TrustServerCertificate=True"
   }
   ```

3. **Run the backend**
   ```bash
   cd backend/BcsExamPlatform.API
   dotnet run
   ```

   That's it! The database will be created automatically with initial data.

4. **Access Swagger UI**
   ```
   https://localhost:2780/swagger
   ```

### Mobile App Setup

1. **Install Flutter dependencies**
   ```bash
   cd mobile
   flutter pub get
   ```

2. **Run the app**
   ```bash
   flutter run
   ```

## 🔐 Default Credentials

```
Email: admin@bcsexam.com
Password: Admin@123
```

## 📚 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user

### Exams
- `GET /api/exam/list` - Get all exams
- `GET /api/exam/{id}` - Get exam details
- `POST /api/exam/start` - Start exam
- `GET /api/exam/{attemptId}/questions` - Get exam questions
- `POST /api/exam/answer` - Submit answer
- `POST /api/exam/submit` - Submit complete exam
- `GET /api/exam/result/{attemptId}` - Get exam results
- `GET /api/exam/review/{attemptId}` - Review answers

## 🗄️ Database Schema

### Core Tables
- **Users** - User accounts
- **Subjects** - 8 subjects (Bangla, English, Math, etc.)
- **Topics** - Topics under each subject
- **Questions** - Question bank
- **QuestionOptions** - MCQ options
- **QuestionExplanations** - Answer explanations
- **Exams** - Exam configurations
- **ExamAttempts** - User exam attempts
- **ExamAnswers** - User answers
- **BadgeConfigurations** - Badge settings
- **SystemSettings** - System configurations

### Initial Data
- ✅ 8 Subjects seeded
- ✅ 9 Topics seeded
- ✅ 5 Badge configurations
- ✅ 10 System settings
- ✅ 1 Admin user

## 🧪 Testing

### Postman Collection
Import the Postman collection for API testing:
```
postman/BCS_Exam_Platform.postman_collection.json
postman/BCS_Exam_Platform.postman_environment.json
```

### Swagger UI
Access interactive API documentation:
```
https://localhost:2780/swagger
```

## 📖 Documentation

- **[RUN_BACKEND_NOW.txt](RUN_BACKEND_NOW.txt)** - Quick start guide
- **[AUTO_DATABASE_SETUP_SUCCESS.md](AUTO_DATABASE_SETUP_SUCCESS.md)** - Database setup
- **[PROJECT_STATUS_SUMMARY.md](PROJECT_STATUS_SUMMARY.md)** - Complete project status
- **[BACKEND_REQUIREMENTS_CHECKLIST.md](BACKEND_REQUIREMENTS_CHECKLIST.md)** - Feature checklist
- **[POSTMAN_TESTING_GUIDE.md](POSTMAN_TESTING_GUIDE.md)** - API testing guide
- **[FLUTTER_INSTALLATION_GUIDE.md](FLUTTER_INSTALLATION_GUIDE.md)** - Flutter setup

## 🎯 Current Status

| Component | Progress | Status |
|-----------|----------|--------|
| Backend API | 90% | ✅ Working |
| Database | 100% | ✅ Working |
| Mobile App Code | 100% | ✅ Complete |
| Admin Features | 10% | ⏳ Pending |
| Documentation | 100% | ✅ Complete |

## 🛠️ Technology Stack

### Backend
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt for password hashing
- Swagger/OpenAPI

### Mobile
- Flutter
- BLoC Pattern
- HTTP Client
- Shared Preferences
- Material Design

## 🌍 Bilingual Support

All content supports both languages:
- Questions (Bangla/English)
- Options (Bangla/English)
- Explanations (Bangla/English)
- Subject names (Bangla/English)
- Topic names (Bangla/English)
- UI text (Bangla/English)

## 📱 Mobile App Features

### Implemented Screens
- Splash Screen
- Login/Register
- Home Screen
- Exam List
- Exam Details
- Exam Screen (200 questions)
- Results Screen
- Review Screen
- Profile Screen

### Features
- Timer countdown
- Question navigation
- Mark for review
- Progress tracking
- Offline storage
- Theme support

## 🔒 Security Features

- JWT token authentication
- Password hashing with BCrypt
- HTTPS enabled
- CORS configured
- SQL injection protection (EF Core)
- Input validation

## 🚧 Roadmap

### Phase 1 (Current)
- ✅ Core exam flow
- ✅ User authentication
- ✅ Results and review
- ✅ Bilingual support

### Phase 2 (Next)
- ⏳ Admin panel APIs
- ⏳ Question management
- ⏳ User management
- ⏳ Analytics dashboard

### Phase 3 (Future)
- ⏳ AI question generation
- ⏳ Performance analytics
- ⏳ Notification system
- ⏳ Payment integration
- ⏳ Practice section (1000 questions)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Saidul Islam Rajib**
- GitHub: [@saidul-islam-rajib](https://github.com/saidul-islam-rajib)

## 🙏 Acknowledgments

- BCS exam preparation community
- ASP.NET Core team
- Flutter team
- All contributors

## 📞 Support

For support, email or create an issue in the repository.

## 🎉 Success Metrics

- ✅ 0 build errors
- ✅ Clean architecture
- ✅ Production-ready code
- ✅ Comprehensive documentation
- ✅ Auto database setup
- ✅ Complete API coverage

---

**Made with ❤️ for BCS aspirants**

*Last Updated: February 23, 2026*
