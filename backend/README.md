# BCS Exam Platform - Backend API

ASP.NET Core Web API for BCS Preliminary Exam Platform

## Prerequisites

- .NET 8.0 SDK
- SQL Server 2019 or later
- Visual Studio 2022 or VS Code

## Project Structure

```
backend/
├── BcsExamPlatform.API/          # Web API project
│   ├── Controllers/               # API controllers
│   ├── DTOs/                      # Data Transfer Objects
│   └── Program.cs                 # Application entry point
├── BcsExamPlatform.Core/          # Domain entities
│   └── Entities/                  # Domain models
└── BcsExamPlatform.Infrastructure/ # Data access layer
    └── Data/                      # DbContext and configurations
```

## Setup Instructions

### 1. Install EF Core Tools

```bash
dotnet tool install --global dotnet-ef
```

### 2. Update Connection String

Edit `BcsExamPlatform.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=BcsExamPlatform;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 3. Create and Apply Migrations

```bash
cd backend

# Create initial migration
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Apply migration (creates database and seeds data)
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

### 4. Run the API

```bash
cd BcsExamPlatform.API
dotnet run
```

The API will start at: `https://localhost:5001` or `http://localhost:5000`

### 5. Access Swagger UI

Open browser: `https://localhost:5001/swagger`

## Default Admin Credentials

- **Email**: `admin@bcsexam.com`
- **Password**: `Admin@123`

## Seeded Data

The database is automatically seeded with:
- 8 Subjects (Bangla, English, Math, etc.)
- 9 Topics (across subjects)
- 5 Badge configurations (Fail, Pass, Good, Excellent, Pro)
- 10 System settings
- 2 Donation payment methods
- 1 Admin user

## API Endpoints

### Authentication

- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user

### Exams

- `GET /api/exam/list` - Get all published exams
- `GET /api/exam/{examId}` - Get exam details
- `POST /api/exam/start` - Start an exam
- `GET /api/exam/questions/{attemptId}` - Get exam questions
- `POST /api/exam/submit-answer` - Submit answer for a question
- `POST /api/exam/submit` - Submit complete exam
- `GET /api/exam/result/{attemptId}` - Get exam result
- `GET /api/exam/review/{attemptId}` - Get question-wise review

## Database Migrations (Alternative to SQL Scripts)

If you prefer Entity Framework migrations:

```bash
cd BcsExamPlatform.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../BcsExamPlatform.API
dotnet ef database update --startup-project ../BcsExamPlatform.API
```

**Note**: The application uses Code-First approach with automatic seeding via `DbInitializer`. No manual SQL scripts needed!

## Configuration

### JWT Settings

Update in `appsettings.json`:

```json
"JwtSettings": {
  "SecretKey": "YOUR_SECRET_KEY_HERE",
  "Issuer": "BcsExamPlatform",
  "Audience": "BcsExamPlatformUsers",
  "ExpiryInMinutes": 1440
}
```

## Testing

Use Swagger UI or tools like Postman to test the API endpoints.

### Sample Register Request

```json
{
  "email": "user@example.com",
  "password": "Password123!",
  "fullName": "John Doe",
  "phoneNumber": "01712345678",
  "preferredLanguage": "Bangla"
}
```

### Sample Login Request

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

## Next Steps

1. ✅ Backend API is ready
2. ⏳ Build Flutter mobile app
3. ⏳ Integrate AI services
4. ⏳ Add admin panel features
5. ⏳ Deploy to production

## Troubleshooting

### Connection Issues

- Verify SQL Server is running
- Check connection string
- Ensure database exists

### Build Errors

```bash
dotnet clean
dotnet restore
dotnet build
```

## License

Proprietary - All rights reserved
