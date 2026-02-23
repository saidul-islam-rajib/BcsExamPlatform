# Contributing to BCS Exam Platform

First off, thank you for considering contributing to BCS Exam Platform! It's people like you that make this project better for everyone.

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues to avoid duplicates. When you create a bug report, include as many details as possible:

- **Use a clear and descriptive title**
- **Describe the exact steps to reproduce the problem**
- **Provide specific examples**
- **Describe the behavior you observed and what you expected**
- **Include screenshots if possible**
- **Include your environment details** (OS, .NET version, Flutter version, etc.)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, include:

- **Use a clear and descriptive title**
- **Provide a detailed description of the suggested enhancement**
- **Explain why this enhancement would be useful**
- **List any similar features in other applications**

### Pull Requests

1. **Fork the repository** and create your branch from `main`
2. **Make your changes** following our coding standards
3. **Test your changes** thoroughly
4. **Update documentation** if needed
5. **Write clear commit messages**
6. **Submit a pull request**

## Development Setup

### Backend Development

1. Install .NET 8.0 SDK
2. Install SQL Server
3. Clone the repository
4. Update connection string in `appsettings.json`
5. Run `dotnet restore`
6. Run `dotnet build`
7. Run `dotnet run`

### Mobile Development

1. Install Flutter SDK
2. Clone the repository
3. Run `flutter pub get`
4. Run `flutter run`

## Coding Standards

### C# / .NET

- Follow Microsoft's C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods small and focused
- Use async/await for asynchronous operations
- Follow SOLID principles

### Flutter / Dart

- Follow Dart style guide
- Use meaningful widget and variable names
- Keep widgets small and reusable
- Use BLoC pattern for state management
- Add comments for complex logic

### Git Commit Messages

- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit first line to 72 characters
- Reference issues and pull requests after the first line

Example:
```
Add user profile editing feature

- Add ProfileEditScreen
- Add update profile API endpoint
- Add validation for profile fields

Closes #123
```

## Project Structure

### Backend
```
backend/
├── BcsExamPlatform.API/          # Controllers, DTOs, Program.cs
├── BcsExamPlatform.Core/         # Entities, Interfaces
└── BcsExamPlatform.Infrastructure/ # DbContext, Repositories
```

### Mobile
```
mobile/lib/
├── core/                         # Services, Constants
├── data/                         # Models
├── blocs/                        # State Management
└── screens/                      # UI Screens
```

## Testing

### Backend Testing
- Write unit tests for business logic
- Write integration tests for API endpoints
- Ensure all tests pass before submitting PR

### Mobile Testing
- Write widget tests for UI components
- Write unit tests for BLoCs
- Test on both Android and iOS if possible

## Documentation

- Update README.md if you change functionality
- Add inline comments for complex logic
- Update API documentation in Swagger
- Create/update guides in documentation files

## Review Process

1. All submissions require review
2. Maintainers will review your PR
3. Address any feedback or requested changes
4. Once approved, your PR will be merged

## Recognition

Contributors will be recognized in:
- README.md contributors section
- Release notes
- Project documentation

## Questions?

Feel free to create an issue with the "question" label if you have any questions about contributing.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to BCS Exam Platform! 🎉
