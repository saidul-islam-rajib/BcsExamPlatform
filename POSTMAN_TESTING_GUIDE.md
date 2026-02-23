# 🚀 Complete Postman Testing Guide

Step-by-step guide to test all BCS Exam Platform APIs using Postman.

---

## 📦 What You Get

1. **Postman Collection** - All API endpoints with sample data
2. **Postman Environment** - Pre-configured variables
3. **SQL Script** - Sample exam with 200 questions
4. **Complete Documentation** - This guide

---

## 🎯 Quick Setup (5 Minutes)

### Step 1: Setup Backend

```bash
# Navigate to backend
cd backend

# Create database migration
dotnet ef migrations add InitialCreate --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Apply migration (creates database + seeds data)
dotnet ef database update --project BcsExamPlatform.Infrastructure --startup-project BcsExamPlatform.API

# Run API
cd BcsExamPlatform.API
dotnet run
```

✅ API running at: `https://localhost:5001`

### Step 2: Create Sample Exam

Open SQL Server Management Studio and run:
```sql
-- File: database/sample-exam-with-questions.sql
-- This creates 1 exam with 200 questions
```

✅ Sample exam created with 200 questions

### Step 3: Import Postman Collection

1. Open Postman
2. Click **Import**
3. Select `postman/BCS_Exam_Platform.postman_collection.json`
4. Select `postman/BCS_Exam_Platform.postman_environment.json`
5. Choose environment: "BCS Exam Platform - Local"

✅ Ready to test!

---

## 🧪 Complete Testing Flow

### Test 1: Register New User

**Endpoint:** `POST /api/auth/register`

**Request:**
```json
{
  "email": "student@example.com",
  "password": "Student@123",
  "fullName": "John Doe",
  "phoneNumber": "01712345678",
  "preferredLanguage": "Bangla"
}
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Registration successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": "guid",
    "email": "student@example.com",
    "fullName": "John Doe",
    "phoneNumber": "01712345678",
    "isGuest": false,
    "preferredLanguage": "Bangla",
    "profileImageUrl": null
  }
}
```

✅ **Auto-saved:** `auth_token`, `user_id`

---

### Test 2: Login User

**Endpoint:** `POST /api/auth/login`

**Request:**
```json
{
  "email": "student@example.com",
  "password": "Student@123"
}
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": "guid",
    "email": "student@example.com",
    "fullName": "John Doe",
    ...
  }
}
```

✅ **Auto-saved:** `auth_token`, `user_id`

---

### Test 3: Get Exam List

**Endpoint:** `GET /api/exam/list?language=Bangla`

**Headers:** None required (public endpoint)

**Expected Response (200 OK):**
```json
[
  {
    "examId": "guid",
    "examName": "BCS ৪৫তম প্রিলিমিনারি মডেল টেস্ট",
    "examDate": "2026-03-01T00:00:00",
    "totalQuestions": 200,
    "durationMinutes": 120,
    "totalMarks": 200.0,
    "languageMode": "Bilingual",
    "isPaid": false,
    "examFee": null,
    "allowGuestUsers": true,
    "isPublished": true
  }
]
```

📝 **Copy the `examId`** and set it in environment variables

---

### Test 4: Get Exam Details

**Endpoint:** `GET /api/exam/{examId}?language=Bangla`

**Expected Response (200 OK):**
```json
{
  "examId": "guid",
  "examName": "BCS ৪৫তম প্রিলিমিনারি মডেল টেস্ট",
  "examDate": "2026-03-01T00:00:00",
  "totalQuestions": 200,
  "durationMinutes": 120,
  "totalMarks": 200.0,
  "languageMode": "Bilingual",
  "isPaid": false,
  "examFee": null,
  "allowGuestUsers": true,
  "subjectDistributions": [
    {
      "subjectName": "বাংলা",
      "totalQuestions": 25,
      "easyQuestions": 10,
      "intermediateQuestions": 10,
      "hardQuestions": 5
    },
    {
      "subjectName": "ইংরেজি",
      "totalQuestions": 25,
      "easyQuestions": 10,
      "intermediateQuestions": 10,
      "hardQuestions": 5
    },
    ... (8 subjects total)
  ]
}
```

---

### Test 5: Start Exam (Registered User)

**Endpoint:** `POST /api/exam/start`

**Headers:** 
```
Authorization: Bearer {{auth_token}}
```

**Request:**
```json
{
  "examId": "{{exam_id}}",
  "selectedLanguage": "Bangla"
}
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Exam started successfully",
  "attemptId": "guid",
  "startedAt": "2026-02-23T10:00:00Z"
}
```

✅ **Auto-saved:** `attempt_id`

---

### Test 5b: Start Exam (Guest User)

**Endpoint:** `POST /api/exam/start`

**Headers:** None

**Request:**
```json
{
  "examId": "{{exam_id}}",
  "selectedLanguage": "Bangla",
  "guestEmail": "guest@example.com"
}
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Exam started successfully",
  "attemptId": "guid",
  "startedAt": "2026-02-23T10:00:00Z"
}
```

---

### Test 6: Get Exam Questions

**Endpoint:** `GET /api/exam/questions/{attemptId}`

**Headers:** 
```
Authorization: Bearer {{auth_token}}
```

**Expected Response (200 OK):**
```json
[
  {
    "questionId": "guid",
    "questionNumber": 1,
    "questionText": "বাংলা ভাষার উৎপত্তি কোন ভাষা থেকে? (প্রশ্ন 1)",
    "questionImageUrl": null,
    "hasMathContent": false,
    "options": [
      {
        "optionId": "guid-1",
        "optionText": "সংস্কৃত",
        "optionOrder": 1
      },
      {
        "optionId": "guid-2",
        "optionText": "পালি",
        "optionOrder": 2
      },
      {
        "optionId": "guid-3",
        "optionText": "প্রাকৃত",
        "optionOrder": 3
      },
      {
        "optionId": "guid-4",
        "optionText": "অপভ্রংশ",
        "optionOrder": 4
      }
    ],
    "subjectName": "বাংলা",
    "topicName": "ব্যাকরণ",
    "difficultyLevel": "Easy",
    "marks": 1.0
  },
  ... (200 questions total)
]
```

📝 **Copy `questionId` and `optionId`** for submitting answers

---

### Test 7: Submit Answer

**Endpoint:** `POST /api/exam/submit-answer`

**Headers:** 
```
Authorization: Bearer {{auth_token}}
```

**Request:**
```json
{
  "attemptId": "{{attempt_id}}",
  "questionId": "{{question_id}}",
  "selectedOptionId": "{{option_id}}",
  "timeSpentSeconds": 45,
  "isMarkedForReview": false
}
```

**Expected Response (200 OK):**
```json
{
  "message": "Answer saved successfully"
}
```

💡 **Repeat this for all 200 questions** (or use a script)

---

### Test 8: Submit Exam

**Endpoint:** `POST /api/exam/submit`

**Headers:** 
```
Authorization: Bearer {{auth_token}}
```

**Request:**
```json
{
  "attemptId": "{{attempt_id}}"
}
```

**Expected Response (200 OK):**
```json
{
  "attemptId": "guid",
  "examName": "BCS ৪৫তম প্রিলিমিনারি মডেল টেস্ট",
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

🎉 **Exam completed!**

---

### Test 9: Get Exam Result

**Endpoint:** `GET /api/exam/result/{attemptId}`

**Headers:** 
```
Authorization: Bearer {{auth_token}}
```

**Expected Response (200 OK):**
```json
{
  "attemptId": "guid",
  "examName": "BCS ৪৫তম প্রিলিমিনারি মডেল টেস্ট",
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

### Test 10: Get Question Review

**Endpoint:** `GET /api/exam/review/{attemptId}`

**Headers:** 
```
Authorization: Bearer {{auth_token}}
```

**Expected Response (200 OK):**
```json
[
  {
    "questionId": "guid",
    "questionNumber": 1,
    "questionText": "বাংলা ভাষার উৎপত্তি কোন ভাষা থেকে? (প্রশ্ন 1)",
    "questionImageUrl": null,
    "options": [
      {
        "optionId": "guid-1",
        "optionText": "সংস্কৃত",
        "optionOrder": 1,
        "isCorrect": true
      },
      {
        "optionId": "guid-2",
        "optionText": "পালি",
        "optionOrder": 2,
        "isCorrect": false
      },
      {
        "optionId": "guid-3",
        "optionText": "প্রাকৃত",
        "optionOrder": 3,
        "isCorrect": false
      },
      {
        "optionId": "guid-4",
        "optionText": "অপভ্রংশ",
        "optionOrder": 4,
        "isCorrect": false
      }
    ],
    "userSelectedOptionId": "guid-1",
    "correctOptionId": "guid-1",
    "isCorrect": true,
    "marksObtained": 1.0,
    "timeSpentSeconds": 45,
    "explanation": "বাংলা ভাষার উৎপত্তি সংস্কৃত ভাষা থেকে। এটি ইন্দো-আর্য ভাষা পরিবারের অন্তর্গত।",
    "subjectName": "বাংলা",
    "topicName": "ব্যাকরণ",
    "difficultyLevel": "Easy",
    "sourceReference": "BCS 40th Preliminary"
  },
  ... (200 questions with review)
]
```

---

## 📊 Badge System

Based on marks obtained:

| Marks | Badge | Color |
|-------|-------|-------|
| 0-99 | Fail | Red (#FF0000) |
| 100-119 | Pass | Orange (#FFA500) |
| 120-149 | Good | Yellow (#FFFF00) |
| 150-179 | Excellent | Green (#00FF00) |
| 180-200 | Pro | Blue (#0000FF) |

---

## 🎯 Testing Scenarios

### Scenario 1: Perfect Score
- Answer all 200 questions correctly
- Expected: 200 marks, Rank 1, "Pro" badge

### Scenario 2: Pass Mark
- Answer 100 questions correctly
- Expected: 100 marks, "Pass" badge

### Scenario 3: Fail
- Answer 50 questions correctly
- Expected: 50 marks, "Fail" badge

### Scenario 4: Guest User
- Start exam without login
- Provide guest email
- Complete exam
- View results

---

## 🔧 Troubleshooting

### Issue: "Exam not found"
**Solution:** Run the SQL script to create sample exam

### Issue: "Already attempted this exam"
**Solution:** Each user can only attempt once. Use different email or create new exam

### Issue: "Unauthorized"
**Solution:** Login first to get token. Check if token is saved in environment

### Issue: "SSL Certificate Error"
**Solution:** In Postman Settings → Turn OFF "SSL certificate verification"

---

## 📝 Files Location

```
bcs-exam-platform/
├── postman/
│   ├── BCS_Exam_Platform.postman_collection.json
│   ├── BCS_Exam_Platform.postman_environment.json
│   └── README.md
├── database/
│   └── sample-exam-with-questions.sql
└── POSTMAN_TESTING_GUIDE.md (this file)
```

---

## ✅ Checklist

Before testing:
- [ ] Backend API is running
- [ ] Database migrations applied
- [ ] Sample exam created (SQL script)
- [ ] Postman collection imported
- [ ] Environment imported and selected
- [ ] SSL verification disabled (if needed)

---

## 🎉 Success!

You now have:
- ✅ Complete Postman collection
- ✅ Sample exam with 200 questions
- ✅ All endpoints tested
- ✅ Ready for mobile app integration

---

**Happy Testing! 🚀**

*Last Updated: February 23, 2026*
