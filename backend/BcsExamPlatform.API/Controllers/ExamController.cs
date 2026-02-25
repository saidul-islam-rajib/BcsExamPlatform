using BcsExamPlatform.API.DTOs;
using BcsExamPlatform.Core.Entities;
using BcsExamPlatform.Core.Enums;
using BcsExamPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BcsExamPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ExamController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<ExamListDTO>>> GetExams([FromQuery] string? language = "Bangla")
    {
        var exams = await _context.Exams
            .Where(e => e.IsPublished && e.IsActive)
            .OrderByDescending(e => e.ExamDate)
            .ToListAsync();

        var examDTOs = exams.Select(e => new ExamListDTO
        {
            ExamId = e.ExamId,
            ExamName = language == "English" ? e.ExamNameEnglish : e.ExamNameBangla,
            ExamDate = e.ExamDate,
            TotalQuestions = e.TotalQuestions,
            DurationMinutes = e.DurationMinutes,
            TotalMarks = e.TotalMarks,
            LanguageMode = e.LanguageMode.ToString(),
            IsPaid = e.IsPaid,
            ExamFee = e.ExamFee,
            AllowGuestUsers = e.AllowGuestUsers,
            IsPublished = e.IsPublished
        }).ToList();

        return Ok(examDTOs);
    }

    [HttpGet("{examId}")]
    public async Task<ActionResult<ExamDetailDTO>> GetExamDetail(Guid examId, [FromQuery] string? language = "Bangla")
    {
        var exam = await _context.Exams
            .Include(e => e.SubjectDistributions)
            .ThenInclude(sd => sd.Subject)
            .FirstOrDefaultAsync(e => e.ExamId == examId);

        if (exam == null)
        {
            return NotFound(new { message = "Exam not found" });
        }

        var examDTO = new ExamDetailDTO
        {
            ExamId = exam.ExamId,
            ExamName = language == "English" ? exam.ExamNameEnglish : exam.ExamNameBangla,
            ExamDate = exam.ExamDate,
            TotalQuestions = exam.TotalQuestions,
            DurationMinutes = exam.DurationMinutes,
            TotalMarks = exam.TotalMarks,
            LanguageMode = exam.LanguageMode.ToString(),
            IsPaid = exam.IsPaid,
            ExamFee = exam.ExamFee,
            AllowGuestUsers = exam.AllowGuestUsers,
            SubjectDistributions = exam.SubjectDistributions.Select(sd => new SubjectDistributionDTO
            {
                SubjectName = language == "English" ? sd.Subject.SubjectNameEnglish : sd.Subject.SubjectNameBangla,
                TotalQuestions = sd.TotalQuestions,
                EasyQuestions = sd.EasyQuestions,
                IntermediateQuestions = sd.IntermediateQuestions,
                HardQuestions = sd.HardQuestions
            }).ToList()
        };

        return Ok(examDTO);
    }

    [HttpPost("start")]
    public async Task<ActionResult<StartExamResponse>> StartExam(StartExamRequest request)
    {
        var exam = await _context.Exams.FindAsync(request.ExamId);

        if (exam == null)
        {
            return NotFound(new StartExamResponse
            {
                Success = false,
                Message = "Exam not found"
            });
        }

        // Get user ID from token if authenticated
        Guid? userId = null;
        if (User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var parsedUserId))
            {
                userId = parsedUserId;
            }
        }

        // For guest users, email is required
        if (userId == null && string.IsNullOrEmpty(request.GuestEmail))
        {
            return BadRequest(new StartExamResponse
            {
                Success = false,
                Message = "Email is required for guest users"
            });
        }

        // Check if user already attempted this exam
        var existingAttempt = await _context.ExamAttempts
            .Where(ea => ea.ExamId == request.ExamId)
            .Where(ea => userId.HasValue ? ea.UserId == userId : ea.GuestEmail == request.GuestEmail)
            .FirstOrDefaultAsync();

        if (existingAttempt != null)
        {
            return BadRequest(new StartExamResponse
            {
                Success = false,
                Message = "You have already attempted this exam"
            });
        }

        // Create exam attempt
        var attempt = new ExamAttempt
        {
            AttemptId = Guid.NewGuid(),
            ExamId = request.ExamId,
            UserId = userId,
            GuestEmail = request.GuestEmail,
            SelectedLanguage = request.SelectedLanguage,
            StartedAt = DateTime.UtcNow,
            IsCompleted = false,
            TabSwitchCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.ExamAttempts.Add(attempt);
        await _context.SaveChangesAsync();

        return Ok(new StartExamResponse
        {
            Success = true,
            Message = "Exam started successfully",
            AttemptId = attempt.AttemptId,
            StartedAt = attempt.StartedAt
        });
    }

    [HttpGet("questions/{attemptId}")]
    public async Task<ActionResult<List<ExamQuestionDTO>>> GetExamQuestions(Guid attemptId)
    {
        var attempt = await _context.ExamAttempts
            .Include(ea => ea.Exam)
            .FirstOrDefaultAsync(ea => ea.AttemptId == attemptId);

        if (attempt == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        var questions = await _context.ExamQuestions
            .Include(eq => eq.Question)
            .ThenInclude(q => q.Options)
            .Include(eq => eq.Question.Subject)
            .Include(eq => eq.Question.Topic)
            .Where(eq => eq.ExamId == attempt.ExamId)
            .OrderBy(eq => eq.DisplayOrder)
            .ToListAsync();

        var language = attempt.SelectedLanguage;
        var questionDTOs = questions.Select(eq => new ExamQuestionDTO
        {
            QuestionId = eq.Question.QuestionId,
            QuestionNumber = eq.QuestionNumber,
            QuestionText = language == "English" ? eq.Question.QuestionTextEnglish : eq.Question.QuestionTextBangla,
            QuestionImageUrl = eq.Question.QuestionImageUrl,
            HasMathContent = eq.Question.HasMathContent,
            SubjectName = language == "English" ? eq.Question.Subject.SubjectNameEnglish : eq.Question.Subject.SubjectNameBangla,
            TopicName = language == "English" ? eq.Question.Topic.TopicNameEnglish : eq.Question.Topic.TopicNameBangla,
            DifficultyLevel = eq.Question.DifficultyLevel.ToString(),
            Marks = eq.Question.Marks,
            Options = eq.Question.Options.OrderBy(o => o.OptionOrder).Select(o => new QuestionOptionDTO
            {
                OptionId = o.OptionId,
                OptionText = language == "English" ? o.OptionTextEnglish : o.OptionTextBangla,
                OptionOrder = o.OptionOrder
            }).ToList()
        }).ToList();

        return Ok(questionDTOs);
    }

    [HttpPost("submit-answer")]
    public async Task<ActionResult> SubmitAnswer(SubmitAnswerRequest request)
    {
        var attempt = await _context.ExamAttempts.FindAsync(request.AttemptId);

        if (attempt == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        if (attempt.IsCompleted)
        {
            return BadRequest(new { message = "Exam already completed" });
        }

        // Check if answer already exists
        var existingAnswer = await _context.ExamAnswers
            .FirstOrDefaultAsync(ea => ea.AttemptId == request.AttemptId && ea.QuestionId == request.QuestionId);

        if (existingAnswer != null)
        {
            // Update existing answer
            existingAnswer.SelectedOptionId = request.SelectedOptionId;
            existingAnswer.TimeSpentSeconds = request.TimeSpentSeconds;
            existingAnswer.IsMarkedForReview = request.IsMarkedForReview;
            existingAnswer.AnsweredAt = DateTime.UtcNow;
        }
        else
        {
            // Create new answer
            var answer = new ExamAnswer
            {
                AnswerId = Guid.NewGuid(),
                AttemptId = request.AttemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                TimeSpentSeconds = request.TimeSpentSeconds,
                IsMarkedForReview = request.IsMarkedForReview,
                AnsweredAt = DateTime.UtcNow
            };

            _context.ExamAnswers.Add(answer);
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Answer saved successfully" });
    }

    [HttpPost("submit")]
    public async Task<ActionResult<ExamResultDTO>> SubmitExam(SubmitExamRequest request)
    {
        var attempt = await _context.ExamAttempts
            .Include(ea => ea.Exam)
            .Include(ea => ea.ExamAnswers)
            .FirstOrDefaultAsync(ea => ea.AttemptId == request.AttemptId);

        if (attempt == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        if (attempt.IsCompleted)
        {
            return BadRequest(new { message = "Exam already submitted" });
        }

        // Calculate results
        var totalQuestions = attempt.Exam.TotalQuestions;
        var answeredQuestions = attempt.ExamAnswers.Count(ea => ea.SelectedOptionId != null);
        var unansweredQuestions = totalQuestions - answeredQuestions;

        decimal totalMarks = 0;
        int correctAnswers = 0;
        int wrongAnswers = 0;

        foreach (var answer in attempt.ExamAnswers.Where(ea => ea.SelectedOptionId != null))
        {
            var correctOption = await _context.QuestionOptions
                .FirstOrDefaultAsync(qo => qo.QuestionId == answer.QuestionId && qo.IsCorrect);

            if (correctOption != null && answer.SelectedOptionId == correctOption.OptionId)
            {
                var question = await _context.Questions.FindAsync(answer.QuestionId);
                answer.IsCorrect = true;
                answer.MarksObtained = question?.Marks ?? 1;
                totalMarks += answer.MarksObtained.Value;
                correctAnswers++;
            }
            else
            {
                answer.IsCorrect = false;
                answer.MarksObtained = 0;
                wrongAnswers++;
            }
        }

        var accuracy = answeredQuestions > 0 ? (decimal)correctAnswers / answeredQuestions * 100 : 0;
        var totalTimeSpent = (int)(DateTime.UtcNow - attempt.StartedAt).TotalSeconds;

        // Update attempt
        attempt.SubmittedAt = DateTime.UtcNow;
        attempt.TotalTimeSpentSeconds = totalTimeSpent;
        attempt.TotalMarksObtained = totalMarks;
        attempt.TotalCorrectAnswers = correctAnswers;
        attempt.TotalWrongAnswers = wrongAnswers;
        attempt.TotalUnanswered = unansweredQuestions;
        attempt.AccuracyPercentage = accuracy;
        attempt.IsCompleted = true;

        // Calculate rank
        var rank = await _context.ExamAttempts
            .Where(ea => ea.ExamId == attempt.ExamId && ea.IsCompleted)
            .Where(ea => ea.TotalMarksObtained > totalMarks)
            .CountAsync() + 1;

        attempt.Rank = rank;

        await _context.SaveChangesAsync();

        // Get badge
        var badge = await _context.BadgeConfigurations
            .Where(b => b.IsActive && totalMarks >= b.MinMarks && totalMarks <= b.MaxMarks)
            .FirstOrDefaultAsync();

        var language = attempt.SelectedLanguage;
        var result = new ExamResultDTO
        {
            AttemptId = attempt.AttemptId,
            ExamName = language == "English" ? attempt.Exam.ExamNameEnglish : attempt.Exam.ExamNameBangla,
            TotalMarksObtained = totalMarks,
            TotalMarks = attempt.Exam.TotalMarks,
            TotalCorrectAnswers = correctAnswers,
            TotalWrongAnswers = wrongAnswers,
            TotalUnanswered = unansweredQuestions,
            AccuracyPercentage = accuracy,
            TotalTimeSpentSeconds = totalTimeSpent,
            Rank = rank,
            BadgeName = badge?.BadgeName ?? "N/A",
            BadgeColor = badge?.BadgeColor ?? "#000000",
            IsPassed = totalMarks >= 100 // Default pass mark
        };

        return Ok(result);
    }

    [HttpGet("result/{attemptId}")]
    public async Task<ActionResult<ExamResultDTO>> GetExamResult(Guid attemptId)
    {
        var attempt = await _context.ExamAttempts
            .Include(ea => ea.Exam)
            .FirstOrDefaultAsync(ea => ea.AttemptId == attemptId);

        if (attempt == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        if (!attempt.IsCompleted)
        {
            return BadRequest(new { message = "Exam not yet completed" });
        }

        var badge = await _context.BadgeConfigurations
            .Where(b => b.IsActive && attempt.TotalMarksObtained >= b.MinMarks && attempt.TotalMarksObtained <= b.MaxMarks)
            .FirstOrDefaultAsync();

        var language = attempt.SelectedLanguage;
        var result = new ExamResultDTO
        {
            AttemptId = attempt.AttemptId,
            ExamName = language == "English" ? attempt.Exam.ExamNameEnglish : attempt.Exam.ExamNameBangla,
            TotalMarksObtained = attempt.TotalMarksObtained ?? 0,
            TotalMarks = attempt.Exam.TotalMarks,
            TotalCorrectAnswers = attempt.TotalCorrectAnswers ?? 0,
            TotalWrongAnswers = attempt.TotalWrongAnswers ?? 0,
            TotalUnanswered = attempt.TotalUnanswered ?? 0,
            AccuracyPercentage = attempt.AccuracyPercentage ?? 0,
            TotalTimeSpentSeconds = attempt.TotalTimeSpentSeconds ?? 0,
            Rank = attempt.Rank ?? 0,
            BadgeName = badge?.BadgeName ?? "N/A",
            BadgeColor = badge?.BadgeColor ?? "#000000",
            IsPassed = (attempt.TotalMarksObtained ?? 0) >= 100
        };

        return Ok(result);
    }

    [HttpGet("review/{attemptId}")]
    public async Task<ActionResult<List<QuestionReviewDTO>>> GetQuestionReview(Guid attemptId)
    {
        var attempt = await _context.ExamAttempts
            .Include(ea => ea.ExamAnswers)
            .FirstOrDefaultAsync(ea => ea.AttemptId == attemptId);

        if (attempt == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        if (!attempt.IsCompleted)
        {
            return BadRequest(new { message = "Exam not yet completed" });
        }

        var examQuestions = await _context.ExamQuestions
            .Include(eq => eq.Question)
            .ThenInclude(q => q.Options)
            .Include(eq => eq.Question.Subject)
            .Include(eq => eq.Question.Topic)
            .Include(eq => eq.Question.Explanation)
            .Where(eq => eq.ExamId == attempt.ExamId)
            .OrderBy(eq => eq.QuestionNumber)
            .ToListAsync();

        var language = attempt.SelectedLanguage;
        var reviewDTOs = new List<QuestionReviewDTO>();

        foreach (var eq in examQuestions)
        {
            var userAnswer = attempt.ExamAnswers.FirstOrDefault(ea => ea.QuestionId == eq.Question.QuestionId);
            var correctOption = eq.Question.Options.FirstOrDefault(o => o.IsCorrect);

            reviewDTOs.Add(new QuestionReviewDTO
            {
                QuestionId = eq.Question.QuestionId,
                QuestionNumber = eq.QuestionNumber,
                QuestionText = language == "English" ? eq.Question.QuestionTextEnglish : eq.Question.QuestionTextBangla,
                QuestionImageUrl = eq.Question.QuestionImageUrl,
                Options = eq.Question.Options.OrderBy(o => o.OptionOrder).Select(o => new QuestionOptionReviewDTO
                {
                    OptionId = o.OptionId,
                    OptionText = language == "English" ? o.OptionTextEnglish : o.OptionTextBangla,
                    OptionOrder = o.OptionOrder,
                    IsCorrect = o.IsCorrect
                }).ToList(),
                UserSelectedOptionId = userAnswer?.SelectedOptionId,
                CorrectOptionId = correctOption?.OptionId ?? Guid.Empty,
                IsCorrect = userAnswer?.IsCorrect ?? false,
                MarksObtained = userAnswer?.MarksObtained ?? 0,
                TimeSpentSeconds = userAnswer?.TimeSpentSeconds ?? 0,
                Explanation = language == "English" ? eq.Question.Explanation?.ExplanationEnglish ?? "" : eq.Question.Explanation?.ExplanationBangla ?? "",
                SubjectName = language == "English" ? eq.Question.Subject.SubjectNameEnglish : eq.Question.Subject.SubjectNameBangla,
                TopicName = language == "English" ? eq.Question.Topic.TopicNameEnglish : eq.Question.Topic.TopicNameBangla,
                DifficultyLevel = eq.Question.DifficultyLevel.ToString(),
                SourceReference = eq.Question.SourceReference
            });
        }

        return Ok(reviewDTOs);
    }
}
