# Backend Setup Guide - EF Core Code-First Approach

## Prerequisites

- .NET 8.0 SDK
- SQL Server 2019+ (or SQL Server Express)
- Visual Studio 2022 / VS Code / Rider

## Step-by-Step Setup

### 1. Install EF Core Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

Or update if already installed:

```bash
dotnet tool update --global dotnet-ef
```

### 2. Update Connection String

Edit `BcsExamPlatform.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BcsExamPlatform;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

**For SQL Server Express:**
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BcsExamPlatform;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

**For SQL Server with username/password:**
```json
"DefaultConnection": "Server=localhost;Database=BcsExamPlatform;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### 3. Create Initial Migration

Navigate to the solution directory:

```bash
cd backend
```

Create the initial migration:

```bash
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

This will create a `Migrations` folder in the Infrastructure project with the migration files.

### 4. Apply Migration to Database

```bash
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

This will:
- Create the database if it doesn't exist
- Create all tables with proper relationships
- Apply all indexes
- Run the seed data automatically (via DbInitializer)

### 5. Verify Database

Open SQL Server Management Studio or Azure Data Studio and verify:

- Database `BcsExamPlatform` is created
- All tables are present:
  - Users
  - Subjects (8 subjects seeded)
  - Topics (9 topics seeded)
  - Questions
  - Exams
  - ExamAttempts
  - BadgeConfigurations (5 badges seeded)
  - SystemSettings (10 settings seeded)
  - DonationSettings (2 payment methods seeded)
  - And more...

### 6. Run the API

```bash
cd BcsExamPlatform.API
dotnet run
```

The API will start at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

### 7. Test with Swagger

Open browser: `https://localhost:5001/swagger`

Test the endpoints:
1. Register a new user
2. Login
3. Get exam list

## Default Admin Credentials

After seeding, you can login with:
- **Email**: `admin@bcsexam.com`
- **Password**: `Admin@123`

## Common EF Core Commands

### Create a new migration
```bash
dotnet ef migrations add MigrationName --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

### Apply migrations
```bash
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

### Rollback to a specific migration
```bash
dotnet ef database update PreviousMigrationName --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

### Remove last migration (if not applied)
```bash
dotnet ef migrations remove --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

### Drop database
```bash
dotnet ef database drop --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

### Generate SQL script from migrations
```bash
dotnet ef migrations script --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API --output migration.sql
```

## Troubleshooting

### Error: "Build failed"
```bash
cd backend
dotnet clean
dotnet restore
dotnet build
```

### Error: "Unable to create an object of type 'ApplicationDbContext'"
Make sure you're running commands from the solution directory and specifying both projects.

### Error: "A network-related or instance-specific error"
- Verify SQL Server is running
- Check connection string
- Test connection with SQL Server Management Studio

### Error: "Login failed for user"
- Check SQL Server authentication mode
- Verify username/password in connection string
- Ensure user has proper permissions

## Database Reset (Development Only)

To completely reset the database:

```bash
# Drop database
dotnet ef database drop --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API --force

# Recreate and seed
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API
```

## Production Deployment

For production, generate SQL scripts instead of running migrations directly:

```bash
dotnet ef migrations script --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API --idempotent --output deploy.sql
```

Then run the SQL script on your production database.

## Next Steps

After successful setup:
1. ✅ Backend API is running
2. ✅ Database is created and seeded
3. ✅ Ready to connect Flutter app
4. ⏳ Update Flutter app's `.env` file with API URL
5. ⏳ Run Flutter app and test

## Benefits of Code-First Approach

✅ **Version Control**: Migrations are tracked in Git
✅ **Team Collaboration**: Everyone gets the same database structure
✅ **Easy Updates**: Just create new migration and apply
✅ **Rollback Support**: Can revert to previous versions
✅ **CI/CD Ready**: Automated deployment scripts
✅ **Cross-Platform**: Works on Windows, Linux, macOS
✅ **Type Safety**: Database schema matches C# models
