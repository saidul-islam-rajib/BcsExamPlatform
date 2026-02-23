# Why Code-First Approach with EF Core?

## The Problem with SQL Scripts

### ❌ Manual SQL Scripts Approach

```
Developer A: Creates schema.sql
Developer B: Runs it locally
Developer C: Modifies a table manually
Developer D: Doesn't know about the change
Production: Different schema than development
Result: 💥 Chaos!
```

**Issues:**
- ❌ No version control for database changes
- ❌ Manual synchronization between team members
- ❌ Hard to track what changed and when
- ❌ Difficult to rollback changes
- ❌ Production vs Development drift
- ❌ No type safety between code and database
- ❌ Manual seed data management

## ✅ Code-First with EF Core

### How It Works

```
1. Define entities in C# (User.cs, Exam.cs, etc.)
2. EF Core generates migration files
3. Migration files are committed to Git
4. Everyone runs: dotnet ef database update
5. Everyone has the same database!
```

### Advantages

#### 1. **Version Control** 📝
```bash
git log --oneline
a1b2c3d Add ExamAttempt table
d4e5f6g Add TabSwitchCount column
g7h8i9j Create initial schema
```

Every database change is tracked in Git!

#### 2. **Team Collaboration** 👥
```bash
# Developer A creates migration
dotnet ef migrations add AddNewFeature

# Commits to Git
git add .
git commit -m "Add new feature"
git push

# Developer B pulls changes
git pull

# Applies migration
dotnet ef database update

# Done! Same database structure
```

#### 3. **Type Safety** 🛡️
```csharp
// If you change this:
public class User {
    public string Email { get; set; }  // Changed from Email to EmailAddress
}

// Compiler error immediately!
var email = user.Email;  // ❌ Compile error - property doesn't exist

// With SQL scripts: Runtime error in production! 💥
```

#### 4. **Easy Rollback** ⏮️
```bash
# Oops, last migration broke something!
dotnet ef database update PreviousMigration

# Fixed! Back to working state
```

#### 5. **Automatic Seed Data** 🌱
```csharp
// DbInitializer.cs runs automatically
await DbInitializer.Initialize(context);

// Seeds:
// - 8 Subjects
// - 9 Topics  
// - 5 Badges
// - 10 Settings
// - 1 Admin user

// No manual SQL scripts to run!
```

#### 6. **CI/CD Ready** 🚀
```yaml
# GitHub Actions / Azure DevOps
- name: Apply Migrations
  run: dotnet ef database update
  
# Automatic deployment!
```

#### 7. **Cross-Platform** 🌍
```bash
# Works on:
✅ Windows
✅ Linux
✅ macOS
✅ Docker containers
✅ Cloud (Azure, AWS, GCP)
```

#### 8. **Idempotent Scripts** 🔄
```bash
# Generate SQL script that can run multiple times safely
dotnet ef migrations script --idempotent

# Safe to run in production multiple times
```

## Real-World Example

### Scenario: Add "PhoneVerified" column to Users table

#### ❌ SQL Scripts Way:
```sql
-- Developer creates: migration_001.sql
ALTER TABLE Users ADD PhoneVerified BIT NOT NULL DEFAULT 0;

-- Problems:
-- 1. Did everyone run this?
-- 2. Did production run this?
-- 3. What if someone already added it manually?
-- 4. How to rollback?
-- 5. Code still references old schema!
```

#### ✅ Code-First Way:
```csharp
// 1. Update entity
public class User {
    public bool PhoneVerified { get; set; }  // Add this
}

// 2. Create migration
dotnet ef migrations add AddPhoneVerified

// 3. Review generated migration
public partial class AddPhoneVerified : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "PhoneVerified",
            table: "Users",
            nullable: false,
            defaultValue: false);
    }
    
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PhoneVerified",
            table: "Users");
    }
}

// 4. Apply migration
dotnet ef database update

// 5. Commit to Git
git add .
git commit -m "Add phone verification"

// Benefits:
// ✅ Everyone gets the change via Git
// ✅ Type-safe in code immediately
// ✅ Can rollback with one command
// ✅ Tracked in version control
// ✅ CI/CD applies automatically
```

## Migration Workflow

### Development
```bash
# 1. Make changes to entities
# Edit: User.cs, Exam.cs, etc.

# 2. Create migration
dotnet ef migrations add DescriptiveNameHere --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# 3. Review migration file
# Check: Migrations/20240223_DescriptiveNameHere.cs

# 4. Apply to local database
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# 5. Test changes

# 6. Commit to Git
git add .
git commit -m "Add new feature"
git push
```

### Team Member
```bash
# 1. Pull changes
git pull

# 2. Apply migrations
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Done! Database is up to date
```

### Production
```bash
# Option 1: Direct migration (staging/test environments)
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Option 2: Generate SQL script (production - safer)
dotnet ef migrations script --idempotent --output deploy.sql

# Review deploy.sql
# Run deploy.sql on production database
```

## Common Migrations

### Add New Table
```csharp
// 1. Create entity
public class Feedback {
    public Guid FeedbackId { get; set; }
    public string Message { get; set; }
}

// 2. Add to DbContext
public DbSet<Feedback> Feedbacks { get; set; }

// 3. Create migration
dotnet ef migrations add AddFeedbackTable
```

### Add Column
```csharp
// 1. Add property to entity
public class User {
    public DateTime? EmailVerifiedAt { get; set; }  // New
}

// 2. Create migration
dotnet ef migrations add AddEmailVerifiedAt
```

### Rename Column
```csharp
// 1. Rename property
public class User {
    public string FullName { get; set; }  // Was: Name
}

// 2. Create migration
dotnet ef migrations add RenameNameToFullName

// 3. Edit migration to preserve data
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "Name",
        table: "Users",
        newName: "FullName");
}
```

### Add Index
```csharp
// In DbContext configuration
modelBuilder.Entity<User>()
    .HasIndex(u => u.Email)
    .IsUnique();

// Create migration
dotnet ef migrations add AddEmailIndex
```

## Best Practices

### ✅ DO:
- Create descriptive migration names: `AddUserPhoneVerification`
- Review generated migrations before applying
- Test migrations on development database first
- Commit migrations to Git
- Use `--idempotent` for production scripts
- Keep migrations small and focused

### ❌ DON'T:
- Modify applied migrations (create new one instead)
- Delete migration files
- Edit database manually (use migrations)
- Skip migrations in version control
- Apply untested migrations to production

## Comparison Table

| Feature | SQL Scripts | Code-First EF Core |
|---------|-------------|-------------------|
| Version Control | ❌ Manual | ✅ Automatic |
| Type Safety | ❌ No | ✅ Yes |
| Team Sync | ❌ Manual | ✅ Automatic |
| Rollback | ❌ Manual | ✅ One command |
| CI/CD | ❌ Complex | ✅ Simple |
| Seed Data | ❌ Manual | ✅ Automatic |
| Cross-Platform | ⚠️ Limited | ✅ Full |
| Learning Curve | ✅ Easy | ⚠️ Medium |
| Production Deploy | ⚠️ Risky | ✅ Safe |

## Conclusion

**Code-First with EF Core is the industry standard for modern .NET applications.**

### Why?
- ✅ Better team collaboration
- ✅ Safer deployments
- ✅ Easier maintenance
- ✅ Type-safe development
- ✅ Version controlled
- ✅ CI/CD friendly

### When to use SQL Scripts?
- Legacy systems with existing databases
- Complex stored procedures
- Database-first approach required
- Team prefers SQL over C#

**For this project: Code-First is the right choice! 🎯**

## Resources

- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Migrations Overview](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Code-First Conventions](https://docs.microsoft.com/en-us/ef/core/modeling/)
