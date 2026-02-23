# Postman Collection - BCS Exam Platform API

Complete API testing collection with sample data for all endpoints.

---

## 📦 Files Included

1. **BCS_Exam_Platform.postman_collection.json** - API collection with all endpoints
2. **BCS_Exam_Platform.postman_environment.json** - Environment variables
3. **README.md** - This guide

---

## 🚀 Quick Start

### Step 1: Import Collection

1. Open Postman
2. Click **Import** button
3. Select `BCS_Exam_Platform.postman_collection.json`
4. Collection will appear in left sidebar

### Step 2: Import Environment

1. Click **Environments** (left sidebar)
2. Click **Import**
3. Select `BCS_Exam_Platform.postman_environment.json`
4. Select "BCS Exam Platform - Local" from environment dropdown (top right)

### Step 3: Start Backend API

```bash
cd backend/BcsExamPlatform.API
dotnet run
```

API should be running at `https://localhost:5001`

### Step 4: Test APIs

Follow the testing workflow below.

---

## 🧪 Testing Workflow

### Phase 1: Authentication

#### 1. Register New User
```
POST /api/auth/register

Body:
{
  "email": "student@example.com",
  "password": "Student@123",
  "fullName": "John Doe",
  "phoneNumber": "01712345678",
  "preferredLanguage": "Bangla"
}

✅ Auto-saves: auth_token, user_id
```

#### 2. Login User
```
POST /api/auth/login

Body:
{
  "email": "student@example.com",
  "password": "Student@123"
}

✅ Auto-saves: auth_token, user_id
```

#### 3. Login as Admin (Default)
```
POST /api/auth/login

Body:
{
  "email": "admin@bcsexam.com",
  "password": "Admin@123"
}

✅ Auto-saves: auth_token, user_id
```

---

### Phase 2: Browse Exams

#### 4. Get Exam List (Bangla)
```
GET /api/exam/list?language=Bangla

Response:
[
  {
    "examId": "guid",
    "examName": "BCS 45th Preliminary",
    "examDate": "2026-03-01T00:00:00",
    "totalQuestions": 200,
    "durationMinutes": 120,
    ...
  }
]

📝 Copy examId for next steps
```

#### 5. Get Exam Details
```
GET /api/exam/{examId}?language=Bangla

Response:
{
  "examId": "guid",
  "examName": "BCS 45th Preliminary",
  "subjectDistributions": [
    {
      "subjectName": "বাংলা",
      "totalQuestions": 25,
      "easyQuestions": 10,
      ...
    }
  ]
}
```

---

### Phase 3: Take Exam

#### 6. Start Exam (Registered User)
```
POST /api/exam/start
Authorization: Bearer {{auth_token}}

Body:
{
  "examId": "{{exam_id}}",
  "selectedLanguage": "Bangla"
}

Response:
{
  "success": true,
  "attemptId": "guid",
  "startedAt": "2026-02-23T10:00:00"
}

✅ Auto-saves: attempt_id
```

#### 6b. Start Exam (Guest User)
```
POST /api/exam/start

Body:
{
  "examId": "{{exam_id}}",
  "selectedLanguage": "Bangla",
  "guestEmail": "guest@example.com"
}

✅ Auto-saves: attempt_id
```

#### 7. Get Exam Questions
```
GET /api/exam/questions/{{attempt_id}}
Authorization: Bearer {{auth_token}}

Response:
[
  {
    "questionId": "guid",
    "questionNumber": 1,
    "questionText": "বাংলা ভাষার উৎপত্তি কোন ভাষা থেকে?",
    "options": [
      {
        "optionId": "guid",
        "optionText": "সংস্কৃত",
        "optionOrder": 1
      },
      ...
    ],
    "subjectName": "বাংলা",
    "topicName": "ব্যাকরণ",
    "difficultyLevel": "Easy",
    "marks": 1.0
  },
  ... (200 questions total)
]

📝 Copy questionId and optionId for answers
```

#### 8. Submit Answer
```
POST /api/exam/submit-answer
Authorization: Bearer {{auth_token}}

Body:
{
  "attemptId": "{{attempt_id}}",
  "questionId": "{{question_id}}",
  "selectedOptionId": "{{option_id}}",
  "timeSpentSeconds": 45,
  "isMarkedForReview": false
}

Response:
{
  "message": "Answer saved successfully"
}

💡 Repeat for all 200 questions
```

#### 9. Submit Exam
```
POST /api/exam/submit
Authorization: Bearer {{auth_token}}

Body:
{
  "attemptId": "{{attempt_id}}"
}

Response:
{
  "attemptId": "guid",
  "examName": "BCS 45th Preliminary",
  "totalMarksObtained": 145.0,
  "totalMarks": 200.0,
  "totalCorrectAnswers": 145,
  "totalWrongAnswers": 50,
  "totalUnanswered": 5,
  "accuracyPercentage": 74.36,
  "totalTimeSpentSeconds": 7200,
  "rank": 1,
  "badgeName": "Good",
  "badgeColor": "#FFFF00",
  "isPassed": true
}
```

---

### Phase 4: View Results

#### 10. Get Exam Result
```
GET /api/exam/result/{{attempt_id}}
Authorization: Bearer {{auth_token}}

Response:
{
  "attemptId": "guid",
  "examName": "BCS 45th Preliminary",
  "totalMarksObtained": 145.0,
  "totalMarks": 200.0,
  "rank": 1,
  "badgeName": "Good",
  "isPassed": true,
  ...
}
```

#### 11. Get Question Review
```
GET /api/exam/review/{{attempt_id}}
Authorization: Bearer {{auth_token}}

Response:
[
  {
    "questionId": "guid",
    "questionNumber": 1,
    "questionText": "বাংলা ভাষার উৎপত্তি কোন ভাষা থেকে?",
    "options": [
      {
        "optionId": "guid",
        "optionText": "সংস্কৃত",
        "optionOrder": 1,
        "isCorrect": true
      },
      ...
    ],
    "userSelectedOptionId": "guid",
    "correctOptionId": "guid",
    "isCorrect": true,
    "marksObtained": 1.0,
    "timeSpentSeconds": 45,
    "explanation": "বাংলা ভাষার উৎপত্তি সংস্কৃত ভাষা থেকে।",
    "subjectName": "বাংলা",
    "topicName": "ব্যাকরণ",
    "difficultyLevel": "Easy",
    "sourceReference": "BCS 40th Preliminary"
  },
  ... (200 questions with review)
]
```

---

## 🔑 Environment Variables

The collection uses these variables (auto-managed):

| Variable | Description | Auto-Set |
|----------|-------------|----------|
| `base_url` | API base URL | Manual |
| `auth_token` | JWT token | ✅ Yes |
| `user_id` | Current user ID | ✅ Yes |
| `exam_id` | Current exam ID | Manual |
| `attempt_id` | Current attempt ID | ✅ Yes |
| `question_id` | Current question ID | Manual |
| `option_id` | Current option ID | Manual |

### How to Set Manual Variables

1. After getting exam list, copy an `examId`
2. In Postman, click **Environments** → **BCS Exam Platform - Local**
3. Set `exam_id` value
4. Similarly set `question_id` and `option_id` when needed

---

## 📝 Sample Data Reference

### Valid Email Formats
```
student@example.com
test@test.com
user123@gmail.com
```

### Valid Passwords
```
Student@123
Test@123
Admin@123
```

### Language Options
```
"Bangla"
"English"
```

### Difficulty Levels
```
"Easy"
"Intermediate"
"Hard"
```

---

## 🎯 Complete Test Scenario

### Scenario: Student Takes Exam

```
1. Register User
   → POST /api/auth/register
   → Get auth_token

2. Browse Exams
   → GET /api/exam/list
   → Copy exam_id

3. View Exam Details
   → GET /api/exam/{exam_id}
   → Check subject distribution

4. Start Exam
   → POST /api/exam/start
   → Get attempt_id

5. Get Questions
   → GET /api/exam/questions/{attempt_id}
   → Get 200 questions

6. Answer Questions (Loop 200 times)
   → POST /api/exam/submit-answer
   → Submit each answer

7. Submit Exam
   → POST /api/exam/submit
   → Get result with rank and badge

8. View Result
   → GET /api/exam/result/{attempt_id}
   → See detailed result

9. Review Answers
   → GET /api/exam/review/{attempt_id}
   → See all questions with correct answers
```

---

## 🔧 Troubleshooting

### Issue: "Cannot connect to API"

**Solution:**
1. Check if backend is running: `dotnet run`
2. Verify URL in environment: `https://localhost:5001`
3. Try HTTP instead: `http://localhost:5000`

### Issue: "Unauthorized" (401)

**Solution:**
1. Login first to get token
2. Check if token is saved in environment
3. Verify Authorization header: `Bearer {{auth_token}}`

### Issue: "Exam not found"

**Solution:**
1. Run database migration first
2. Create exam manually via SQL
3. Or wait for admin APIs to be built

### Issue: "Already attempted this exam"

**Solution:**
- Each user can only attempt an exam once
- Use different email or create new exam

### Issue: SSL Certificate Error

**Solution:**
1. In Postman: Settings → General
2. Turn OFF "SSL certificate verification"
3. Or use HTTP: `http://localhost:5000`

---

## 📊 Expected Response Codes

| Code | Meaning | When |
|------|---------|------|
| 200 | Success | Request successful |
| 400 | Bad Request | Invalid data |
| 401 | Unauthorized | Missing/invalid token |
| 404 | Not Found | Resource doesn't exist |
| 500 | Server Error | Backend error |

---

## 🎓 Tips for Testing

### 1. Use Collection Runner
- Select collection
- Click **Run**
- Test all endpoints automatically

### 2. Use Pre-request Scripts
- Already included in collection
- Auto-saves tokens and IDs

### 3. Use Tests Tab
- View response data
- Check status codes
- Validate responses

### 4. Export Results
- After running collection
- Export results as JSON/HTML
- Share with team

---

## 📚 Additional Resources

- **Swagger UI**: https://localhost:5001/swagger
- **Backend Setup**: See `backend/SETUP.md`
- **API Documentation**: See `backend/README.md`

---

## 🆘 Need Help?

### Common Questions

**Q: How do I create an exam?**
A: Admin APIs not yet built. Create manually via SQL or wait for admin panel.

**Q: Can I test without database?**
A: No, you need to run migrations first to create database.

**Q: How do I add questions?**
A: Use SQL scripts or wait for question management APIs.

**Q: Can I test as guest?**
A: Yes! Use "Start Exam (Guest User)" endpoint with email.

---

## ✅ Checklist

Before testing:
- [ ] Backend API is running
- [ ] Database is created (migrations applied)
- [ ] Postman collection imported
- [ ] Environment imported and selected
- [ ] At least one exam exists in database
- [ ] Exam has 200 questions

---

**Happy Testing! 🚀**

*Last Updated: February 23, 2026*
